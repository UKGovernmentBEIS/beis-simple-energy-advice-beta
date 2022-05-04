using System.Collections.Generic;
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
        private readonly string apiKey = Global.GovUkNotifyApiKey;
        private readonly NotificationClient client;

        public GovUkNotifyApi()
        {
            client = new NotificationClient(apiKey);
        }

        public EmailNotificationResponse SendEmail(GovUkNotifyEmailModel emailModel)
        {
            try
            {
                var response = client.SendEmail(
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

        public EmailNotificationResponse SendReferenceNumberEmail(string emailAddress, string reference)
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                { "reference", reference }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = EmailTemplates.ApplicationReferenceNumberTemplateId,
                Personalisation = personalisation
            };
            return SendEmail(emailModel);
        }
    }

    public static class EmailTemplates
    {
        public const string ApplicationReferenceNumberTemplateId = "28470b42-26ff-4888-8221-c65e27a8c832";
    }
}