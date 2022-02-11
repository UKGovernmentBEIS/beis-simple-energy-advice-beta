using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum HomeAge
    {
        [GovUkRadioCheckboxLabelText(Text = "Before 1900")]
        Pre1900,
        [GovUkRadioCheckboxLabelText(Text = "1900 - 1929")]
        From1900To1929,
        [GovUkRadioCheckboxLabelText(Text = "1930 - 1949")]
        From1930To1949,
        [GovUkRadioCheckboxLabelText(Text = "1950 - 1966")]
        From1950To1966,
        [GovUkRadioCheckboxLabelText(Text = "1967 - 1975")]
        From1967To1975,
        [GovUkRadioCheckboxLabelText(Text = "1976 - 1982")]
        From1976To1982,
        [GovUkRadioCheckboxLabelText(Text = "1983 - 1990")]
        From1983To1990,
        [GovUkRadioCheckboxLabelText(Text = "1991 - 1995")]
        From1991To1995,
        [GovUkRadioCheckboxLabelText(Text = "1996 - 2002")]
        From1996To2002,
        [GovUkRadioCheckboxLabelText(Text = "2003 - 2006")]
        From2003To2006,
        [GovUkRadioCheckboxLabelText(Text = "2007 or later")]
        From2007ToPresent
    }
}