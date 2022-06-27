using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class PrivateBetaViewModel
{
    [ModelBinder(typeof(GovUkCheckboxBoolBinder))]
    public bool HasAcceptedCookies { get; set; }
}