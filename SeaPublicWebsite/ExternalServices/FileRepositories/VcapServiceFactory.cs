﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SeaPublicWebsite.ExternalServices.FileRepositories
{
    // TODO: SEABETA-193 Remove this file
    public class VcapServiceFactory
    {
        public static VcapServices GetVcapServices(IConfiguration configuration)
        {
            var setting = configuration.GetValue<string>("TEMP_AWS_CONFIG");
            return !string.IsNullOrWhiteSpace(setting)
                ? JsonConvert.DeserializeObject<VcapServices>(setting)
                : null;
        }
    }
}