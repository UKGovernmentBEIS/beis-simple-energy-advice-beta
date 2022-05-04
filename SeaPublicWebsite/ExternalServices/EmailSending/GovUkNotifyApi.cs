using System;
using System.Collections.Generic;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using SeaPublicWebsite.ExternalServices.Models;
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
                throw;
            }
        }

        public void SendReferenceNumberEmail(string emailAddress, string reference)
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
            var response = SendEmail(emailModel);
        }

        public void SendRequestedDocumentEmail(string emailAddress, byte[] documentContents)
        {
            var personalisation = new Dictionary<string, dynamic>
            {
                { "link_to_file", NotificationClient.PrepareUpload(documentContents) }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = EmailTemplates.RequestDocumentTemplateId,
                Personalisation = personalisation
            };
            var response = SendEmail(emailModel);
        }
    }

    public static class EmailTemplates
    {
        public const string ApplicationReferenceNumberTemplateId = "28470b42-26ff-4888-8221-c65e27a8c832";
        public const string RequestDocumentTemplateId = "91ea7d56-aca9-4f79-8ba9-99dfb54c464d";
    }
}