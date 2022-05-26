using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions
{
    public enum HeatingPattern
    {
        [GovUkRadioCheckboxLabelText(Text = "All day and all night")]
        AllDayAndNight,
        [GovUkRadioCheckboxLabelText(Text = "All day and off at night")]
        AllDayNotNight,
        [GovUkRadioCheckboxLabelText(Text = "Morning and evening")]
        MorningAndEvening,
        [GovUkRadioCheckboxLabelText(Text = "Less frequently")]
        OnceADay,
        [GovUkRadioCheckboxLabelText (Text = "Something else")]
        Other
    }
}