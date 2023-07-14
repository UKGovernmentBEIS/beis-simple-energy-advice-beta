using System.Text.Json.Serialization;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;

public class EpbAddressDto
{
    [JsonPropertyName("addressLine1")]
    public string Address1 { get; set; }
        
    [JsonPropertyName("addressLine2")]
    public string Address2 { get; set; }
        
    [JsonPropertyName("addressLine3")]
    public string Address3 { get; set; }
        
    [JsonPropertyName("addressLine4")]
    public string Address4 { get; set; }
        
    [JsonPropertyName("town")]
    public string Town { get; set; }
        
    [JsonPropertyName("postcode")]
    public string Postcode { get; set; }
}