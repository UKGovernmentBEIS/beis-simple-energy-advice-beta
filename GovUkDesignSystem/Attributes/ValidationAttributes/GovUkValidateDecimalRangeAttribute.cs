using System;
using System.ComponentModel.DataAnnotations;

namespace GovUkDesignSystem.Attributes.ValidationAttributes;

public class GovUkValidateDecimalRangeAttribute : RangeAttribute
{
    public GovUkValidateDecimalRangeAttribute(string propertyName, decimal minimum, decimal maximum) : base(Decimal.ToDouble(minimum), Decimal.ToDouble(maximum))
    {
        ErrorMessage = $"{propertyName} must be between {minimum} and {maximum}";
    }
}