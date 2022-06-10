using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
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
        [GovUkRadioCheckboxLabelText(Text = "I don’t see my option listed")]
        Other
    }
}
