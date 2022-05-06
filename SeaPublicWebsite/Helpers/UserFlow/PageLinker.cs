using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    {
        public string GetBackLink();
    }

    public class PageLinker: IPageLinker
    {
        private readonly LinkGenerator linkGenerator;

        public PageLinker(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }
        
        public string GetBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }
    }
}