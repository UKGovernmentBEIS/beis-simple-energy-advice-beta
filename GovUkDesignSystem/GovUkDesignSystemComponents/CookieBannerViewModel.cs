using GovUkDesignSystem.GovUkDesignSystemComponents.Enums;

namespace GovUkDesignSystem.GovUkDesignSystemComponents;

public class CookieBannerViewModel
{
    public string NameOfService { get; set; }
    public BannerState BannerState { get; set; } 
    public CookieType CookieType { get; set; }
    public string ButtonClickAction { get; set; }
    public string ViewCookiesLink { get; set; }
    public string ReturnUrl { get; set; }
}

public enum BannerState
{
    ShowBanner,
    ShowAccepted,
    ShowRejected,
    Hide
}