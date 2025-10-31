using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Common;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;

/**
 * Documentated at <see href="https://softwiretech.atlassian.net/wiki/spaces/Support/pages/21647818868/DESNZ+Support+Reference+Documentation#FWTS-BRE-Recommendations-API"/>
 */
public class BreApi(
    IOptions<BreConfiguration> options,
    ILogger<BreApi> logger)
{
    private readonly BreConfiguration configuration = options.Value;

    /**
     * Sample request/response documented in docs/sample-responses/bre-api/get-recommendations
     */
    public async Task<BreRecommendationsWithPriceCap> GetRecommendationsWithPriceCapForPropertyRequestAsync(
        BreRequest request)
    {
        // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
        logger.LogInformation("Sending BRE request: {Request}", JsonConvert.SerializeObject(request));

        try
        {
            var username = configuration.Username;
            var password = configuration.Password;
            var nonce = Guid.NewGuid();
            var created = DateTime.Now.ToUniversalTime().ToString
                (DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern) + "Z";
            var token = GenerateToken(password + nonce + username + created);
            var wsseHeader =
                $"WSSE UsernameToken Token=\"{token}\", Nonce=\"{nonce}\", Username=\"{username}\", Created=\"{created}\"";
            var requestString = JsonConvert.SerializeObject(request);
            StringContent stringContent = new(requestString);

            var apiResponse = await HttpRequestHelper.SendPostRequestAsync<BreResponse>(
                new RequestParameters
                {
                    BaseAddress = configuration.BaseUrl,
                    Path = "/v2/energy_use",
                    Auth = new AuthenticationHeaderValue("Basic", wsseHeader),
                    Body = stringContent
                }
            );

            // BRE requests and responses shouldn't contain any sensitive details so we are OK to log them as-is
            logger.LogInformation("Received BRE response: {Response}", JsonConvert.SerializeObject(apiResponse));

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
            logger.LogError("There was an error calling the BRE API: {Message}", e.Message);
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
                Saving = (int)apiMeasure.Saving,
                LifetimeSaving = (int)(apiMeasure.Lifetime * apiMeasure.Saving),
                Lifetime = apiMeasure.Lifetime,
                Summary = dictEntry.Summary
            };
        }
        catch (Exception e)
        {
            // We would prefer to return some recommendations rather than show the error page to the user.
            logger.LogError(
                "There was an error parsing a BRE recommendation. Code: {MeasureCode} Details: {Measure} Error: {Message}",
                measureCode, JsonConvert.SerializeObject(apiMeasure), e.Message);
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
            logger.LogError(
                "There was an error parsing the energy price cap date: {EnergyPriceCap}. Error: {Message}",
                energyPriceCap, e.Message);
            return null;
        }
    }

    private static string GenerateToken(string input)
    {
        return string.Concat(SHA256.HashData(Encoding.UTF8.GetBytes(input)).Select(item => item.ToString("x2")));
    }

    internal class BreResponse
    {
        [JsonProperty(PropertyName = "measures")]
        public Dictionary<string, BreMeasure> Measures { get; set; }

        [JsonProperty(PropertyName = "notes")] public BreNotes Notes { get; set; }
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