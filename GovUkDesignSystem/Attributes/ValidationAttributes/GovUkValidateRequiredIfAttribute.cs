using System;
using System.ComponentModel.DataAnnotations;

namespace GovUkDesignSystem.Attributes.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GovUkValidateRequiredIfAttribute: ValidationAttribute
    {
        private readonly string requiredError;
        private readonly bool isRequired;

        public GovUkValidateRequiredIfAttribute(string requiredError, bool isRequired)
        {
            this.requiredError = requiredError;
            this.isRequired = isRequired;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (isRequired && value is null)
            {
                return new ValidationResult(requiredError);
            }
            return ValidationResult.Success;
        }
    }
}