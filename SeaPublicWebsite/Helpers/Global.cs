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
        public static string GovUkNotifyApiKey => Config.GetAppSetting("GovUkNotifyApiKey");
        public static string GovUkNotifyApiTestKey => Config.GetAppSetting("GovUkNotifyApiTestKey");

        public static string GetIdForTemplate(string template)
        {
            return Config.GetAppSetting($"GovUkNotifyTemplates:{template}:id");
        }

        public static string GetFieldForTemplate(string template, string field)
        {
            return Config.GetAppSetting($"GovUkNotifyTemplates:{template}:fields:{field}");
        }
    }
}