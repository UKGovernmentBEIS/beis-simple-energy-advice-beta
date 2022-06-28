using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

// TODO: seabeta-576 When private beta finishes, this section should be removed.
public class PrivateBetaViewModel
{
    [ModelBinder(typeof(GovUkCheckboxBoolBinder))]
    public bool HasAcceptedCookies { get; set; }
}