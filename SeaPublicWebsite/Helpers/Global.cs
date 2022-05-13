using Microsoft.Extensions.Options;

namespace SeaPublicWebsite.Helpers;

public class GlobalConstants
{
    public readonly GlobalConstantsConfiguration Configuration;
    public GlobalConstants(IOptions<GlobalConstantsConfiguration> options)
    {
        Configuration = options.Value;
    }
}

public class GlobalConstantsConfiguration
{
    public const string ConfigSection = "GlobalConstants";
    public string ServiceName { get; set; }
}