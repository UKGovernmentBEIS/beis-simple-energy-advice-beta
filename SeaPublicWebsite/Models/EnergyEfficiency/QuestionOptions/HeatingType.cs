using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HeatingType
    {
        [GovUkRadioCheckboxLabelText(Text = "Gas boiler")]
        GasBoiler,
        [GovUkRadioCheckboxLabelText(Text = "Oil boiler")]
        OilBoiler,
        [GovUkRadioCheckboxLabelText(Text = "LPG boiler")]
        LPGBoiler,
        [GovUkRadioCheckboxLabelText(Text = "Storage heater")]
        Storage,
        [GovUkRadioCheckboxLabelText(Text = "Other electric heating")]
        OtherElectric,
        [GovUkRadioCheckboxLabelText(Text = "Heat pump")]
        HeatPump,
        [GovUkRadioCheckboxLabelText(Text = "Coal")]
        Coal
    }
}
