namespace SeaPublicWebsite.ExternalServices.OpenEpc
{
    public class OpenEpcConfiguration
    {
        public const string ConfigSection = "OpenEpc";
        
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}