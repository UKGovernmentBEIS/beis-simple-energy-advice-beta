using System;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    { 
        public string BackLink(PageName page, UserDataModel userData, bool change = false, string from = null);
        
        public string ForwardLink(PageName page, UserDataModel userData, bool change = false);
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
                PageName.OwnershipStatus => OwnershipStatusBackLink(userData, change),
                PageName.Country => CountryBackLink(userData, change),
                PageName.ServiceUnsuitable => ServiceUnsuitableBackLink(userData, from),
                PageName.AskForPostcode => AskForPostcodeBackLink(userData),
                PageName.ConfirmAddress => ConfirmAddressBackLink(userData),
                PageName.PropertyType => PropertyTypeBackLink(userData, change),
                PageName.HouseType => HouseTypeBackLink(userData, change),
                PageName.BungalowType => BungalowTypeBackLink(userData, change),
                PageName.FlatType => FlatTypeBackLink(userData, change),
                PageName.HomeAge => HomeAgeBackLink(userData, change),
                PageName.WallConstruction => WallConstructionBackLink(userData, change),
                PageName.CavityWallsInsulated => CavityWallsInsulatedBackLink(userData, change),
                PageName.SolidWallsInsulated => SolidWallsInsulatedBackLink(userData, change),
                PageName.FloorConstruction => FloorConstructionBackLink(userData, change),
                PageName.FloorInsulated => FloorInsulatedBackLink(userData, change),
                PageName.RoofConstruction => RoofConstructionBackLink(userData, change),
                PageName.AccessibleLoftSpace => AccessibleLoftSpaceBackLink(userData, change),
                PageName.RoofInsulated => RoofInsulatedBackLink(userData, change),
                PageName.OutdoorSpace => OutdoorSpaceBackLink(userData, change),
                PageName.GlazingType => GlazingTypeBackLink(userData, change),
                PageName.HeatingType => HeatingTypeBackLink(userData, change),
                PageName.OtherHeatingType => OtherHeatingTypeBackLink(userData, change),
                PageName.HotWaterCylinder => HotWaterCylinderBackLink(userData, change),
                PageName.NumberOfOccupants => NumberOfOccupantsBackLink(userData, change),
                PageName.HeatingPattern => HeatingPatternBackLink(userData, change),
                PageName.Temperature => TemperatureBackLink(userData, change),
                PageName.EmailAddress => EmailAddressBackLink(userData, change),
                PageName.AnswerSummary => AnswerSummaryBackLink(userData),
                PageName.YourRecommendations => YourRecommendationsBackLink(userData),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string ForwardLink(PageName page, UserDataModel userData, bool change = false)
        {
            var expr = "";
            return page switch
            {
                PageName.NewOrReturningUser => NewOrReturningUserForwardLink(),
                PageName.OwnershipStatus => OwnerShipStatusForwardLink(userData, change),
                PageName.Country => expr,
                PageName.ServiceUnsuitable => expr,
                PageName.AskForPostcode => expr,
                PageName.ConfirmAddress => expr,
                PageName.PropertyType => expr,
                PageName.HouseType => expr,
                PageName.BungalowType => expr,
                PageName.FlatType => expr,
                PageName.HomeAge => expr,
                PageName.WallConstruction => expr,
                PageName.CavityWallsInsulated => expr,
                PageName.SolidWallsInsulated => expr,
                PageName.FloorConstruction => expr,
                PageName.FloorInsulated => expr,
                PageName.RoofConstruction => expr,
                PageName.AccessibleLoftSpace => expr,
                PageName.RoofInsulated => expr,
                PageName.OutdoorSpace => expr,
                PageName.GlazingType => expr,
                PageName.HeatingType => expr,
                PageName.OtherHeatingType => expr,
                PageName.HotWaterCylinder => expr,
                PageName.NumberOfOccupants => expr,
                PageName.HeatingPattern => expr,
                PageName.Temperature => expr,
                PageName.EmailAddress => expr,
                PageName.AnswerSummary => expr,
                PageName.YourRecommendations => expr,
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }
        
        private string NewOrReturningUserBackLink()
        {
            return linkGenerator.GetPathByAction("Index", "EnergyEfficiency");
        }

        private string OwnershipStatusBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Country_Get", "EnergyEfficiency", new { reference });
        }

        private string CountryBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NewOrReturningUser_Get", "EnergyEfficiency");
        }

        private string ServiceUnsuitableBackLink(UserDataModel userData, string from)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction($"{from}_Get", "EnergyEfficiency", new { reference });
        }

        private string AskForPostcodeBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string ConfirmAddressBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string PropertyTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }

        private string HouseTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string BungalowTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string FlatTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference, change });
        }

        private string HomeAgeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.PropertyType switch
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

        private string WallConstructionBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency", new { reference });
        }

        private string CavityWallsInsulatedBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string SolidWallsInsulatedBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Mixed => 
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new {reference }),
                    WallConstruction.Solid => 
                        linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new {reference }),
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private string FloorConstructionBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.WallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference }),
                    WallConstruction.Cavity =>
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                            new { reference }),
                    _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
                };
        }

        private string FloorInsulatedBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string RoofConstructionBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference }),
                    _ => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference }),
                WallConstruction.Cavity =>
                    linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                        new { reference }),
                _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
            };
        }

        private string AccessibleLoftSpaceBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference  })
                : linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference });
        }

        private string RoofInsulatedBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency", new { reference });
        }

        private string GlazingTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }

            if (HasRoof(userData))
            {
                return userData switch
                {
                    { RoofConstruction: RoofConstruction.Flat }
                        => linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency", new { reference }),
                    { AccessibleLoftSpace: not AccessibleLoftSpace.Yes }
                        => linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency",
                            new { reference }),
                    _ => linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency", new { reference }),
                };
            }

            if (HasFloor(userData))
            {
                return userData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency", new { reference }),
                    _ => linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency", new { reference }),
                };
            }
            
            return userData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference }),
                WallConstruction.Cavity =>
                    linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency",
                        new { reference }),
                _ => linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
            };
        }

        private string OutdoorSpaceBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency", new { reference });
        }

        private string HeatingTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency", new { reference });
        }

        private string OtherHeatingTypeBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        private string HotWaterCylinderBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency", new { reference });
        }

        private string NumberOfOccupantsBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : userData.HeatingType switch
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

        private string HeatingPatternBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency", new { reference });
        }

        private string TemperatureBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency", new { reference });
        }

        private string EmailAddressBackLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private string AnswerSummaryBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency", new { reference });
        }

        private string YourRecommendationsBackLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
        }

        private string NewOrReturningUserForwardLink()
        {
            // TODO: Routing for the first step?
            throw new InvalidOperationException();
        }

        private string OwnerShipStatusForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (userData.OwnershipStatus == OwnershipStatus.PrivateTenancy)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "OwnershipStatus", reference });
            }

            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new {reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new {reference });
        }

        private bool HasFloor(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor) => true,
                _ => false
            };
        }

        private bool HasRoof(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.TopFloor) => true,
                _ => false
            };
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