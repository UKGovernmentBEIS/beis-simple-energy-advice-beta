using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace SeaPublicWebsite.Helpers
{
    public static class Config
    {
        private static string environmentName;
        private static IConfiguration configuration;

        public static string EnvironmentName
        {
            get
            {
                if (environmentName == null)
                {
                    environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    if (string.IsNullOrWhiteSpace(environmentName))
                    {
                        environmentName = "Local";
                    }
                }

                return environmentName;
            }
            set => environmentName = value;
        }
        
        public static IConfiguration Build(IConfigurationBuilder builder = null)
        {
            // NOTE: The order in which these commands run determines which settings take priority

            // First, load from appsettings.json
            builder.AddJsonFile("appsettings.json", false, true);

            // Then, override this with the environment-specific settings
            builder.AddJsonFile($"appsettings.{EnvironmentName}.json", true, true);

            // If we're running the code locally, override this with appsettings.secret.json
            if (Debugger.IsAttached || IsLocal())
            {
                builder.AddJsonFile("appsettings.secret.json", true, true);
            }

            // Then add the unit test configuration (only used when running automated tests)
            builder.AddJsonFile("appsettings.unittests.json", true, false);

            // Environment variables are added last (so have highest priority)
            builder.AddEnvironmentVariables();

            configuration = builder.Build();
            return configuration;
        }

        public static bool IsLocal()
        {
            return IsEnvironment("Local");
        }

        public static bool IsProduction()
        {
            return IsEnvironment("PROD", "PRODUCTION");
        }

        private static bool IsEnvironment(params string[] environmentNames)
        {
            foreach (string name in environmentNames)
            {
                if (EnvironmentName.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetAppSetting(string key, string defaultValue = null)
        {
            IConfiguration appSettings = GetAppSettings();
            string value = appSettings[key];

            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        private static IConfiguration GetAppSettings()
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
