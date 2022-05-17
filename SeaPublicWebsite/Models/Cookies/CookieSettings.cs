using GovUkDesignSystem.GovUkDesignSystemComponents;

namespace SeaPublicWebsite.Models.Cookies;

public class CookieSettings
{
    public int Version { get; set; }
    public BannerState BannerState { get; set; }
    public bool GoogleAnalytics { get; set; }
}