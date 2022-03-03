using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum FloorConstruction
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Suspended timber")]
        SuspendedTimber,
        [GovUkRadioCheckboxLabelText(Text = "Solid concrete")]
        SolidConcrete,
        [GovUkRadioCheckboxLabelText(Text = "A mix of both")]
        Mix,
    }
}
