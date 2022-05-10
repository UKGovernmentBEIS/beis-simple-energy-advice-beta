namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyConfiguration
    {
        public const string Name = "GovUkNotify";
        public ApplicationReferenceNumberConfiguration ApplicationReferenceNumberTemplate { get; set; }
        public RequestDocumentConfiguration RequestDocumentTemplate { get; set; }
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
}