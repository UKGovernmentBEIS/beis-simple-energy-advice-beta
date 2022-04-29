using System;
using System.ComponentModel.DataAnnotations;

namespace GovUkDesignSystem.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GovUkValidateRequiredIfAttribute: ValidationAttribute
    {
        private readonly string requiredError;
        private readonly string isRequiredPropertyName;

        public GovUkValidateRequiredIfAttribute(string requiredError, string isRequiredPropertyName)
        {
            this.requiredError = requiredError;
            this.isRequiredPropertyName = isRequiredPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var isRequiredPropertyInfo = validationContext.ObjectInstance.GetType().GetProperty(isRequiredPropertyName);
            
            if (isRequiredPropertyInfo is null)
            {
                throw new ArgumentException(
                    "'isRequiredPropertyName' must be a boolean property in the model the attribute is included in");
            }
            
            var isRequired = (bool)isRequiredPropertyInfo.GetValue(validationContext.ObjectInstance, null)!;
            
            if (isRequired && value is null)
            {
                return new ValidationResult(requiredError);
            }
            return ValidationResult.Success;
        }
    }
}