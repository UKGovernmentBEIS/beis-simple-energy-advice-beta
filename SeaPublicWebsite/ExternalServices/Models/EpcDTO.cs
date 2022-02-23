using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.Models
{
    public class EpcDTO
    {
        // Individual lodgement identifier. Guaranteed to be unique and can be used to identify a
        // certificate in the downloads and the API.
        [JsonProperty(PropertyName = "lmk-key")]
        public string LmkKey { get; set; }
        // Unique identifier for the property.
        [JsonProperty(PropertyName = "building-reference-number")]
        public string BuildingReference { get; set; }
        [JsonProperty(PropertyName = "inspection-date")]
        public string InspectionDate { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
    }
}