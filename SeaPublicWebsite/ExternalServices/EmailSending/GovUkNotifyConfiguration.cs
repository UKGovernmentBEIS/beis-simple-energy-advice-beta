namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyConfiguration
    {
        public const string ConfigSection = "GovUkNotify";
        
        public string ApiKey { get; set; }
        public string FeedbackCollectingEmailAddress { get; set; }
        public ApplicationReferenceNumberConfiguration ApplicationReferenceNumberTemplate { get; set; }
        public RequestDocumentConfiguration RequestDocumentTemplate { get; set; }
        public FeedbackFormResponseConfiguration FeedbackFormResponseTemplate { get; set; }
        public FeedbackSurveyResponseConfiguration FeedbackSurveyResponseTemplate { get; set; }
    }
    
    public class ApplicationReferenceNumberConfiguration
    {
        public string Id { get; set; }
        public string ReferencePlaceholder { get; set; }
    }

    public class RequestDocumentConfiguration
    {
        public string Id { get; set; }
        public string DocumentContentsPlaceholder { get; set; }
    }

    public class FeedbackFormResponseConfiguration
    {
        public string Id { get; set; }
        public string WhatUserWasDoingPlaceholder { get; set; }
        public string WhatUserToldUsPlaceholder { get; set; }
    }

    public class FeedbackSurveyResponseConfiguration
    {
        public string Id { get; set; }
        public string VisitReasonPlaceholder { get; set; }
        public string FoundInformationPlaceholder { get; set; }
        public string HowInformationHelpedPlaceholder { get; set; }
        public string WhatPlannedToDoPlaceholder { get; set; }
    }
}