using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices
{

    public interface IGovNotifyApi
    {
        public EmailNotificationResponse SendEmail(GovUkNotifyEmailModel emailModel);
    }
    
    public class GovUkNotifyApi: IGovNotifyApi
    {
        private static readonly string ApiKey = Global.GovUkNotifyApiKey;
        private static readonly NotificationClient Client = new NotificationClient(ApiKey);

        public EmailNotificationResponse SendEmail(GovUkNotifyEmailModel emailModel)
        {
            try
            {
                var response = Client.SendEmail(
                    emailModel.EmailAddress,
                    emailModel.TemplateId,
                    emailModel.Personalisation);
                return response;
            }
            catch (NotifyClientException e)
            {
                throw;
            }
        }
    }
}