namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public class GovUkNotifyConfiguration
    {
        public const string Name = "GovUkNotify";
        public string Test { get; set; }
        public ApplicationReferenceNumberConfiguration ApplicationReferenceNumber { get; set; }
        public RequestDocumentConfiguration RequestDocument { get; set; }
    }
}