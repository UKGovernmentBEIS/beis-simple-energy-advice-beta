using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.Models.Epb;

public class EpbAssessmentsDto
{
    [JsonProperty(PropertyName = "data")]
    public EpbAssessmentsDataDto Data { get; set; }
}

public class EpbAssessmentsDataDto
{
    [JsonProperty(PropertyName = "assessments")]
    public List<EpbAssessmentId> Assessments { get; set; }
}

public class EpbAssessmentId
{
    [JsonProperty(PropertyName = "epcRrn")]
    public string EpcId { get; set; }
    
    [JsonProperty(PropertyName = "address")]
    public EpbAddressDto Address { get; set; }
}