using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum FloorInsulated
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "No, my floors are not insulated")]
        No,
        [GovUkRadioCheckboxLabelText(Text = "Yes, my floor is insulated")]
        Yes,
    }
}