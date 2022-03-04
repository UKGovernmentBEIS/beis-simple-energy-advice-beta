using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.Models
{
    public class EpcDto
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
        [JsonProperty(PropertyName = "property-type")]
        public string PropertyType { get; set; }
        [JsonProperty(PropertyName = "built-form")]
        public string BuiltForm { get; set; }
        [JsonProperty(PropertyName = "mainheat-description")]
        public string MainHeatDescription { get; set; }
        [JsonProperty(PropertyName = "main-fuel")]
        public string MainFuel { get; set; }        
        [JsonProperty(PropertyName = "walls-description")]
        public string WallsDescription { get; set; }        
        [JsonProperty(PropertyName = "floor-description")]
        public string FloorDescription { get; set; }
        [JsonProperty(PropertyName = "construction-age-band")]
        public string ConstructionAgeBand { get; set; }

    }
}