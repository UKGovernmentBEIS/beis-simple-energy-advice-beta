﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    {
        public string NewOrReturningUserBackLink();

        public string OwnershipStatusBackLink(string reference, bool change);


        public string CountryBackLink(string reference, bool change);

        public string ServiceUnsuitableBackLink(string reference, string from);

        public string AskForPostcodeBackLink(string reference);

        public string ConfirmAddressBackLink(string reference);

        public string PropertyTypeBackLink(string reference, bool change);
        
        public string HouseTypeBackLink(string reference, bool change);
        
        public string BungalowTypeBackLink(string reference, bool change);
        
        public string FlatTypeBackLink(string reference, bool change);
        
        public string HomeAgeBackLink(string reference, PropertyType? propertyType, bool change);
        
        public string WallConstructionBackLink(string reference, bool change);
        
        public string CavityWallsInsulatedBackLink(string reference, bool change);
        
        public string SolidWallsInsulatedBackLink(string reference, WallConstruction? wallConstruction, bool change);
        
        public string FloorConstructionBackLink(string reference, WallConstruction? wallConstruction, bool change);
        
        public string FloorInsulatedBackLink(string reference, bool change);
        
        public string RoofConstructionBackLink(string reference, PropertyType? propertyType, FlatType? flatType, bool change);
        
        public string AccessibleLoftSpaceBackLink(string reference, bool change);
        
        public string RoofInsulatedBackLink(string reference, bool change);
        
        public string OutdoorSpaceBackLink(string reference, bool change);
        
        public string GlazingTypeBackLink(string reference, bool change);
        
        public string HeatingTypeBackLink(string reference, bool change);
        
        public string OtherHeatingTypeBackLink(string reference, bool change);
        
        public string HotWaterCylinderBackLink(string reference, bool change);
        
        public string NumberOfOccupantsBackLink(string reference, bool change);
        
        public string HeatingPatternBackLink(string reference, bool change);
        
        public string TemperatureBackLink(string reference, bool change);
        
        // Ghost page
        public string EmailAddressBackLink(string reference, bool change);

        public string AnswerSummaryBackLink(string reference);
        
        public string YourRecommendationsBackLink(string reference);
        
        public string BackLink();
        
        // ^ Copy paste template ^
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

        public string CountryBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        public string ServiceUnsuitableBackLink(string reference, string from)
        {
            return linkGenerator.GetPathByAction($"{from}_Get", "EnergyEfficiency", new { reference });
        }

        public string AskForPostcodeBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        public string ConfirmAddressBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        public string PropertyTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        public string HouseTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        public string BungalowTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        public string FlatTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        public string HomeAgeBackLink(string reference, PropertyType? propertyType, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : propertyType switch
                {
                    PropertyType.House => 
                        linkGenerator.GetPathByAction("HouseType_Get", $"EnergyEfficiency, new {reference}"),
                    PropertyType.Bungalow => 
                        linkGenerator.GetPathByAction("BungalowType_Get", "EnergyEfficiency", new { reference }),
                    PropertyType.ApartmentFlatOrMaisonette => 
                        linkGenerator.GetPathByAction("FlatType_Get", "EnergyEfficiency", new { reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        public string WallConstructionBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency", new { reference });
        }

        public string CavityWallsInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference });
        }

        public string SolidWallsInsulatedBackLink(string reference, WallConstruction? wallConstruction, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : wallConstruction switch
                {
                    WallConstruction.Cavity or WallConstruction.Mixed => 
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new {reference }),
                    WallConstruction.Solid or WallConstruction.DoNotKnow or WallConstruction.Other => 
                        linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new {reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        public string FloorConstructionBackLink(string reference, WallConstruction? wallConstruction, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : wallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference }),
                    WallConstruction.Cavity =>
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                            new { reference }),
                    WallConstruction.DoNotKnow or WallConstruction.Other
                        =>
                        linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        public string FloorInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference });
        }

        public string RoofConstructionBackLink(string reference, PropertyType? propertyType, FlatType? flatType, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : (propertyType, flatType) switch
                {
                    (PropertyType.House, _) or (PropertyType.Bungalow, _)
                        or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor)
                        => linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference }),
                    _ => linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                        new { reference })
                };
        }

        public string AccessibleLoftSpaceBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference  })
                : linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference });
        }

        public string RoofInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference });
        }

        public string OutdoorSpaceBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        public string GlazingTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference });
        }

        public string HeatingTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency", new { reference });
        }

        public string OtherHeatingTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        public string HotWaterCylinderBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        public string NumberOfOccupantsBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HotWaterCylinder_Get", "EnergyEfficiency", new { reference });
        }

        public string HeatingPatternBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference });
        }

        public string TemperatureBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference });
        }

        public string EmailAddressBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        public string AnswerSummaryBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        public string YourRecommendationsBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
        }

        public string AskForPostcodeBackLink()
        {
            throw new NotImplementedException();
        }

        public string ConfirmAddressBackLink()
        {
            throw new NotImplementedException();
        }

        public string PropertyTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string HouseTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string BungalowTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string FlatTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string HomeAgeBackLink()
        {
            throw new NotImplementedException();
        }

        public string WallConstructionBackLink()
        {
            throw new NotImplementedException();
        }

        public string CavityWallsInsulatedBackLink()
        {
            throw new NotImplementedException();
        }

        public string SolidWallsInsulatedBackLink()
        {
            throw new NotImplementedException();
        }

        public string FloorConstructionBackLink()
        {
            throw new NotImplementedException();
        }

        public string FloorInsulatedBackLink()
        {
            throw new NotImplementedException();
        }

        public string RoofConstructionBackLink()
        {
            throw new NotImplementedException();
        }

        public string AccessibleLoftSpaceBackLink()
        {
            throw new NotImplementedException();
        }

        public string RoofInsulatedBackLink()
        {
            throw new NotImplementedException();
        }

        public string OutdoorSpaceBackLink()
        {
            throw new NotImplementedException();
        }

        public string GlazingTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string HeatingTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string OtherHeatingTypeBackLink()
        {
            throw new NotImplementedException();
        }

        public string HotWaterCylinderBackLink()
        {
            throw new NotImplementedException();
        }

        public string NumberOfOccupantsBackLink()
        {
            throw new NotImplementedException();
        }

        public string HeatingPatternBackLink()
        {
            throw new NotImplementedException();
        }

        public string TemperatureBackLink()
        {
            throw new NotImplementedException();
        }

        public string EmailAddressBackLink()
        {
            throw new NotImplementedException();
        }

        public string AnswerSummaryBackLink()
        {
            throw new NotImplementedException();
        }

        public string YourRecommendationsBackLink()
        {
            throw new NotImplementedException();
        }

        public string BackLink()
        {
            throw new NotImplementedException();
        }
    }
}