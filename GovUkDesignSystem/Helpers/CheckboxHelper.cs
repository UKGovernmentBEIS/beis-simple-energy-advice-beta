using System;
using GovUkDesignSystem.Attributes;

namespace GovUkDesignSystem.Helpers
{
    public static class CheckboxHelper
    {
        public static string GetCheckboxLabelText<TEnum>(
            this TEnum enumValue)
            where TEnum : struct, IConvertible
        {
            string textFromAttribute = GovUkRadioCheckboxLabelTextAttribute.GetValueForEnum(typeof(TEnum), enumValue);

            string checkboxLabel = textFromAttribute ?? enumValue.ToString();

            return checkboxLabel;
        }
        
    }
}