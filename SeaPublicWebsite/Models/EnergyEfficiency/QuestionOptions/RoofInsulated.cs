using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum RoofInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Yes, there is at least 200mm of insulation in my loft")]
        Yes,
        [GovUkRadioCheckboxLabelText(Text = "No, there is less than 200mm of insulation in my loft")]
        No,
    }
}