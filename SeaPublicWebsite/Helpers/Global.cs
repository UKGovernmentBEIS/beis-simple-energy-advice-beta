using Newtonsoft.Json;

namespace SeaPublicWebsite.Helpers
{
    public static class Global
    {
        public static VcapServices VcapServices =>
            Config.GetAppSetting("VCAP_SERVICES") != null
                ? JsonConvert.DeserializeObject<VcapServices>(Config.GetAppSetting("VCAP_SERVICES"))
                : null;
    }
}
