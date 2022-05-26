using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum WallType
    {
        [GovUkRadioCheckboxLabelText(Text = "Cavity and uninsulated")]
        CavityNoInsulation,
        [GovUkRadioCheckboxLabelText(Text = "Cavity and insulated")]
        CavityInsulated,
        [GovUkRadioCheckboxLabelText(Text = "Solid and uninsulated")]
        SolidNoInsulation,
        [GovUkRadioCheckboxLabelText(Text = "Solid and insulated")]
        SolidInsulated,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}