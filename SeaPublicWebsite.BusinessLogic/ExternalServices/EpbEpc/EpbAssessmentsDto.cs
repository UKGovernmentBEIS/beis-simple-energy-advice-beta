using System.Text.Json.Serialization;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;

public class EpbAssessmentsDto
{
    [JsonPropertyName("data")]
    public EpbAssessmentsDataDto Data { get; set; }
}

public class EpbAssessmentsDataDto
{
    [JsonPropertyName("assessments")]
    public List<EpbAssessmentInformation> Assessments { get; set; }
}

public class EpbAssessmentInformation
{
    [JsonPropertyName("epcRrn")]
    public string EpcId { get; set; }
    
    [JsonPropertyName("address")]
    public EpbAddressDto Address { get; set; }
}