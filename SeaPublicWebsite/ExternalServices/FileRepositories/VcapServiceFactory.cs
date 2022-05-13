using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public class VcapServiceFactory
    {
        public static VcapServices GetVcapServices(IConfiguration configuration)
        {
            return GetAppSetting(configuration, "VCAP_SERVICES") != null
                ? JsonConvert.DeserializeObject<VcapServices>(GetAppSetting(configuration, "VCAP_SERVICES"))
                : null;
        }
        
        private static string GetAppSetting(IConfiguration configuration, string key)
        {
            IConfiguration appSettings = GetAppSettings(configuration);
            string value = appSettings[key];
        
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private static IConfiguration GetAppSettings(IConfiguration configuration)
        {
            IConfiguration appSettings = configuration.GetSection("AppSettings");
            if (!appSettings.GetChildren().Any())
            {
                appSettings = configuration;
            }
        
            return appSettings;
        }
    }
}