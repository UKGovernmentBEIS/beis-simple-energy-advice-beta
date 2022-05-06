namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    {
        public string GetBackLink();
        public string GetForwardLink();
    }

    public class PageLinker: IPageLinker
    {
        public string GetBackLink()
        {
            throw new System.NotImplementedException();
        }

        public string GetForwardLink()
        {
            throw new System.NotImplementedException();
        }
    }
}