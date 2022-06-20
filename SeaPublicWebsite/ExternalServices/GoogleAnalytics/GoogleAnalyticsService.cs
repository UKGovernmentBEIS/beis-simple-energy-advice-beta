using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices.GoogleAnalytics;

public class GoogleAnalyticsService
{
    public readonly GoogleAnalyticsConfiguration Configuration;
    
    public GoogleAnalyticsService(IOptions<GoogleAnalyticsConfiguration> options)
    {
        this.Configuration = options.Value;
    }
    
    public async Task SendEvent(GaRequestBody body)
    {
        await HttpRequestHelper.SendPostRequestAsync<string>(new RequestParameters
        {
            BaseAddress = Configuration.BaseUrl,
            Path = $"/mp/collect?api_secret={Configuration.ApiSecret}&measurement_id={Configuration.MeasurementId}",
            Body = new StringContent(JsonConvert.SerializeObject(body))
        });
    }
}

public class GaRequestBody
{
    [JsonProperty(PropertyName = "client_id")]
    public string ClientId { get; set; }
    
    [JsonProperty(PropertyName = "events")]
    public List<GaEvent> GaEvents { get; set; }
}

public class GaEvent
{
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "params")]
    public object Parameters { get; set; }
}