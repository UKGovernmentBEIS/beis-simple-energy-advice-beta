using System;
using Microsoft.AspNetCore.Routing;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers.UserFlow
{
    public interface IPageLinker
    { 
        public string BackLink(PageName page, UserDataModel userData, bool change = false, string from = null);
        
        public string ForwardLink(PageName page, UserDataModel userData, bool change = false, string houseNameOrNumber = null);
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

        public string ForwardLink(PageName page, UserDataModel userData, bool change = false, string houseNameOrNumber = null)
        {
            return page switch
            {
                PageName.NewOrReturningUser => NewOrReturningUserForwardLink(),
                PageName.OwnershipStatus => OwnershipStatusForwardLink(userData, change),
                PageName.Country => CountryForwardLink(userData, change),
                PageName.AskForPostcode => AskForPostcodeForwardLink(userData, houseNameOrNumber),
                PageName.ConfirmAddress => ConfirmAddressForwardLink(userData),
                PageName.PropertyType => PropertyTypeForwardLink(userData, change),
                PageName.HouseType => HouseTypeForwardLink(userData, change),
                PageName.BungalowType => BungalowTypeForwardLink(userData, change),
                PageName.FlatType => FlatTypeForwardLink(userData, change),
                PageName.HomeAge => HomeAgeForwardLink(userData, change),
                PageName.WallConstruction => WallConstructionForwardLink(userData, change),
                PageName.CavityWallsInsulated => CavityWallsInsulatedForwardLink(userData, change),
                PageName.SolidWallsInsulated => SolidWallsInsulatedForwardLink(userData, change),
                PageName.FloorConstruction => FloorConstructionForwardLink(userData, change),
                PageName.FloorInsulated => FloorInsulatedForwardLink(userData, change),
                PageName.RoofConstruction => RoofConstructionForwardLink(userData, change),
                PageName.AccessibleLoftSpace => AccessibleLoftSpaceForwardLink(userData, change),
                PageName.RoofInsulated => RoofInsulatedForwardLink(userData, change),
                PageName.OutdoorSpace => OutdoorSpaceForwardLink(userData, change),
                PageName.GlazingType => GlazingTypeForwardLink(userData, change),
                PageName.HeatingType => HeatingTypeForwardLink(userData, change),
                PageName.OtherHeatingType => OtherHeatingTypeForwardLink(userData, change),
                PageName.HotWaterCylinder => HotWaterCylinderForwardLink(userData, change),
                PageName.NumberOfOccupants => NumberOfOccupantsForwardLink(userData, change),
                PageName.HeatingPattern => HeatingPatternForwardLink(userData, change),
                PageName.Temperature => TemperatureForwardLink(userData, change),
                PageName.EmailAddress => EmailAddressForwardLink(userData, change),
                PageName.ServiceUnsuitable or PageName.AnswerSummary or PageName.YourRecommendations => throw new InvalidOperationException(),
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
                        linkGenerator.GetPathByAction("HouseType_Get", "EnergyEfficiency", new { reference}),
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
                        linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference }),
                    WallConstruction.Solid => 
                        linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency", new { reference }),
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

        private string OwnershipStatusForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (userData.OwnershipStatus == OwnershipStatus.PrivateTenancy)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "OwnershipStatus", reference });
            }

            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AskForPostcode_Get", "EnergyEfficiency", new { reference });
        }
        
        

        private string CountryForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (userData.Country != Country.England && userData.Country != Country.Wales)
            {
                return linkGenerator.GetPathByAction("ServiceUnsuitable", "EnergyEfficiency", new {from = "Country", reference });
            }

            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("OwnershipStatus_Get", "EnergyEfficiency", new { reference });
        }

        private string AskForPostcodeForwardLink(UserDataModel userData, string houseNameOrNumber)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("ConfirmAddress_Get", "EnergyEfficiency", new { reference, houseNameOrNumber });
        }

        private string ConfirmAddressForwardLink(UserDataModel userData)
        {
            var reference = userData.Reference;
            return linkGenerator.GetPathByAction("PropertyType_Get", "EnergyEfficiency", new { reference });
        }

        private string PropertyTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            switch (userData.PropertyType)
            {
                case PropertyType.House:
                    return linkGenerator.GetPathByAction("HouseType_Get", "EnergyEfficiency",new { reference, change});
                case PropertyType.Bungalow:
                    return linkGenerator.GetPathByAction("BungalowType_Get", "EnergyEfficiency",new { reference, change});
                case PropertyType.ApartmentFlatOrMaisonette:
                    return linkGenerator.GetPathByAction("FlatType_Get","EnergyEfficiency", new { reference, change});
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string HouseTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference});
        }

        private string BungalowTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference});
        }

        private string FlatTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HomeAge_Get", "EnergyEfficiency",new { reference});
        }

        private string HomeAgeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("WallConstruction_Get", "EnergyEfficiency",new { reference});
        }

        private string WallConstructionForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference});
            }
            else if (userData.WallConstruction == WallConstruction.Cavity ||
                     userData.WallConstruction == WallConstruction.Mixed)
            {
                return linkGenerator.GetPathByAction("CavityWallsInsulated_Get", "EnergyEfficiency", new { reference });
            }
            else if (userData.WallConstruction == WallConstruction.Solid)
            {
                return linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference });
            }
            else
            {
                // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
                if (userData.PropertyType == PropertyType.House ||
                    userData.PropertyType == PropertyType.Bungalow ||
                    (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.GroundFloor))
                {
                    return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",new { reference });
                }
                else if (userData.PropertyType == PropertyType.House ||
                         userData.PropertyType == PropertyType.Bungalow ||
                         (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.TopFloor))
                {
                    return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
                }
                else
                {
                    return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
                }
            }
        }

        private string CavityWallsInsulatedForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }
            else if (userData.WallConstruction == WallConstruction.Mixed)
            {
                return linkGenerator.GetPathByAction("SolidWallsInsulated_Get", "EnergyEfficiency", new { reference });
            }
            else
            {
                // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
                if (userData.PropertyType == PropertyType.House ||
                    userData.PropertyType == PropertyType.Bungalow ||
                    (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.GroundFloor))
                {
                    return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",new { reference });
                }
                else if (userData.PropertyType == PropertyType.House ||
                         userData.PropertyType == PropertyType.Bungalow ||
                         (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.TopFloor))
                {
                    return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
                }
                else
                {
                    return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
                }
            }
        }

        private string SolidWallsInsulatedForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }
            else if (userData.PropertyType == PropertyType.House ||
                     userData.PropertyType == PropertyType.Bungalow ||
                     (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.GroundFloor))
            {
                return linkGenerator.GetPathByAction("FloorConstruction_Get", "EnergyEfficiency",new { reference });
            }
            else if (userData.PropertyType == PropertyType.House ||
                     userData.PropertyType == PropertyType.Bungalow ||
                     (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.TopFloor))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }
            else
            {
                return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
            }
        }

        private string FloorConstructionForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }
            else if (userData.FloorConstruction == FloorConstruction.SolidConcrete 
                     || userData.FloorConstruction == FloorConstruction.SuspendedTimber 
                     || userData.FloorConstruction == FloorConstruction.Mix ) 
            {
                return linkGenerator.GetPathByAction("FloorInsulated_Get", "EnergyEfficiency",new { reference });
            }
            else if (userData.PropertyType == PropertyType.House ||
                     userData.PropertyType == PropertyType.Bungalow ||
                     (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.TopFloor))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }
            else
            {
                return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
            }
        }

        private string FloorInsulatedForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference });
            }
            else if (userData.PropertyType == PropertyType.House ||
                     userData.PropertyType == PropertyType.Bungalow ||
                     (userData.PropertyType == PropertyType.ApartmentFlatOrMaisonette && userData.FlatType == FlatType.TopFloor))
            {
                return linkGenerator.GetPathByAction("RoofConstruction_Get", "EnergyEfficiency",new { reference });
            }
            else
            {
                return linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
            }
        }

        private string RoofConstructionForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : userData.RoofConstruction == RoofConstruction.Flat 
                    ? linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference }) 
                    :  linkGenerator.GetPathByAction("AccessibleLoftSpace_Get", "EnergyEfficiency",new { reference});
        }

        private string AccessibleLoftSpaceForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : userData.AccessibleLoftSpace == AccessibleLoftSpace.Yes
                    ? linkGenerator.GetPathByAction("RoofInsulated_Get", "EnergyEfficiency",new { reference})
                    : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference });
        }

        private string RoofInsulatedForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("GlazingType_Get", "EnergyEfficiency",new { reference});
        }

        private string GlazingTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("OutdoorSpace_Get", "EnergyEfficiency",new { reference});
        }

        private string OutdoorSpaceForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingType_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            if (userData.HeatingType == HeatingType.Other)
            {
                return linkGenerator.GetPathByAction("OtherHeatingType_Get", "EnergyEfficiency", new { reference, change});
            }
            else if (change)
            {
                return linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference});
            }
            else if (userData.HeatingType == HeatingType.GasBoiler ||
                     userData.HeatingType == HeatingType.OilBoiler ||
                     userData.HeatingType == HeatingType.LpgBoiler)
            {
                return linkGenerator.GetPathByAction("HotWaterCylinder_Get", "EnergyEfficiency", new { reference, change});
            }
            else
            {
                return linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference});
            }
        }

        private string OtherHeatingTypeForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference });
        }

        private string HotWaterCylinderForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("NumberOfOccupants_Get", "EnergyEfficiency",new { reference});
        }

        private string NumberOfOccupantsForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("HeatingPattern_Get", "EnergyEfficiency",new { reference});
        }

        private string HeatingPatternForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("Temperature_Get", "EnergyEfficiency",new { reference});
        }

        private string TemperatureForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference})
                : linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency",new { reference });
        }

        private string EmailAddressForwardLink(UserDataModel userData, bool change)
        {
            var reference = userData.Reference;
            return change
                ? linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency", new { reference })
                : linkGenerator.GetPathByAction("AnswerSummary", "EnergyEfficiency",new { reference });
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
        GlazingType,
        OutdoorSpace,
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