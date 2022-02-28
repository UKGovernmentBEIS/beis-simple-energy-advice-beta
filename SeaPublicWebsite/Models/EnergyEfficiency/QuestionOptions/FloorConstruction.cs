using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum FloorConstruction
    { 
        [GovUkRadioCheckboxLabelText(Text = "Suspended timber")]
        SuspendedTimber,
        [GovUkRadioCheckboxLabelText(Text = "Solid concrete")]
        SolidConcrete,
        [GovUkRadioCheckboxLabelText(Text = "A mix of both")]
        Mix,
        [GovUkRadioCheckboxLabelText(Text = "I don't know")]
        DoNotKnow,
    }
}
