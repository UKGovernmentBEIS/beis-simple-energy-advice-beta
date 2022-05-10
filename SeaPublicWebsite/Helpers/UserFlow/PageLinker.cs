using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    { 
        public string BackLink(PageName page, UserDataModel userDataModel, bool change = false, string from = null);
    }

    public class PageLinker: IPageLinker
    {
        private readonly LinkGenerator linkGenerator;

        public PageLinker(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }
        
        public string BackLink(
            PageName page, 
            UserDataModel userData, 
            bool change = false,
            string from = null)
        {
            return page switch
            {
                PageName.NewOrReturningUser => NewOrReturningUserBackLink(),
                PageName.OwnershipStatus => OwnershipStatusBackLink(userData.Reference, change),
                PageName.Country => CountryBackLink(userData.Reference, change),
                PageName.ServiceUnsuitable => ServiceUnsuitableBackLink(userData.Reference, from),
                PageName.AskForPostcode => AskForPostcodeBackLink(userData.Reference),
                PageName.ConfirmAddress => ConfirmAddressBackLink(userData.Reference),
                PageName.PropertyType => PropertyTypeBackLink(userData.Reference, change),
                PageName.HouseType => HouseTypeBackLink(userData.Reference, change),
                PageName.BungalowType => BungalowTypeBackLink(userData.Reference, change),
                PageName.FlatType => FlatTypeBackLink(userData.Reference, change),
                PageName.HomeAge => HomeAgeBackLink(userData.Reference, userData.PropertyType, change),
                PageName.WallConstruction => WallConstructionBackLink(userData.Reference, change),
                PageName.CavityWallsInsulated => CavityWallsInsulatedBackLink(userData.Reference, change),
                PageName.SolidWallsInsulated => SolidWallsInsulatedBackLink(userData.Reference, userData.WallConstruction, change),
                PageName.FloorConstruction => FloorConstructionBackLink(userData.Reference, userData.WallConstruction, change),
                PageName.FloorInsulated => FloorInsulatedBackLink(userData.Reference, change),
                PageName.RoofConstruction => RoofConstructionBackLink(userData.Reference, userData.PropertyType, userData.FlatType, userData.FloorConstruction, change),
                PageName.AccessibleLoftSpace => AccessibleLoftSpaceBackLink(userData.Reference, change),
                PageName.RoofInsulated => RoofInsulatedBackLink(userData.Reference, change),
                PageName.OutdoorSpace => OutdoorSpaceBackLink(userData.Reference, change),
                PageName.GlazingType => GlazingTypeBackLink(userData, change),
                PageName.HeatingType => HeatingTypeBackLink(userData.Reference, change),
                PageName.OtherHeatingType => OtherHeatingTypeBackLink(userData.Reference, change),
                PageName.HotWaterCylinder => HotWaterCylinderBackLink(userData.Reference, change),
                PageName.NumberOfOccupants => NumberOfOccupantsBackLink(userData.Reference, userData.HeatingType, change),
                PageName.HeatingPattern => HeatingPatternBackLink(userData.Reference, change),
                PageName.Temperature => TemperatureBackLink(userData.Reference, change),
                PageName.EmailAddress => EmailAddressBackLink(userData.Reference, change),
                PageName.AnswerSummary => AnswerSummaryBackLink(userData.Reference),
                PageName.YourRecommendations => YourRecommendationsBackLink(userData.Reference),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private string NewOrReturningUserBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }

        private string OwnershipStatusBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference });
        }

        private string CountryBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        private string ServiceUnsuitableBackLink(string reference, string from)
        {
            return linkGenerator.GetPathByAction($"{from}_Get", "EnergyEfficiency", new { reference });
        }

        private string AskForPostcodeBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string ConfirmAddressBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string PropertyTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string HouseTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string BungalowTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string FlatTypeBackLink(string reference, bool change)
        {
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string HomeAgeBackLink(string reference, PropertyType? propertyType, bool change)
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

        private string WallConstructionBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency", new { reference });
        }

        private string CavityWallsInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string SolidWallsInsulatedBackLink(string reference, WallConstruction? wallConstruction, bool change)
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

        private string FloorConstructionBackLink(string reference, WallConstruction? wallConstruction, bool change)
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
                    _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
                };
        }

        private string FloorInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string RoofConstructionBackLink(string reference, PropertyType? propertyType, FlatType? flatType, FloorConstruction? floorConstruction, bool change)
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
                            _ => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference }),
                        },
                    _ => linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                        new { reference })
                };
        }

        private string AccessibleLoftSpaceBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference  })
                : linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string RoofInsulatedBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference });
        }

        private string OutdoorSpaceBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private string GlazingTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData switch
                {
                    {
                            WallConstruction: WallConstruction.Other or WallConstruction.DoNotKnow,
                            PropertyType: PropertyType.ApartmentFlatOrMaisonette, FlatType: FlatType.MiddleFloor
                        }
                        => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
                    {
                            WallConstruction: WallConstruction.Cavity,
                            PropertyType: PropertyType.ApartmentFlatOrMaisonette, FlatType: FlatType.MiddleFloor
                        }
                        => linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                            new { reference }),
                    {
                            WallConstruction: WallConstruction.Solid or WallConstruction.Mixed,
                            PropertyType: PropertyType.ApartmentFlatOrMaisonette, FlatType: FlatType.MiddleFloor
                        }
                        => linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency",
                            new { reference }),
                    {
                            FloorConstruction: FloorConstruction.Other or FloorConstruction.DoNotKnow,
                            PropertyType: PropertyType.ApartmentFlatOrMaisonette, FlatType: not FlatType.TopFloor
                        }
                        => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",
                            new { reference }),
                    {
                            FloorConstruction: FloorConstruction.SolidConcrete or FloorConstruction.SuspendedTimber
                            or FloorConstruction.Mix,
                            PropertyType: PropertyType.ApartmentFlatOrMaisonette, FlatType: not FlatType.TopFloor
                        }
                        => linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference }),
                    { RoofConstruction: RoofConstruction.Flat }
                        => linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference }),
                    { AccessibleLoftSpace: not AccessibleLoftSpace.Yes }
                        => linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency",
                            new { reference }),
                    _ => linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference }),
                };
        }

        private string HeatingTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency", new { reference });
        }

        private string OtherHeatingTypeBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        private string HotWaterCylinderBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        private string NumberOfOccupantsBackLink(string reference, HeatingType? heatingType, bool change)
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

        private string HeatingPatternBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference });
        }

        private string TemperatureBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference });
        }

        private string EmailAddressBackLink(string reference, bool change)
        {
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private string AnswerSummaryBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private string YourRecommendationsBackLink(string reference)
        {
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
        }
    }

    public enum PageName
    {
        NewOrReturningUser,
        OwnershipStatus,
        Country,
        ServiceUnsuitable,
        AskForPostcode,
        ConfirmAddress,
        PropertyType,
        HouseType,
        BungalowType,
        FlatType,
        HomeAge,
        WallConstruction,
        CavityWallsInsulated,
        SolidWallsInsulated,
        FloorConstruction,
        FloorInsulated,
        RoofConstruction,
        AccessibleLoftSpace,
        RoofInsulated,
        OutdoorSpace,
        GlazingType,
        HeatingType,
        OtherHeatingType,
        HotWaterCylinder,
        NumberOfOccupants,
        HeatingPattern,
        Temperature,
        EmailAddress,
        AnswerSummary,
        YourRecommendations
    }
}