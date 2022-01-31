namespace SeaPublicWebsite.Helpers
{
    public static class Global
    {
        public static string BasicAuthUsername => Config.GetAppSetting("BasicAuthUsername");
        public static string BasicAuthPassword => Config.GetAppSetting("BasicAuthPassword");
    }
}