using Microsoft.Extensions.Options;

namespace SeaPublicWebsite.Services
{
    public class FullHostnameService
    {
        public readonly FullHostnameConfiguration Configuration;
    
        public FullHostnameService(IOptions<FullHostnameConfiguration> options)
        {
            Configuration = options.Value;
        }

        public string GetHostname()
        {
            return Configuration.BaseUrl;
        }
    }
}
