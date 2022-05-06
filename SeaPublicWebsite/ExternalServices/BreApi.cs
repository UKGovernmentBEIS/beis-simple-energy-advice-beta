using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.ExternalServices
{
    public static class BreApi
    {
        public static List<Recommendation> GetRecommendationsForUserRequest(string requestString)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://uat.brewebserv.com");

                    string username = Global.BreUsername;
                    string password = Global.BrePassword;
                    string nonce = GenerateNonce();
                    string created = DateTime.Now.ToUniversalTime().ToString
                        (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";
                    string token = GenerateToken(password + nonce + username + created);
                    string wsseHeader =
                        $"WSSE UsernameToken Token=\"{token}\", Nonce=\"{nonce}\", Username=\"{username}\", Created=\"{created}\"";
                    httpClient.DefaultRequestHeaders.Add("Authorization", wsseHeader);

                    string path = "/bemapi/energy_use";
                    StringContent stringContent = new(requestString);
                    HttpResponseMessage response = httpClient.PostAsync(path, stringContent).Result;

                    Console.WriteLine(response.ToString());
                    if (response.IsSuccessStatusCode)
                    {
                        string bodyString = response.Content.ReadAsStringAsync().Result;
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
                        Console.WriteLine($"{recommendations.Count} recommendations found");
                        return recommendations;
                    }

                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static string GenerateNonce()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new();
            return new string(Enumerable.Repeat(chars, 64)
                .Select(s => s[random.Next(s.Length)]).ToArray());
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