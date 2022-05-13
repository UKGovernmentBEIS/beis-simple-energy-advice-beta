using GovUkDesignSystem.GovUkDesignSystemComponents.Enums;

namespace GovUkDesignSystem.GovUkDesignSystemComponents;

public class CookieBannerViewModel
{
    public string Action { get; set; }
    public string NameOfService { get; set; }
    public CookieType CookieType { get; set; }
    public string ReturnUrl { get; set; }
}