using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace GovUkDesignSystem.Parsers
{
    public class NullableIntParser
    {

        public static void ParseAndValidate(
            GovUkViewModel model,
            PropertyInfo property,
            HttpRequest httpRequest)
        {
            string parameterName = $"GovUk_Text_{property.Name}";

            StringValues parameterValues = HttpRequestParameterHelper.GetRequestParameter(httpRequest, parameterName);

            ParserHelpers.ThrowIfMoreThanOneValue(parameterValues, property);
            ParserHelpers.SaveUnparsedValueFromRequestToModel(model, httpRequest, parameterName);

            if (ParserHelpers.IsValueRequiredAndMissingOrEmpty(property, parameterValues))
            {
                ParserHelpers.AddRequiredAndMissingErrorMessage(model, property);
                return;
            }

            if (parameterValues.Count > 0)
            {
                string parameterValue = parameterValues[0];

                if (!double.TryParse(parameterValue, out _))
                {
                    ParserHelpers.AddErrorMessageBasedOnPropertyDisplayName(model, property,
                        (name) => $"{name} must be a number",
                        ErrorMessagePropertyNamePosition.StartOfMessage);
                    return;
                }

                int parsedIntValue;
                if (!int.TryParse(parameterValue, out parsedIntValue))
                {
                    ParserHelpers.AddErrorMessageBasedOnPropertyDisplayName(model, property,
                        (name) => $"{name} must be a whole number",
                        ErrorMessagePropertyNamePosition.StartOfMessage);
                    return;
                }


                List<GovUkValidationAttribute> customAttributes = property
                    .GetCustomAttributes(typeof(GovUkValidationAttribute))
                    .Cast<GovUkValidationAttribute>()
                    .ToList();
                foreach (var attribute in customAttributes)
                {
                    var result = attribute.CheckForValidationErrors(model, property, parsedIntValue);
                    if (result == false)
                    {
                        return;
                    }
                }

                property.SetValue(model, parsedIntValue);
            }

            model.ValueWasSuccessfullyParsed(property);
        }

    }
}
