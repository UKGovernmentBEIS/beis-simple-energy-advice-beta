using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum WallType
    {
        [GovUkRadioCheckboxLabelText(Text = "Cavity and uninsulated")]
        CavityNoInsulation = 1,
        [GovUkRadioCheckboxLabelText(Text = "Cavity and insulated")]
        CavityInsulated = 2,
        [GovUkRadioCheckboxLabelText(Text = "Solid and uninsulated")]
        SolidNoInsulation = 3,
        [GovUkRadioCheckboxLabelText(Text = "Solid and insulated")]
        SolidInsulated = 4,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow = 0,
    }
}