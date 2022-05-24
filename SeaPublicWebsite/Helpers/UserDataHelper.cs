using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers
{
    public class UserDataHelper
    {
        // User has a solid floor
        // OR user do not know and the EPC states they have a solid floor
        // OR user does not know and property built after 1950
        // User year built is used if provided, over the EPC construction date
        public static bool HasSolidFloor(UserDataModel userData)
        {
            return userData.FloorConstruction == FloorConstruction.SolidConcrete
                   || userData.FloorConstruction == FloorConstruction.DoNotKnow
                   && (userData.Epc?.FloorConstruction == FloorConstruction.SolidConcrete
                       || userData.Epc?.FloorConstruction == null
                       && (!userData.YearBuilt.HasValue &&
                           userData.Epc?.ConstructionAgeBand >= HomeAge.From1950To1966 || userData.YearBuilt >= 1950));
        }

        // User has a suspended floor
        // OR user do not know and the EPC states they have a suspended floor
        // OR user does not know and property built before 1950
        // User year built is used if provided, over the EPC construction date
        public static bool HasSuspendedFloor(UserDataModel userData)
        {
            return userData.FloorConstruction == FloorConstruction.SuspendedTimber
                   || userData.FloorConstruction == FloorConstruction.DoNotKnow
                   && (userData.Epc?.FloorConstruction == FloorConstruction.SuspendedTimber
                       || userData.Epc?.FloorConstruction == null
                       && (!userData.YearBuilt.HasValue && userData.Epc?.ConstructionAgeBand < HomeAge.From1950To1966 ||
                           userData.YearBuilt < 1950));
        }

        // User has an insulated floor
        // OR user do not know and the EPC states they have an insulated floor
        // OR user does not know and property built after 1996
        // User year built is used if provided, over the EPC construction date
        public static bool HasInsulatedFloor(UserDataModel userData)
        {
            return userData.FloorInsulated == FloorInsulated.Yes
                   || userData.FloorInsulated == FloorInsulated.DoNotKnow
                   && (userData.Epc?.FloorInsulated == FloorInsulated.Yes
                       || userData.Epc?.FloorInsulated == null
                       && (!userData.YearBuilt.HasValue &&
                           userData.Epc?.ConstructionAgeBand >= HomeAge.From1996To2002 || userData.YearBuilt >= 1996));
        }

        // User has cavity walls
        // OR user do not know and the EPC states they have cavity walls
        // OR user does not know and property built after 1930
        // User year built is used if provided, over the EPC construction date
        public static bool HasCavityWalls(UserDataModel userData)
        {
            return userData.WallConstruction == WallConstruction.Cavity
                   || userData.WallConstruction == WallConstruction.DoNotKnow
                   && (userData.Epc?.WallConstruction == WallConstruction.Cavity
                       || userData.Epc?.WallConstruction == null
                       && (!userData.YearBuilt.HasValue &&
                           userData.Epc?.ConstructionAgeBand >= HomeAge.From1930To1949 || userData.YearBuilt >= 1930));
        }

        // User has solid walls
        // OR user do not know and the EPC states they have solid walls
        // OR user does not know and property built before 1930
        // User year built is used if provided, over the EPC construction date
        public static bool HasSolidWalls(UserDataModel userData)
        {
            return userData.WallConstruction == WallConstruction.Solid
                   || userData.WallConstruction == WallConstruction.DoNotKnow 
                   && (userData.Epc?.WallConstruction == WallConstruction.Solid
                       || userData.Epc?.WallConstruction == null 
                       && (!userData.YearBuilt.HasValue && userData.Epc?.ConstructionAgeBand < HomeAge.From1930To1949 || userData.YearBuilt < 1930));
        }

        // User has solid walls
        // AND user knows they are insulated OR they do not know and the EPC states they have solid insulated walls
        public static bool HasInsulatedSolidWalls(UserDataModel userData)
        {
            return HasSolidWalls(userData)
                   && (userData.SolidWallsInsulated == SolidWallsInsulated.All
                       || userData.SolidWallsInsulated == SolidWallsInsulated.DoNotKnow &&
                       userData.Epc?.SolidWallsInsulated == SolidWallsInsulated.All);
        }

        // User has cavity walls
        // AND user knows they are insulated
        // OR user do not know and the EPC states they have insulated walls
        // OR user does not know and property built is after 1991
        // User year built is used if provided, over the EPC construction date
        public static bool HasInsulatedCavityWalls(UserDataModel userData)
        {
            return HasCavityWalls(userData)
                   && (userData.CavityWallsInsulated == CavityWallsInsulated.All
                       || userData.CavityWallsInsulated == CavityWallsInsulated.DoNotKnow
                       && (userData.Epc?.CavityWallsInsulated == CavityWallsInsulated.All
                           || userData.Epc?.CavityWallsInsulated == null
                           && (!userData.YearBuilt.HasValue &&
                               userData.Epc?.ConstructionAgeBand > HomeAge.From1991To1995 ||
                               userData.YearBuilt > 1991)));
        }

        // User has a pitched roof (or some pitched)
        // User has accessible loft space, or does not know
        // User knows the roof is insulated or does not know and property built after 2002
        // User year built is used if provided, over the EPC construction date
        public static bool HasRoofInsulation(UserDataModel userData)
        {
            return userData.RoofConstruction is RoofConstruction.Pitched or RoofConstruction.Mixed
                   && userData.AccessibleLoftSpace is AccessibleLoftSpace.Yes or AccessibleLoftSpace.DoNotKnow
                   && (userData.RoofInsulated == RoofInsulated.Yes || userData.RoofInsulated == RoofInsulated.DoNotKnow
                       && (!userData.YearBuilt.HasValue && userData.Epc?.ConstructionAgeBand > HomeAge.From1996To2002 ||
                           userData.YearBuilt > 2002));
        }

        public static bool HasAnsweredFloorQuestions(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor) => true,
                _ => false
            };
        }
        
        public static bool HasAnsweredRoofQuestions(UserDataModel userData)
        {
            return (userData.PropertyType, userData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.TopFloor) => true,
                _ => false
            };
        }
        
        public static void ResetUnusedFields(UserDataModel userData)
        {
            if (userData.PropertyType is not PropertyType.House)
            {
                userData.HouseType = null;
            }
            
            if (userData.PropertyType is not PropertyType.Bungalow)
            {
                userData.BungalowType = null;
            }
            
            if (userData.PropertyType is not PropertyType.ApartmentFlatOrMaisonette)
            {
                userData.FlatType = null;
            }
            
            if (userData.WallConstruction is not WallConstruction.Cavity and not WallConstruction.Mixed)
            {
                userData.CavityWallsInsulated = null;
            }
            
            if (userData.WallConstruction is not WallConstruction.Solid and not WallConstruction.Mixed)
            {
                userData.SolidWallsInsulated = null;
            }
            
            if (!HasAnsweredFloorQuestions(userData))
            {
                userData.FloorConstruction = null;
            }

            if (userData.FloorConstruction is not FloorConstruction.SolidConcrete
                and not FloorConstruction.SuspendedTimber and not FloorConstruction.Mix)
            {
                userData.FloorInsulated = null;
            }

            if (!HasAnsweredRoofQuestions(userData))
            {
                userData.RoofConstruction = null;
            }

            if (userData.RoofConstruction is not RoofConstruction.Mixed and not RoofConstruction.Pitched)
            {
                userData.AccessibleLoftSpace = null;
            }

            if (userData.AccessibleLoftSpace is not AccessibleLoftSpace.Yes)
            {
                userData.RoofInsulated = null;
            }

            if (userData.HeatingType is not HeatingType.Other)
            {
                userData.OtherHeatingType = null;
            }

            if (userData.HeatingType is not HeatingType.GasBoiler and not HeatingType.OilBoiler
                and not HeatingType.LpgBoiler)
            {
                userData.HasHotWaterCylinder = null;
            }
        }
    }
}