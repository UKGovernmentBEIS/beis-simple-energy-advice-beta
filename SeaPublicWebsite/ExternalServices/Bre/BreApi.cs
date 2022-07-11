using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.ExternalServices.Bre
{
    public class BreApi
    {
        private readonly BreConfiguration configuration;
        private readonly ILogger<BreApi> logger;

        public BreApi(IOptions<BreConfiguration> options,
            ILogger<BreApi> logger)
        {
            configuration = options.Value;
            this.logger = logger;
        }

        public async Task<List<BreRecommendation>> GetRecommendationsForPropertyRequestAsync(BreRequest request)
        {
            BreResponse response = null;
            try
            {
                string username = configuration.Username;
                string password = configuration.Password;
                Guid nonce = Guid.NewGuid();
                string created = DateTime.Now.ToUniversalTime().ToString
                    (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";
                string token = GenerateToken(password + nonce + username + created);
                string wsseHeader =
                    $"WSSE UsernameToken Token=\"{token}\", Nonce=\"{nonce}\", Username=\"{username}\", Created=\"{created}\"";
                string requestString = JsonConvert.SerializeObject(request);
                StringContent stringContent = new(requestString);
                
                response = await HttpRequestHelper.SendPostRequestAsync<BreResponse>(
                    new RequestParameters
                    {
                        BaseAddress = configuration.BaseUrl,
                        Path = "/v2/energy_use",
                        Auth = new AuthenticationHeaderValue("Basic", wsseHeader),
                        Body = stringContent
                    }
                );
                
                List<BreRecommendation> recommendations = new List<BreRecommendation>();

                if (response.Measures is null)
                {
                    return recommendations;
                }
                
                Dictionary<string, BreRecommendation> recommendationDictionary =
                    RecommendationService.RecommendationDictionary;
                foreach (KeyValuePair<string, BreMeasure> entry in response.Measures)
                {
                    string key = entry.Key;
                    BreMeasure measure = entry.Value;
                    BreRecommendation dictEntry = recommendationDictionary[key];
                    recommendations.Add(new BreRecommendation()
                    {
                        Key = dictEntry.Key,
                        Title = dictEntry.Title,
                        MinInstallCost = measure.MinInstallationCost,
                        MaxInstallCost = measure.MaxInstallationCost,
                        Saving = (int) measure.Saving,
                        LifetimeSaving = (int) (measure.Lifetime * measure.Saving),
                        Lifetime = measure.Lifetime,
                        Summary = dictEntry.Summary
                    });
                }
                return recommendations;
            }
            catch (Exception e)
            {
                // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
                logger.LogError(@$"There was an error calling the BRE API: {e.Message}

BreRequest details: {JsonConvert.SerializeObject(request)}

BreResponse details: {JsonConvert.SerializeObject(response)}");

                throw;
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

        internal class BreResponse
        {
            [JsonProperty(PropertyName = "measures")]
            public Dictionary<string, BreMeasure> Measures { get; set; }
        }

        internal class BreMeasure
        {
            [JsonProperty(PropertyName = "min_installation_cost")]
            public int MinInstallationCost { get; set; }

            [JsonProperty(PropertyName = "max_installation_cost")]
            public int MaxInstallationCost { get; set; }

            [JsonProperty(PropertyName = "cost_saving")]
            public decimal Saving { get; set; }

            [JsonProperty(PropertyName = "lifetime")]
            public int Lifetime { get; set; }
        }
    }
}