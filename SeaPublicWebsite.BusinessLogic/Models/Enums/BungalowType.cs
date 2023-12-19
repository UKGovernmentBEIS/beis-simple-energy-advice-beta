using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Security.AccessControl;
using SeaPublicWebsite.BusinessLogic.Resources;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum BungalowType
    {
        /* TODO Take the functionality from display's class which allows strings to be localised. (The localise string and ResourceType thing)
         * TODO Port the functionality into GovUkRadioCheckboxLabelText & the Databinder Errors
         * https://github.com/cabinetoffice/govuk-design-system-dotnet/blob/master/GovUkDesignSystem/Attributes/DataBinding/GovUkDataBindingMandatoryDecimalErrorTextAttribute.cs
         * https://github.com/cabinetoffice/govuk-design-system-dotnet/blob/4c183fb6eb8f79a3b6f09909286d76326d499cc0/GovUkDesignSystem/Attributes/GovUkRadioCheckboxLabelTextAttribute.cs
         * TODO Ask someone how to test the library works how I want it to before I change it for everyone (if poss)
         */
        [Display(ResourceType = typeof(EnumDisplay), Name = nameof(EnumDisplay.Detached))]
        [GovUkRadioCheckboxLabelText(Text = "Detached")]
        Detached,
        [Display(ResourceType = typeof(EnumDisplay), Name = nameof(EnumDisplay.SemiDetached))]
        [GovUkRadioCheckboxLabelText(Text = "Semi-detached")]
        SemiDetached,
        [GovUkRadioCheckboxLabelText(Text = "Terraced")]
        Terraced,
        [GovUkRadioCheckboxLabelText(Text = "End terrace")]
        EndTerrace
    }
}