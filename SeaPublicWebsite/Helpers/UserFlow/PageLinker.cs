using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    {
        public string NewOrReturningUserBackLink();

        public string OwnershipStatusBackLink(string reference, bool change);
    }

    public class PageLinker: IPageLinker
    {
        private readonly LinkGenerator linkGenerator;

        public PageLinker(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }
        
        public string NewOrReturningUserBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }

        public string OwnershipStatusBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference });
        }

        public string CountryBackLink()
        {
            throw new Exception();
        }

        public string ServiceUnsuitableBackLink()
        {
            throw new Exception();
            
        }

        public string AskForPostcodeBackLink()
        {
            throw new Exception();
        }

        public string ConfirmAddressBackLink()
        {
            throw new Exception();
        }

        public string PropertyTypeBackLink()
        {
            throw new Exception();
        }
        
        public string HouseTypeBackLink()
        {
            throw new Exception();
        }
        
        public string BungalowTypeBackLink()
        {
            throw new Exception();
        }
        
        public string FlatTypeBackLink()
        {
            throw new Exception();
        }
        
        public string HomeAgeBackLink()
        {
            throw new Exception();
        }
        
        public string WallConstructionBackLink()
        {
            throw new Exception();
        }
        
        public string CavityWallsInsulatedBackLink()
        {
            throw new Exception();
        }
        
        public string SolidWallsInsulatedBackLink()
        {
            throw new Exception();
        }
        
        public string FloorConstructionBackLink()
        {
            throw new Exception();
        }
        
        public string FloorInsulatedBackLink()
        {
            throw new Exception();
        }
        
        public string RoofConstructionBackLink()
        {
            throw new Exception();
        }
        
        public string AccessibleLoftSpaceBackLink()
        {
            throw new Exception();
        }
        
        public string RoofInsulatedBackLink()
        {
            throw new Exception();
        }
        
        public string OutdoorSpaceBackLink()
        {
            throw new Exception();
        }
        
        public string GlazingTypeBackLink()
        {
            throw new Exception();
        }
        
        public string HeatingTypeBackLink()
        {
            throw new Exception();
        }
        
        public string OtherHeatingTypeBackLink()
        {
            throw new Exception();
        }
        
        public string HotWaterCylinderBackLink()
        {
            throw new Exception();
        }
        
        public string NumberOfOccupantsBackLink()
        {
            throw new Exception();
        }
        
        public string HeatingPatternBackLink()
        {
            throw new Exception();
        }
        
        public string TemperatureBackLink()
        {
            throw new Exception();
        }
        
        // Ghost page
        public string EmailAddressBackLink()
        {
            throw new Exception();
        }
        
        public string AnswerSummaryBackLink()
        {
            throw new Exception();
        }
        
        public string YourRecommendationsBackLink()
        {
            throw new Exception();
        }
        
        public string BackLink()
        {
            throw new Exception();
        }
        
        // ^ Copy paste template ^
    }
}