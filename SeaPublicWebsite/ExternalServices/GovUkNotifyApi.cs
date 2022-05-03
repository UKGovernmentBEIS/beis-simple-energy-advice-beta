using Notify.Client;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices
{

    public interface IGovNotifyAPI
    {
        
        // TODO
    }
    
    public class GovUkNotifyApi: IGovNotifyAPI
    {
        private static string ApiKey = Global.GovUkNotifyApiKey;
        private static NotificationClient Client = new NotificationClient(ApiKey);
        
        // TODO
    }
}