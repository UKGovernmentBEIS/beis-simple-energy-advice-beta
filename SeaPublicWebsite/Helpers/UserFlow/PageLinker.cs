using System;
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
        
        public string RoofConstructionBackLink(string reference, PropertyType? propertyType, FlatType? flatType, FloorConstruction? foFloorConstruction, bool change);
        
        public string AccessibleLoftSpaceBackLink(string reference, bool change);
        
        public string RoofInsulatedBackLink(string reference, bool change);
        
        public string OutdoorSpaceBackLink(string reference, bool change);
        
        public string GlazingTypeBackLink(string reference, RoofConstruction? roofConstruction, AccessibleLoftSpace? accessibleLoftSpace, bool change);
        
        public string HeatingTypeBackLink(string reference, bool change);
        
        public string OtherHeatingTypeBackLink(string reference, bool change);
        
        public string HotWaterCylinderBackLink(string reference, bool change);
        
        public string NumberOfOccupantsBackLink(string reference, HeatingType? heatingType, bool change);
        
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
                        linkGenerator.GetPathByAction("HouseType_Get", "EnergyEfficiency", new {reference}),
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

        public string RoofConstructionBackLink(string reference, PropertyType? propertyType, FlatType? flatType, FloorConstruction? floorConstruction, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : (propertyType, flatType) switch
                {
                    (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor)
                        => floorConstruction switch
                        {
                            FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix
                                => linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference }),
                            FloorConstruction.DoNotKnow or FloorConstruction.Other => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference }),
                            _ => throw new ArgumentOutOfRangeException()
                        },
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

        public string GlazingTypeBackLink(string reference, RoofConstruction? roofConstruction, AccessibleLoftSpace? accessibleLoftSpace, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : (roofConstruction, accessibleLoftSpace) switch
                {
                    (RoofConstruction.Flat, _)
                        => linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference }),
                    (RoofConstruction.Pitched or RoofConstruction.Mixed, AccessibleLoftSpace.No or AccessibleLoftSpace.DoNotKnow)
                        => linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency",
                            new { reference }),
                    (RoofConstruction.Pitched or RoofConstruction.Mixed, AccessibleLoftSpace.Yes)
                        => linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
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

        public string NumberOfOccupantsBackLink(string reference, HeatingType? heatingType, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : heatingType switch
                {
                    HeatingType.Storage or HeatingType.DirectActionElectric or HeatingType.HeatPump
                        or HeatingType.DoNotKnow
                        => linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference }),
                    HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler
                        => linkGenerator.GetPathByAction("HotWaterCylinder_Get", "EnergyEfficiency", new { reference }),
                    HeatingType.Other
                        => linkGenerator.GetPathByAction("OtherHeatingType_Get", "EnergyEfficiency", new { reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
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

        public string BackLink()
        {
            throw new NotImplementedException();
        }
    }
}