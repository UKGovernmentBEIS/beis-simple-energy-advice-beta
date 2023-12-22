using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
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
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.Detached))]
        Detached,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.SemiDetached))]
        SemiDetached,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.Terraced))]
        Terraced,
        [Display(ResourceType = typeof(Resources.Enum.BungalowType), Description = nameof(Resources.Enum.BungalowType.EndTerrace))]
        EndTerrace
    }
}