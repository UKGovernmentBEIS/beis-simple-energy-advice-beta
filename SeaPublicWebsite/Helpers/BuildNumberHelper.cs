using System.IO;
using System.Reflection;
using System.Text.Json;

namespace SeaPublicWebsite.Helpers
{
    public static class BuildNumberHelper
    {
        class BuildNumberObject
        {
            public string BuildNumber { get; set; }
        }

        private static BuildNumberObject cachedBuildNumber;
        
        public static string GetBuildNumber()
        {
            if (cachedBuildNumber == null)
            {
                string executablePath = Assembly.GetEntryAssembly().Location;
                string executableDirectory = Path.GetDirectoryName(executablePath);
                string pathToFile = Path.Combine(executableDirectory, "build-number.json");

                using (StreamReader streamReader = new StreamReader(pathToFile))
                {
                    string buildNumberJson = streamReader.ReadToEnd();
                    cachedBuildNumber = JsonSerializer.Deserialize<BuildNumberObject>(buildNumberJson);
                }
            }

            return cachedBuildNumber.BuildNumber;
        }

    }
}
