using System;
using System.ComponentModel.DataAnnotations;

namespace GovUkDesignSystem.Attributes.ValidationAttributes;

public class GovUkValidateDecimalRangeAttribute : RangeAttribute
{
    // Attributes do not allow decimals as parameters, but allow for their string representation
    public GovUkValidateDecimalRangeAttribute(string propertyName, string minimum, string maximum) : base(typeof(decimal), minimum, maximum)
    {
        ErrorMessage = $"{propertyName} must be between {minimum} and {maximum}";
    }
}