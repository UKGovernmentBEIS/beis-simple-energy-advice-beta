﻿namespace SeaPublicWebsite.ExternalServices.EmailSending
{
    public interface IEmailSender
    {
        public void SendReferenceNumberEmail(string emailAddress, string reference);
        public void SendRequestedDocumentEmail(string emailAddress, byte[] documentContents);
        public void SendFeedbackFormResponseEmail(string whatUserWasDoing, string whatUserToldUs);
    }
}