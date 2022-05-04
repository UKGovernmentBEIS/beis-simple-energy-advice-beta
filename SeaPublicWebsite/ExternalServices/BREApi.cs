using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json.Linq;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency;


namespace SeaPublicWebsite.ExternalServices
{
    public class BreApi
    {
        public static List<Recommendation> EnergyUse(string data)
        {
            try
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new();
                var nonce = new string(Enumerable.Repeat(chars, 64)
                    .Select(s => s[random.Next(s.Length)]).ToArray());

                var created = DateTime.Now.ToUniversalTime().ToString
                    (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";

                var username = "softwire0.0.1";

                var password = "9b052a18df10baade8c7448f3";

                string sha256_hash(string value)
                {
                    using (var hash = SHA256.Create())
                    {
                        return string.Concat(hash
                            .ComputeHash(Encoding.UTF8.GetBytes(value))
                            .Select(item => item.ToString("x2")));
                    }
                }

                var token = sha256_hash(password + nonce + username + created);
                var wsse_header = string.Join("", "WSSE UsernameToken ", "Token=\"", token, "\", ", "Nonce=\"", nonce,
                    "\", ", "Username=\"", username, "\", ", "Created=\"", created, "\"");
                

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", wsse_header);
                    httpClient.BaseAddress = new Uri("https://uat.brewebserv.com");

                    var path = "/bemapi/energy_use";
                    StringContent stringContent = new(data);

                    var response = httpClient.PostAsync(path, stringContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var bodyString = response.Content.ReadAsStringAsync().Result;
                        JObject measures = JObject.FromObject(JObject.Parse(bodyString)["measures"]);

                        IList<Recommendation> recommendations = new List<Recommendation>();
                        foreach (JProperty prop in measures.Properties())
                        {
                            var value = prop.Value;
                            var minInstallCost = (int) value["min_installation_cost"];
                            var maxInstallCost = (int) value["max_installation_cost"];
                            var saving = (int) value["cost_saving"];
                            var lifetime = (int) value["lifetime"];

                            Recommendation recommendation = new()
                            {
                                Key = RecommendationKey.AddLoftInsulation,
                                Title = prop.Name,
                                MinInstallCost = minInstallCost,
                                MaxInstallCost = maxInstallCost,
                                Saving = saving,
                                LifetimeSaving = lifetime * saving,
                                Lifetime = lifetime,
                                Summary = prop.Name
                            };
                            recommendations.Add(recommendation);
                        }

                        foreach (var recommendation in recommendations) Console.WriteLine(recommendation.Title);
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }
    }

    public class BreApiResponse
    {
        public bool Result { get; set; }
    }
    
}