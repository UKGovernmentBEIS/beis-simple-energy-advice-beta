using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions
{
    public enum UserGoal
    {
        [GovUkRadioCheckboxLabelText(Text = "Reduce my energy bills")]
        ReduceBills,
        [GovUkRadioCheckboxLabelText(Text = "Reduce my greenhouse emissions")]
        GreenerHome
    }
}