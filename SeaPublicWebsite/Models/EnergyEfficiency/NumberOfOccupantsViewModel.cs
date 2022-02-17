using GovUkDesignSystem;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class NumberOfOccupantsViewModel : GovUkViewModel
    {
        [GovUkDisplayNameForErrors(NameAtStartOfSentence = "Number of occupants", NameWithinSentence = "number of occupants")]
        [GovUkValidateRequired(ErrorMessageIfMissing = "Enter the number of people who live in the property")]
        [GovUkValidateIntRange(Minimum = 1, Maximum = 9,
            OutOfRangeErrorMessage = "Enter a number between 1 and 9. If there are more than 9 people living at the property, enter 9.")]
        public int? NumberOfOccupants { get; set; }

        public string Reference { get; set; }
        public bool Change { get; set; }
    }
}