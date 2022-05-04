namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public interface IEmailSender
    {
        public void SendReferenceNumberEmail(string emailAddress, string reference);
    }
}