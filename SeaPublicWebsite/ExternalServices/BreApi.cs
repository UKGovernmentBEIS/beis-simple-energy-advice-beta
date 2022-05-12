using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.ExternalServices
{
    public static class BreApi
    {
        public static async Task<List<Recommendation>> GetRecommendationsForUserRequestAsync(BreRequest request)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri(Global.BreBaseAddress);

                    string username = Global.BreUsername;
                    string password = Global.BrePassword;
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

                        List<Recommendation> recommendations = new List<Recommendation>();
                        foreach (JProperty prop in measures.Properties())
                        {
                            Dictionary<string, Recommendation> recommendationDictionary =
                                RecommendationService.RecommendationDictionary;
                            JToken value = prop.Value;
                            int minInstallCost = (int) value["min_installation_cost"];
                            int maxInstallCost = (int) value["max_installation_cost"];
                            int saving = (int) value["cost_saving"];
                            int lifetime = (int) value["lifetime"];

                            Recommendation recommendation = new()
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
                            recommendations.Add(recommendation);
                        }
                        return recommendations;
                    }

                    throw new ArgumentNullException();
                }
            }
            catch (Exception)
            {
                // TODO: seabeta-192 to add a log here
                return null;
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