namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyConfiguration
    {
        public const string Name = "GovUkNotify";
        public ApplicationReferenceNumberConfiguration ApplicationReferenceNumber { get; set; }
        public RequestDocumentConfiguration RequestDocument { get; set; }
    }
    public class ApplicationReferenceNumberConfiguration
    {
        public string Id { get; set; }
        public string Reference { get; set; }
    }

    public class RequestDocumentConfiguration
    {
        public string Id { get; set; }
        public string DocumentContents { get; set; }
    }
}