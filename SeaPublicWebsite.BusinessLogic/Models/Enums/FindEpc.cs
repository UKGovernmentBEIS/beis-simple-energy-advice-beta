using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum FindEpc
{
    [GovUkRadioCheckboxLabelText(Text = "Yes")]
    Yes,

    [GovUkRadioCheckboxLabelText(Text = "No")]
    No,
}