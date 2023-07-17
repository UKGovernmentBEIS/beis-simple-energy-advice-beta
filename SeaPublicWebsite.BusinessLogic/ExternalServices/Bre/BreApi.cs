using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Common;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.Bre
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
            // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
            logger.LogInformation($"Sending BRE request: {JsonSerializer.Serialize(request)}");

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
                string requestString = JsonSerializer.Serialize(request);
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
                
                // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
                logger.LogInformation($"Received BRE response: {JsonSerializer.Serialize(response)}");
                
                if (response.Measures is null)
                {
                    return new List<BreRecommendation>();
                }

                return response
                    .Measures
                    .Select(kvp => CreateRecommendation(kvp.Key, kvp.Value))
                    .Where(r => r != null)
                    .ToList();
            }
            catch (Exception e)
            {
                logger.LogError($"There was an error calling the BRE API: {e.Message}");
                throw;
            }
        }

        private BreRecommendation CreateRecommendation(string measureCode, BreMeasure measure)
        {
            try
            {
                BreRecommendation dictEntry = RecommendationService.RecommendationDictionary[measureCode];
                return new BreRecommendation
                {
                    Key = dictEntry.Key,
                    Title = dictEntry.Title,
                    MinInstallCost = measure.MinInstallationCost,
                    MaxInstallCost = measure.MaxInstallationCost,
                    Saving = (int) measure.Saving,
                    LifetimeSaving = (int) (measure.Lifetime * measure.Saving),
                    Lifetime = measure.Lifetime,
                    Summary = dictEntry.Summary
                };
            }
            catch (Exception e)
            {
                // We would prefer to return some recommendations rather than show the error page to the user.
                logger.LogError($"There was an error parsing a BRE recommendation. Code: {measureCode} Details: {JsonSerializer.Serialize(measure)} Error: {e.Message}");
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

        internal class BreResponse
        {
            [JsonPropertyName("measures")]
            public Dictionary<string, BreMeasure> Measures { get; set; }
        }

        internal class BreMeasure
        {
            [JsonPropertyName("min_installation_cost")]
            public int MinInstallationCost { get; set; }

            [JsonPropertyName("max_installation_cost")]
            public int MaxInstallationCost { get; set; }

            [JsonPropertyName("cost_saving")]
            public decimal Saving { get; set; }

            [JsonPropertyName("lifetime")]
            public int Lifetime { get; set; }
        }
    }
}