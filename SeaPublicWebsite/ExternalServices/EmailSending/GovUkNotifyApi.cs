using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyApi: IEmailSender
    {
        private readonly NotificationClient client;
        private readonly GovUkNotifyConfiguration govUkNotifyConfig;

        public GovUkNotifyApi(IOptions<GovUkNotifyConfiguration> config)
        {
            govUkNotifyConfig = config.Value;
            client = new NotificationClient(govUkNotifyConfig.ApiKey);
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
            catch (NotifyClientException)
            {
                // TODO: Logging, SEABETA-192
                throw;
            }
        }

        public void SendReferenceNumberEmail(string emailAddress, string reference)
        {
            var template = govUkNotifyConfig.ApplicationReferenceNumberTemplate;
            var personalisation = new Dictionary<string, dynamic>
            {
                { template.ReferencePlaceholder, reference }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = template.Id,
                Personalisation = personalisation
            };
            var response = SendEmail(emailModel);
        }

        public void SendRequestedDocumentEmail(string emailAddress, byte[] documentContents)
        {
            var template = govUkNotifyConfig.RequestDocumentTemplate;
            var personalisation = new Dictionary<string, dynamic>
            {
                { template.DocumentContentsPlaceholder, NotificationClient.PrepareUpload(documentContents) }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = emailAddress,
                TemplateId = template.Id,
                Personalisation = personalisation
            };
            var response = SendEmail(emailModel);
        }

        public void SendFeedbackFormResponseEmail(string whatUserWasDoing, string whatUserToldUs)
        {
            var template = govUkNotifyConfig.FeedbackFormResponseTemplate;
            var personalisation = new Dictionary<string, dynamic>
            {
                { template.WhatUserWasDoingPlaceholder, whatUserWasDoing },
                { template.WhatUserToldUsPlaceholder, whatUserToldUs }
            };
            var emailModel = new GovUkNotifyEmailModel
            {
                EmailAddress = govUkNotifyConfig.FeedbackCollectingEmailAddress,
                TemplateId = template.Id,
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