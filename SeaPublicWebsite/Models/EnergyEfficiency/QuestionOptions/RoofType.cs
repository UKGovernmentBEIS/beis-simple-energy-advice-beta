using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
     public enum RoofType
    {

        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow,
        [GovUkRadioCheckboxLabelText(Text = "Pitched uninsulated roof")]
        PitchedNoInsulation = 1,
        [GovUkRadioCheckboxLabelText(Text = "Pitched insulated roof")]
        PitchedInsulated = 2,
        [GovUkRadioCheckboxLabelText(Text = "Flat uninsulated roof")]
        FlatNoInsulation = 3,
        [GovUkRadioCheckboxLabelText(Text = "Flat uninsulated roof")]
        FlatInsulated = 4,
    }
}