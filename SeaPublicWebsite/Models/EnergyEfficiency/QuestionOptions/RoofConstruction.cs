using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum RoofConstruction
    { 
        [GovUkRadioCheckboxLabelText(Text = "Flat")]
        Flat,
        [GovUkRadioCheckboxLabelText(Text = "Pitched at an angle")]
        Pitched,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow
    }
}
