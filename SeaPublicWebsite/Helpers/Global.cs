using Newtonsoft.Json;

namespace SeaPublicWebsite.Helpers
{
    public static class Global
    {
        public static VcapServices VcapServices =>
            Config.GetAppSetting("VCAP_SERVICES") != null
                ? JsonConvert.DeserializeObject<VcapServices>(Config.GetAppSetting("VCAP_SERVICES"))
                : null;

        public static string BasicAuthUsername => Config.GetAppSetting("BasicAuthUsername");
        public static string BasicAuthPassword => Config.GetAppSetting("BasicAuthPassword");
        public static string EpcAuthUsername => Config.GetAppSetting("EpcAuthUsername");
        public static string EpcAuthPassword => Config.GetAppSetting("EpcAuthPassword");
        public static string OpenEpcBaseAddress => Config.GetAppSetting("OpenEpcBaseAddress");
        public static string EpbEpcBaseAddress => Config.GetAppSetting("EpbEpcBaseAddress");
        public static string BreUsername => Config.GetAppSetting("BreUsername");
        public static string BrePassword => Config.GetAppSetting("BrePassword");
    }
}