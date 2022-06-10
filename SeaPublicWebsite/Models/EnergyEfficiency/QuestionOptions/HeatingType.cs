using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HeatingType
    {
        [GovUkRadioCheckboxLabelText(Text = "I'm not sure")]
        DoNotKnow, 
        [GovUkRadioCheckboxLabelText(Text = "Gas boiler")]
        GasBoiler,
        [GovUkRadioCheckboxLabelText(Text = "Oil boiler")]
        OilBoiler,
        [GovUkRadioCheckboxLabelText(Text = "LPG boiler")]
        LpgBoiler,
        [GovUkRadioCheckboxLabelText(Text = "Storage heaters")]
        Storage,
        [GovUkRadioCheckboxLabelText(Text = "Direct acting electric heating")]
        DirectActionElectric,
        [GovUkRadioCheckboxLabelText(Text = "Heat pump")]
        HeatPump,
        [GovUkRadioCheckboxLabelText(Text = "Other")]
        Other,
    }
}
