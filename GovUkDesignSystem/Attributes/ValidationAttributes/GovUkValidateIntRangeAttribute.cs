using System;
using System.Reflection;
using GovUkDesignSystem.Helpers;

namespace GovUkDesignSystem.Attributes.ValidationAttributes
{
    public class GovUkValidateIntRangeAttribute : GovUkValidationAttribute
    {

        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public string OutOfRangeErrorMessage { get; set; }
        
        public override bool CheckForValidationErrors<TProperty>(
            GovUkViewModel model,
            PropertyInfo property,
            TProperty parameterValue)
        {
            
            if (typeof(TProperty) != typeof(int))
            {
                throw new Exception("Paramater value has the wrong type");
            }

            var value = parameterValue as int?;

            if (value.HasValue)
            {
                if (ValueIsOutOfRange(property, value.Value))
                {
                    AddValueIsOutOfRangeErrorMessage(model, property);
                    return false;
                }
            }

            return true;
        }

        private static bool ValueIsOutOfRange(PropertyInfo property, int value)
        {
            var intRangeAttribute = property.GetSingleCustomAttribute<GovUkValidateIntRangeAttribute>();

            bool intRangeInForce = intRangeAttribute != null;

            if (intRangeInForce)
            {
                decimal minimumAllowed = (decimal) intRangeAttribute.Minimum;
                decimal maximumAllowed = (decimal) intRangeAttribute.Maximum;

                bool outOfRange = value < minimumAllowed || value > maximumAllowed;
                return outOfRange;
            }

            return false;
        }

        private static void AddValueIsOutOfRangeErrorMessage(GovUkViewModel model, PropertyInfo property)
        {
            var intRangeAttribute = property.GetSingleCustomAttribute<GovUkValidateIntRangeAttribute>();

            int minimum = (int) intRangeAttribute.Minimum;
            int maximum = (int) intRangeAttribute.Maximum;

            if (!string.IsNullOrWhiteSpace(intRangeAttribute.OutOfRangeErrorMessage))
            {
                model.AddErrorFor(property, intRangeAttribute.OutOfRangeErrorMessage);
            }
            else
            {
                ParserHelpers.AddErrorMessageBasedOnPropertyDisplayName(model, property,
                    name => $"{name} must be between {minimum} and {maximum}",
                    ErrorMessagePropertyNamePosition.StartOfMessage);
            }
        }

    }
}
