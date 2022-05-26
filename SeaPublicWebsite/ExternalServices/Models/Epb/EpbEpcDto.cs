using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.Models.Epb;

public class EpbEpcDto
{
    [JsonProperty(PropertyName = "data")]
    public EpbEpcDataDto Data { get; set; }
}

public class EpbEpcDataDto
{
    [JsonProperty(PropertyName = "assessment")]
    public EpbEpcAssessmentDto Assessment { get; set; }
}

public class EpbEpcAssessmentDto
{
    [JsonProperty(PropertyName = "typeOfAssessment")]
    public string AssessmentType { get; set; }
    
    [JsonProperty(PropertyName = "address")]
    public EpbAddressDto Address { get; set; }
    
    [JsonProperty(PropertyName = "lodgementDate")]
    public string LodgementDate { get; set; }
    
    [JsonProperty(PropertyName = "isLatestAssessmentForAddress")]
    public bool IsLatestAssessmentForAddress { get; set; }
    
    [JsonProperty(PropertyName = "propertyType")]
    public string PropertyType { get; set; }
    
    [JsonProperty(PropertyName = "builtForm")]
    public string BuiltForm { get; set; }
    
    [JsonProperty(PropertyName = "PropertyAgeBand")]
    public string PropertyAgeBand { get; set; }

    [JsonProperty(PropertyName = "wallsDescription")]
    public List<string> WallsDescription { get; set; }
    
    [JsonProperty(PropertyName = "floorDescription")]
    public List<string> FloorDescription { get; set; }
    
    [JsonProperty(PropertyName = "roofDescription")]
    public List<string> RoofDescription { get; set; }
    
    [JsonProperty(PropertyName = "windowsDescription")]
    public List<string> WindowsDescription { get; set; }
    
    [JsonProperty(PropertyName = "mainHeatingDescription")]
    public string MainHeatingDescription { get; set; }
    
    [JsonProperty(PropertyName = "mainFuelType")]
    public string MainFuelType { get; set; }
    
    [JsonProperty(PropertyName = "hasHotWaterCylinder")]
    public bool HasHotWaterCylinder { get; set; }
}
