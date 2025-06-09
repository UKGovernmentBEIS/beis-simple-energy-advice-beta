using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
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

        public async Task<BreRecommendationsWithPriceCap> GetRecommendationsWithPriceCapForPropertyRequestAsync(BreRequest request)
        {
            // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
            logger.LogInformation($"Sending BRE request: {JsonConvert.SerializeObject(request)}");

            BreResponse apiResponse = null;
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
                
                apiResponse = await HttpRequestHelper.SendPostRequestAsync<BreResponse>(
                    new RequestParameters
                    {
                        BaseAddress = configuration.BaseUrl,
                        Path = "/v2/energy_use",
                        Auth = new AuthenticationHeaderValue("Basic", wsseHeader),
                        Body = stringContent
                    }
                );
                
                // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
                logger.LogInformation($"Received BRE response: {JsonConvert.SerializeObject(apiResponse)}");

                var recommendations = apiResponse.Measures switch
                {
                    null => [],
                    _ => apiResponse
                            .Measures
                            .Select(kvp => CreateRecommendation(kvp.Key, kvp.Value))
                            .Where(r => r != null)
                            .ToList()
                };

                return new BreRecommendationsWithPriceCap
                {
                    Recommendations = recommendations,
                    EnergyPriceCapInfo = CreateEnergyPriceCapNotes(apiResponse.Notes.EnergyPriceCap)
                };
            }
            catch (Exception e)
            {
                logger.LogError($"There was an error calling the BRE API: {e.Message}");
                throw;
            }
        }

        private BreRecommendation CreateRecommendation(string measureCode, BreMeasure apiMeasure)
        {
            try
            {
                BreRecommendation dictEntry = RecommendationService.RecommendationDictionary[measureCode];
                return new BreRecommendation
                {
                    Key = dictEntry.Key,
                    Title = dictEntry.Title,
                    MinInstallCost = apiMeasure.MinInstallationCost,
                    MaxInstallCost = apiMeasure.MaxInstallationCost,
                    Saving = (int) apiMeasure.Saving,
                    LifetimeSaving = (int) (apiMeasure.Lifetime * apiMeasure.Saving),
                    Lifetime = apiMeasure.Lifetime,
                    Summary = dictEntry.Summary
                };
            }
            catch (Exception e)
            {
                // We would prefer to return some recommendations rather than show the error page to the user.
                logger.LogError($"There was an error parsing a BRE recommendation. Code: {measureCode} Details: {JsonConvert.SerializeObject(apiMeasure)} Error: {e.Message}");
                return null;
            }
            
        }

        private BreEnergyPriceCapInfo CreateEnergyPriceCapNotes(string energyPriceCap)
        {
            try
            {
                var date = DateTime.ParseExact(energyPriceCap, "MMMM yyyy", CultureInfo.InvariantCulture);

                return new BreEnergyPriceCapInfo
                {
                    Year = date.Year,
                    MonthIndex = date.Month - 1 // Month is 1-based, we need 0-based index
                };
            } 
            catch (FormatException e)
            {
                logger.LogError($"There was an error parsing the energy price cap date: {energyPriceCap}. Error: {e.Message}");
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
            [JsonProperty(PropertyName = "measures")]
            public Dictionary<string, BreMeasure> Measures { get; set; }
            [JsonProperty(PropertyName = "notes")]
            public BreNotes Notes { get; set; }
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

        internal class BreNotes
        {
            [JsonProperty(PropertyName = "energy_price_cap")]
            public string EnergyPriceCap { get; set; }
        }
    }
}