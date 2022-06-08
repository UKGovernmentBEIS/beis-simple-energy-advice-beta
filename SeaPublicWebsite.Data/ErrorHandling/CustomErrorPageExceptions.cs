namespace SeaPublicWebsite.Data.ErrorHandling
{

    public abstract class CustomErrorPageException : Exception
    {
        public abstract string ViewName { get; }
        public abstract int StatusCode { get; }
    }

    public class PropertyReferenceNotFoundException : CustomErrorPageException
    {
        public override string ViewName => "../Errors/PropertyReferenceNotFound";
        public override int StatusCode => 404;
        public string Reference { get; set; }
    }

}