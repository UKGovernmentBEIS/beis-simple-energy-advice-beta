using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.ExternalServices.Bre
{
    public class BreApi
    {
        private static BreConfiguration configuration;

        public BreApi(IOptions<BreConfiguration> options)
        {
            configuration = options.Value;
        }

        public static async Task<List<BreRecommendation>> GetRecommendationsForUserRequestAsync(BreRequest request)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(configuration.BaseUrl);

                    string username = configuration.Username;
                    string password = configuration.Password;
                    Guid nonce =  Guid.NewGuid();
                    string created = DateTime.Now.ToUniversalTime().ToString
                        (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";
                    string token = GenerateToken(password + nonce + username + created);
                    string wsseHeader =
                        $"WSSE UsernameToken Token=\"{token}\", Nonce=\"{nonce}\", Username=\"{username}\", Created=\"{created}\"";
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", wsseHeader);

                    string path = "/bemapi/energy_use";
                    string requestString = JsonConvert.SerializeObject(request);
                    StringContent stringContent = new(requestString);
                    HttpResponseMessage response = await httpClient.PostAsync(path, stringContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string bodyString = await response.Content.ReadAsStringAsync();
                        JObject measures = JObject.FromObject(JObject.Parse(bodyString)["measures"] ?? new JObject());

                        List<BreRecommendation> recommendations = new List<BreRecommendation>();
                        foreach (JProperty prop in measures.Properties())
                        {
                            Dictionary<string, BreRecommendation> recommendationDictionary =
                                RecommendationService.RecommendationDictionary;
                            JToken value = prop.Value;
                            int minInstallCost = (int) value["min_installation_cost"];
                            int maxInstallCost = (int) value["max_installation_cost"];
                            int saving = (int) value["cost_saving"];
                            int lifetime = (int) value["lifetime"];

                            BreRecommendation breRecommendation = new()
                            {
                                Key = recommendationDictionary[prop.Name].Key,
                                Title = recommendationDictionary[prop.Name].Title,
                                MinInstallCost = minInstallCost,
                                MaxInstallCost = maxInstallCost,
                                Saving = saving,
                                LifetimeSaving = lifetime * saving,
                                Lifetime = lifetime,
                                Summary = recommendationDictionary[prop.Name].Summary
                            };
                            recommendations.Add(breRecommendation);
                        }
                        return recommendations;
                    }

                    throw new Exception($"BRE API returned an error: {response.StatusCode}");
                }
            }
            catch (Exception e)
            {
                // TODO: seabeta-192 to add a log here
                throw new Exception($"BRE API returned an error: {e.Message}");

            }
        }

        private static string GenerateToken(string input)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(input))
                    .Select(item => item.ToString("x2")));
            }
        }
    }
}