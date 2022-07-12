using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Services
{
    public interface IQuestionFlowService
    { 
        public QuestionFlowPage BackDestination(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public QuestionFlowPage ForwardDestination(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
        
        public QuestionFlowPage SkipDestination(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null);
    }

    public class QuestionFlowService: IQuestionFlowService
    {
        public QuestionFlowPage BackDestination(
            QuestionFlowPage page, 
            PropertyData propertyData, 
            QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserBackDestination(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusBackDestination(),
                QuestionFlowPage.Country => CountryBackDestination(),
                QuestionFlowPage.FindEpc => FindEpcBackDestination(),
                QuestionFlowPage.ServiceUnsuitable => ServiceUnsuitableBackDestination(propertyData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeBackDestination(),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressBackDestination(),
                QuestionFlowPage.ConfirmEpcDetails => ConfirmEpcDetailsBackDestination(),
                QuestionFlowPage.NoEpcFound => NoEpcFoundBackDestination(),
                QuestionFlowPage.PropertyType => PropertyTypeBackDestination(propertyData, entryPoint),
                QuestionFlowPage.HouseType => HouseTypeBackDestination(),
                QuestionFlowPage.BungalowType => BungalowTypeBackDestination(),
                QuestionFlowPage.FlatType => FlatTypeBackDestination(),
                QuestionFlowPage.HomeAge => HomeAgeBackDestination(propertyData, entryPoint),
                QuestionFlowPage.CheckYourUnchangeableAnswers => CheckYourUnchangeableAnswersBackDestination(),
                QuestionFlowPage.WallConstruction => WallConstructionBackDestination(propertyData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedBackDestination(entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedBackDestination(propertyData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionBackDestination(propertyData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedBackDestination(entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionBackDestination(propertyData, entryPoint),
                QuestionFlowPage.LoftSpace => LoftSpaceBackDestination(entryPoint),
                QuestionFlowPage.LoftAccess => LoftAccessBackDestination(entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedBackDestination(entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceBackDestination(entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeBackDestination(propertyData, entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeBackDestination(entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeBackDestination(entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderBackDestination(entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsBackDestination(propertyData, entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternBackDestination(entryPoint),
                QuestionFlowPage.Temperature => TemperatureBackDestination(entryPoint),
                QuestionFlowPage.AnswerSummary => AnswerSummaryBackDestination(),
                QuestionFlowPage.NoRecommendations => NoRecommendationsBackDestination(),
                QuestionFlowPage.YourRecommendations => YourRecommendationsBackDestination(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public QuestionFlowPage ForwardDestination(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.NewOrReturningUser => NewOrReturningUserForwardDestination(),
                QuestionFlowPage.OwnershipStatus => OwnershipStatusForwardDestination(propertyData),
                QuestionFlowPage.Country => CountryForwardDestination(propertyData),
                QuestionFlowPage.FindEpc => FindEpcForwardDestination(propertyData),
                QuestionFlowPage.AskForPostcode => AskForPostcodeForwardDestination(propertyData),
                QuestionFlowPage.ConfirmAddress => ConfirmAddressForwardDestination(propertyData),
                QuestionFlowPage.ConfirmEpcDetails => ConfirmEpcDetailsForwardDestination(propertyData),
                QuestionFlowPage.NoEpcFound => NoEpcFoundForwardDestination(),
                QuestionFlowPage.PropertyType => PropertyTypeForwardDestination(propertyData),
                QuestionFlowPage.HouseType => HouseTypeForwardDestination(entryPoint),
                QuestionFlowPage.BungalowType => BungalowTypeForwardDestination(entryPoint),
                QuestionFlowPage.FlatType => FlatTypeForwardDestination(entryPoint),
                QuestionFlowPage.HomeAge => HomeAgeForwardDestination(),
                QuestionFlowPage.CheckYourUnchangeableAnswers => CheckYourUnchangeableAnswersForwardDestination(),
                QuestionFlowPage.WallConstruction => WallConstructionForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.CavityWallsInsulated => CavityWallsInsulatedForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.SolidWallsInsulated => SolidWallsInsulatedForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.FloorConstruction => FloorConstructionForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.FloorInsulated => FloorInsulatedForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.RoofConstruction => RoofConstructionForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.LoftSpace => LoftSpaceForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.LoftAccess => LoftAccessForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.RoofInsulated => RoofInsulatedForwardDestination(entryPoint),
                QuestionFlowPage.OutdoorSpace => OutdoorSpaceForwardDestination(entryPoint),
                QuestionFlowPage.GlazingType => GlazingTypeForwardDestination(entryPoint),
                QuestionFlowPage.HeatingType => HeatingTypeForwardDestination(propertyData, entryPoint),
                QuestionFlowPage.OtherHeatingType => OtherHeatingTypeForwardDestination(entryPoint),
                QuestionFlowPage.HotWaterCylinder => HotWaterCylinderForwardDestination(entryPoint),
                QuestionFlowPage.NumberOfOccupants => NumberOfOccupantsForwardDestination(entryPoint),
                QuestionFlowPage.HeatingPattern => HeatingPatternForwardDestination(entryPoint),
                QuestionFlowPage.Temperature => TemperatureForwardDestination(),
                QuestionFlowPage.AnswerSummary => AnswerSummaryForwardDestination(propertyData),
                _ => throw new ArgumentOutOfRangeException(nameof(page), page, null)
            };
        }

        public QuestionFlowPage SkipDestination(QuestionFlowPage page, PropertyData propertyData, QuestionFlowPage? entryPoint = null)
        {
            return page switch
            {
                QuestionFlowPage.AskForPostcode => AskForPostcodeSkipDestination(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private QuestionFlowPage NewOrReturningUserBackDestination()
        {
            return QuestionFlowPage.Start;
        }

        private QuestionFlowPage CountryBackDestination()
        {
            return QuestionFlowPage.NewOrReturningUser;
        }

        private QuestionFlowPage OwnershipStatusBackDestination()
        {
            return QuestionFlowPage.Country;
        }

        private QuestionFlowPage ServiceUnsuitableBackDestination(PropertyData propertyData)
        {
            return propertyData switch
            {
                { Country: not Country.England and not Country.Wales }
                    => QuestionFlowPage.Country,
                { OwnershipStatus: OwnershipStatus.PrivateTenancy }
                    => QuestionFlowPage.OwnershipStatus,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private QuestionFlowPage FindEpcBackDestination()
        {
            return QuestionFlowPage.OwnershipStatus;
        }
        
        private QuestionFlowPage AskForPostcodeBackDestination()
        {
            return QuestionFlowPage.FindEpc;
        }

        private QuestionFlowPage ConfirmAddressBackDestination()
        {
            return QuestionFlowPage.AskForPostcode;
        }

        private QuestionFlowPage ConfirmEpcDetailsBackDestination()
        {
            return QuestionFlowPage.AskForPostcode;
        }

        private QuestionFlowPage NoEpcFoundBackDestination()
        {
            return QuestionFlowPage.AskForPostcode;
        }

        private QuestionFlowPage PropertyTypeBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is QuestionFlowPage.PropertyType)
            {
                return QuestionFlowPage.CheckYourUnchangeableAnswers;
            }
            
            if (propertyData.Epc != null)
            {
                if (propertyData.Epc.ContainsPropertyTypeAndAge())
                {
                    return QuestionFlowPage.ConfirmEpcDetails;
                }
                return QuestionFlowPage.AskForPostcode;
            }

            if (propertyData.SearchForEpc == SearchForEpc.Yes)
            {
                return QuestionFlowPage.NoEpcFound;    
            }

            return QuestionFlowPage.FindEpc;
        }

        private QuestionFlowPage HouseTypeBackDestination()
        {
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage BungalowTypeBackDestination()
        {
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage FlatTypeBackDestination()
        {
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage HomeAgeBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.HomeAge
                ? QuestionFlowPage.CheckYourUnchangeableAnswers
                : propertyData.PropertyType switch
                {
                    PropertyType.House => 
                        QuestionFlowPage.HouseType,
                    PropertyType.Bungalow => 
                        QuestionFlowPage.BungalowType,
                    PropertyType.ApartmentFlatOrMaisonette => 
                        QuestionFlowPage.FlatType,
                    _ => throw new ArgumentOutOfRangeException()
                };
        }
        
        private QuestionFlowPage CheckYourUnchangeableAnswersBackDestination()
        {
            return QuestionFlowPage.HomeAge;
        }

        private QuestionFlowPage WallConstructionBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is QuestionFlowPage.WallConstruction)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.EpcDetailsConfirmed == EpcDetailsConfirmed.Yes)
            {
                return QuestionFlowPage.ConfirmEpcDetails;
            }
            return QuestionFlowPage.CheckYourUnchangeableAnswers;
        }

        private QuestionFlowPage CavityWallsInsulatedBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.CavityWallsInsulated
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.WallConstruction;
        }

        private QuestionFlowPage SolidWallsInsulatedBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.SolidWallsInsulated
                ? QuestionFlowPage.AnswerSummary
                : propertyData.WallConstruction switch
                {
                    WallConstruction.Mixed => 
                        QuestionFlowPage.CavityWallsInsulated,
                    WallConstruction.Solid => 
                        QuestionFlowPage.WallConstruction,
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private QuestionFlowPage FloorConstructionBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.FloorConstruction
                ? QuestionFlowPage.AnswerSummary
                : propertyData.WallConstruction switch
                {
                    WallConstruction.Solid or WallConstruction.Mixed =>
                        QuestionFlowPage.SolidWallsInsulated,
                    WallConstruction.Cavity =>
                        QuestionFlowPage.CavityWallsInsulated,
                    _ => QuestionFlowPage.WallConstruction
                };
        }

        private QuestionFlowPage FloorInsulatedBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.FloorInsulated
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.FloorConstruction;
        }

        private QuestionFlowPage RoofConstructionBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is QuestionFlowPage.RoofConstruction)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.HasFloor())
            {
                return propertyData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        QuestionFlowPage.FloorInsulated,
                    _ => QuestionFlowPage.FloorConstruction
                };
            }
            
            return propertyData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    QuestionFlowPage.SolidWallsInsulated,
                WallConstruction.Cavity =>
                    QuestionFlowPage.CavityWallsInsulated,
                _ => QuestionFlowPage.WallConstruction
            };
        }

        private QuestionFlowPage LoftSpaceBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.LoftSpace
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.RoofConstruction;
        }
        
        private QuestionFlowPage LoftAccessBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.LoftAccess
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.LoftSpace;
        }

        private QuestionFlowPage RoofInsulatedBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.RoofInsulated
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.LoftAccess;
        }

        private QuestionFlowPage GlazingTypeBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is QuestionFlowPage.GlazingType)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.HasRoof())
            {
                return propertyData switch
                {
                    { RoofConstruction: RoofConstruction.Flat }
                        => QuestionFlowPage.RoofConstruction,
                    { LoftSpace: not LoftSpace.Yes }
                        => QuestionFlowPage.LoftSpace,
                    { LoftAccess: not LoftAccess.Yes }
                        => QuestionFlowPage.LoftAccess,
                    _ => QuestionFlowPage.RoofInsulated
                };
            }

            if (propertyData.HasFloor())
            {
                return propertyData.FloorConstruction switch
                {
                    FloorConstruction.SuspendedTimber or FloorConstruction.SolidConcrete or FloorConstruction.Mix =>
                        QuestionFlowPage.FloorInsulated,
                    _ => QuestionFlowPage.FloorConstruction
                };
            }
            
            return propertyData.WallConstruction switch
            {
                WallConstruction.Solid or WallConstruction.Mixed =>
                    QuestionFlowPage.SolidWallsInsulated,
                WallConstruction.Cavity =>
                    QuestionFlowPage.CavityWallsInsulated,
                _ => QuestionFlowPage.WallConstruction
            };
        }

        private QuestionFlowPage OutdoorSpaceBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.OutdoorSpace
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage HeatingTypeBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.HeatingType
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.OutdoorSpace;
        }

        private QuestionFlowPage OtherHeatingTypeBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.OtherHeatingType
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.HeatingType;
        }

        private QuestionFlowPage HotWaterCylinderBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.HotWaterCylinder
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.HeatingType;
        }

        private QuestionFlowPage NumberOfOccupantsBackDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.NumberOfOccupants
                ? QuestionFlowPage.AnswerSummary
                : propertyData.HeatingType switch
                {
                    HeatingType.Storage or HeatingType.DirectActionElectric or HeatingType.HeatPump
                        or HeatingType.DoNotKnow
                        => QuestionFlowPage.HeatingType,
                    HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler
                        => QuestionFlowPage.HotWaterCylinder,
                    HeatingType.Other
                        => QuestionFlowPage.OtherHeatingType,
                    _ => throw new ArgumentOutOfRangeException()
                };
        }

        private QuestionFlowPage HeatingPatternBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.HeatingPattern
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.NumberOfOccupants;
        }

        private QuestionFlowPage TemperatureBackDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is QuestionFlowPage.Temperature
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.HeatingPattern;
        }

        private QuestionFlowPage AnswerSummaryBackDestination()
        {
            return QuestionFlowPage.Temperature;
        }

        private QuestionFlowPage NoRecommendationsBackDestination()
        {
            return QuestionFlowPage.AnswerSummary;
        }

        private QuestionFlowPage YourRecommendationsBackDestination()
        {
            return QuestionFlowPage.AnswerSummary;
        }

        private QuestionFlowPage NewOrReturningUserForwardDestination()
        {
            // TODO: Routing for the first step?
            throw new InvalidOperationException();
        }
        
        private QuestionFlowPage CountryForwardDestination(PropertyData propertyData)
        {
            return propertyData.Country is not Country.England and not Country.Wales 
                ? QuestionFlowPage.ServiceUnsuitable
                : QuestionFlowPage.OwnershipStatus;
        }

        private QuestionFlowPage OwnershipStatusForwardDestination(PropertyData propertyData)
        {
            return propertyData.OwnershipStatus is OwnershipStatus.PrivateTenancy 
                ? QuestionFlowPage.ServiceUnsuitable
                : QuestionFlowPage.FindEpc;
        }

        private QuestionFlowPage FindEpcForwardDestination(PropertyData propertyData)
        {
            if (propertyData.SearchForEpc == SearchForEpc.Yes)
            {
                return QuestionFlowPage.AskForPostcode;
            }
            
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage AskForPostcodeForwardDestination(PropertyData propertyData)
        {
            if (propertyData.SearchForEpc == SearchForEpc.Yes)
            {
                return QuestionFlowPage.ConfirmAddress;
            }
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage ConfirmAddressForwardDestination(PropertyData propertyData)
        {
            var epc = propertyData.Epc;
            if (epc != null)
            {
                if (epc.ContainsPropertyTypeAndAge())
                {
                    return QuestionFlowPage.ConfirmEpcDetails;
                }
                return QuestionFlowPage.PropertyType;
            }
            return QuestionFlowPage.NoEpcFound;
        }

        private QuestionFlowPage ConfirmEpcDetailsForwardDestination(PropertyData propertyData)
        {
            if (propertyData.EpcDetailsConfirmed == EpcDetailsConfirmed.Yes)
            {
                return QuestionFlowPage.WallConstruction;
            }
            return QuestionFlowPage.PropertyType;
        }

        private QuestionFlowPage NoEpcFoundForwardDestination()
        {
            return QuestionFlowPage.PropertyType;
        }
        
        private QuestionFlowPage PropertyTypeForwardDestination(PropertyData propertyData)
        {
            return propertyData.PropertyType switch
            {
                PropertyType.House =>
                    QuestionFlowPage.HouseType,
                PropertyType.Bungalow =>
                    QuestionFlowPage.BungalowType,
                PropertyType.ApartmentFlatOrMaisonette =>
                    QuestionFlowPage.FlatType,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        private QuestionFlowPage HouseTypeForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.CheckYourUnchangeableAnswers
                : QuestionFlowPage.HomeAge;
        }

        private QuestionFlowPage BungalowTypeForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.CheckYourUnchangeableAnswers
                : QuestionFlowPage.HomeAge;
        }

        private QuestionFlowPage FlatTypeForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.CheckYourUnchangeableAnswers
                : QuestionFlowPage.HomeAge;
        }

        private QuestionFlowPage HomeAgeForwardDestination()
        {
            return QuestionFlowPage.CheckYourUnchangeableAnswers;
        }

        private QuestionFlowPage CheckYourUnchangeableAnswersForwardDestination()
        {
            return QuestionFlowPage.WallConstruction;
        }
        
        private QuestionFlowPage WallConstructionForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {

            if (propertyData.WallConstruction is WallConstruction.Cavity or WallConstruction.Mixed)
            {
                return QuestionFlowPage.CavityWallsInsulated;
            }

            if (propertyData.WallConstruction == WallConstruction.Solid)
            {
                return QuestionFlowPage.SolidWallsInsulated;
            }
            
            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            // These options below are for people who have chosen "Don't know" to "What type of walls do you have?"
            if (propertyData.HasFloor())
            {
                return QuestionFlowPage.FloorConstruction;
            }

            if (propertyData.HasRoof())
            {
                return QuestionFlowPage.RoofConstruction;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage CavityWallsInsulatedForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            
            if (entryPoint is QuestionFlowPage.CavityWallsInsulated)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.WallConstruction is WallConstruction.Mixed)
            {
                return QuestionFlowPage.SolidWallsInsulated;
            }
            
            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            // These options below are for people who have finished the "wall insulation" questions (e.g. who only have cavity walls)
            if (propertyData.HasFloor())
            {
                return QuestionFlowPage.FloorConstruction;
            }

            if (propertyData.HasRoof())
            {
                return QuestionFlowPage.RoofConstruction;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage SolidWallsInsulatedForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.HasFloor())
            {
                return QuestionFlowPage.FloorConstruction;
            }

            if (propertyData.HasRoof())
            {
                return QuestionFlowPage.RoofConstruction;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage FloorConstructionForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {

            if (propertyData.FloorConstruction is FloorConstruction.SolidConcrete or FloorConstruction.SuspendedTimber or FloorConstruction.Mix ) 
            {
                return QuestionFlowPage.FloorInsulated;
            }
            
            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.HasRoof())
            {
                return QuestionFlowPage.RoofConstruction;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage FloorInsulatedForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            if (propertyData.HasRoof())
            {
                return QuestionFlowPage.RoofConstruction;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage RoofConstructionForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (propertyData.RoofConstruction is RoofConstruction.Mixed or RoofConstruction.Pitched)
            {
                return QuestionFlowPage.LoftSpace;
            }

            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage LoftSpaceForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (propertyData.LoftSpace is LoftSpace.Yes)
            {
                return QuestionFlowPage.LoftAccess;
            }

            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            return QuestionFlowPage.GlazingType;
        }
        
        private QuestionFlowPage LoftAccessForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (propertyData.LoftAccess is LoftAccess.Yes)
            {
                return QuestionFlowPage.RoofInsulated;
            }

            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            return QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage RoofInsulatedForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.GlazingType;
        }

        private QuestionFlowPage GlazingTypeForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.OutdoorSpace;
        }

        private QuestionFlowPage OutdoorSpaceForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.HeatingType;
        }

        private QuestionFlowPage HeatingTypeForwardDestination(PropertyData propertyData, QuestionFlowPage? entryPoint)
        {
            if (propertyData.HeatingType == HeatingType.Other)
            {
                return QuestionFlowPage.OtherHeatingType;
            }

            if (propertyData.HeatingType is HeatingType.GasBoiler or HeatingType.OilBoiler or HeatingType.LpgBoiler)
            {
                return QuestionFlowPage.HotWaterCylinder;
            }

            if (entryPoint is not null)
            {
                return QuestionFlowPage.AnswerSummary;
            }

            return QuestionFlowPage.NumberOfOccupants;
        }

        private QuestionFlowPage OtherHeatingTypeForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.NumberOfOccupants;
        }

        private QuestionFlowPage HotWaterCylinderForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.NumberOfOccupants;
        }

        private QuestionFlowPage NumberOfOccupantsForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.HeatingPattern;
        }

        private QuestionFlowPage HeatingPatternForwardDestination(QuestionFlowPage? entryPoint)
        {
            return entryPoint is not null
                ? QuestionFlowPage.AnswerSummary
                : QuestionFlowPage.Temperature;
        }

        private QuestionFlowPage TemperatureForwardDestination()
        {
            return QuestionFlowPage.AnswerSummary;
        }

        private QuestionFlowPage AnswerSummaryForwardDestination(PropertyData propertyData)
        {
            return propertyData.PropertyRecommendations.Any()
                ? QuestionFlowPage.YourRecommendations
                : QuestionFlowPage.NoRecommendations;
        }
        
        private QuestionFlowPage AskForPostcodeSkipDestination()
        {
            return QuestionFlowPage.PropertyType;
        }
    }

    public enum QuestionFlowPage
    {
        Start,
        NewOrReturningUser,
        Country,
        OwnershipStatus,
        ServiceUnsuitable,
        FindEpc,
        AskForPostcode,
        ConfirmAddress,
        ConfirmEpcDetails,
        NoEpcFound,
        PropertyType,
        HouseType,
        BungalowType,
        FlatType,
        HomeAge,
        CheckYourUnchangeableAnswers,
        WallConstruction,
        CavityWallsInsulated,
        SolidWallsInsulated,
        FloorConstruction,
        FloorInsulated,
        RoofConstruction,
        LoftSpace,
        LoftAccess,
        RoofInsulated,
        GlazingType,
        OutdoorSpace,
        HeatingType,
        OtherHeatingType,
        HotWaterCylinder,
        NumberOfOccupants,
        HeatingPattern,
        Temperature,
        AnswerSummary,
        NoRecommendations,
        YourRecommendations
    }
}