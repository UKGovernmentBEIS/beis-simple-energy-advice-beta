using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Exceptions;
using Notify.Models.Responses;
using SeaPublicWebsite.Models.Feedback;

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
            catch (NotifyClientException e)
            {
                if (e.Message.Contains("Not a valid email address"))
                {
                    throw new EmailSenderException(EmailSenderExceptionType.InvalidEmailAddress);
                }

                throw new EmailSenderException(EmailSenderExceptionType.Other);
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
        
        public void SendFeedbackSurveyResponseEmail(FeedbackSurveyViewModel feedback)
        {
            var template = govUkNotifyConfig.FeedbackSurveyResponseTemplate;
            var visitReason = string.Join('\n', feedback.VisitReasonList.Select(
                r => r is VisitReason.Other 
                    ? feedback.OtherReason 
                    : GovUkRadioCheckboxLabelTextAttribute.GetLabelText(r)));
            var foundInformation = feedback.FoundInformation is FoundInformation.Yes
                ? "Yes"
                : "No. " + feedback.NotFoundInformationDetails;
            var howInformationHelped = string.Join('\n', feedback.HowInformationHelpedList.Select(
                r => r is HowInformationHelped.Other 
                    ? feedback.OtherHelp 
                    : GovUkRadioCheckboxLabelTextAttribute.GetLabelText(r)));
            var whatPlannedToDo = string.Join('\n', feedback.WhatPlannedToDoList.Select(
                r => r is WhatPlannedToDo.Other
                    ? feedback.OtherPlan 
                    : GovUkRadioCheckboxLabelTextAttribute.GetLabelText(r)));
            var personalisation = new Dictionary<string, dynamic>
            {
                { template.VisitReasonPlaceholder, visitReason },
                { template.FoundInformationPlaceholder, foundInformation},
                { template.HowInformationHelpedPlaceholder, howInformationHelped },
                { template.WhatPlannedToDoPlaceholder, whatPlannedToDo },
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