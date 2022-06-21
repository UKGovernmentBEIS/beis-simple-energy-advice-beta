using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class FindEpc1ViewModel : QuestionFlowViewModel
{
    [GovUkValidateRequired(ErrorMessageIfMissing =
        "Please confirm whether or not you would like to search for your EPC before continuing")]
    public FindEpc? FindEpc { get; set; }

    public string Reference { get; set; }
}