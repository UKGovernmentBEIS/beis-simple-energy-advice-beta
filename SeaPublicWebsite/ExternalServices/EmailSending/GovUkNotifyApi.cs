using System.Collections.Generic;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyApi: IEmailSender
    {
        private readonly string apiKey = Global.GovUkNotifyApiKey;
        private readonly NotificationClient client;

        public GovUkNotifyApi()
        {
            client = new NotificationClient(apiKey);
        }

        private EmailNotificationResponse SendEmail(GovUkNotifyEmailModel emailModel)
        {
            try
            {
                var response = client.SendEmail(
                    emailModel.EmailAddress,
                    emailModel.TemplateId,
                    emailModel.Personalisation,
                    emailModel.Reference,
                    emailModel.EmailReplyToId);
                return response;
            }
            catch (NotifyClientException e)
            {
                // TODO: Logging, SEABETA-192
                throw;
            }
        }

        public void SendReferenceNumberEmail(string emailAddress, string reference)
        {
            var template = "ApplicationReferenceNumberTemplate";
            var personalisation = new Dictionary<string, dynamic>
            {
                { Global.GetFieldForTemplate(template, "reference"), reference }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = Global.GetIdForTemplate(template),
                Personalisation = personalisation
            };
            var response = SendEmail(emailModel);
        }

        public void SendRequestedDocumentEmail(string emailAddress, byte[] documentContents)
        {
            var template = "RequestDocumentTemplate";
            var personalisation = new Dictionary<string, dynamic>
            {
                { Global.GetFieldForTemplate(template, "documentContents"), NotificationClient.PrepareUpload(documentContents) }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = Global.GetIdForTemplate(template),
                Personalisation = personalisation
            };
            var response = SendEmail(emailModel);
        }
    }

    internal class GovUkNotifyEmailModel
    {
        public string EmailAddress { get; set; }
        public string TemplateId { get; set; }
        public Dictionary<string, dynamic> Personalisation { get; set; }
        public string Reference { get; set; }
        public string EmailReplyToId { get; set; }
    }
}