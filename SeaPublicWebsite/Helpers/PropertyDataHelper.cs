using SeaPublicWebsite.Data.DataModels;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers
{
    public class PropertyDataHelper
    {
        // User has a solid floor
        // OR user do not know and the EPC states they have a solid floor
        // OR user does not know and property built after 1950
        // User year built is used if provided, over the EPC construction date
        public static bool HasSolidFloor(PropertyData propertyData)
        {
            return propertyData.FloorConstruction == FloorConstruction.SolidConcrete
                   || propertyData.FloorConstruction == FloorConstruction.DoNotKnow
                   && (propertyData.Epc?.FloorConstruction == FloorConstruction.SolidConcrete
                       || propertyData.Epc?.FloorConstruction == null
                       && (!propertyData.YearBuilt.HasValue &&
                           propertyData.Epc?.ConstructionAgeBand >= HomeAge.From1950To1966 || propertyData.YearBuilt >= 1950));
        }

        // User has a suspended floor
        // OR user do not know and the EPC states they have a suspended floor
        // OR user does not know and property built before 1950
        // User year built is used if provided, over the EPC construction date
        public static bool HasSuspendedFloor(PropertyData propertyData)
        {
            return propertyData.FloorConstruction == FloorConstruction.SuspendedTimber
                   || propertyData.FloorConstruction == FloorConstruction.DoNotKnow
                   && (propertyData.Epc?.FloorConstruction == FloorConstruction.SuspendedTimber
                       || propertyData.Epc?.FloorConstruction == null
                       && (!propertyData.YearBuilt.HasValue && propertyData.Epc?.ConstructionAgeBand < HomeAge.From1950To1966 ||
                           propertyData.YearBuilt < 1950));
        }

        // User has an insulated floor
        // OR user do not know and the EPC states they have an insulated floor
        // OR user does not know and property built after 1996
        // User year built is used if provided, over the EPC construction date
        public static bool HasInsulatedFloor(PropertyData propertyData)
        {
            return propertyData.FloorInsulated == FloorInsulated.Yes
                   || propertyData.FloorInsulated == FloorInsulated.DoNotKnow
                   && (propertyData.Epc?.FloorInsulated == FloorInsulated.Yes
                       || propertyData.Epc?.FloorInsulated == null
                       && (!propertyData.YearBuilt.HasValue &&
                           propertyData.Epc?.ConstructionAgeBand >= HomeAge.From1996To2002 || propertyData.YearBuilt >= 1996));
        }

        // User has cavity walls
        // OR user do not know and the EPC states they have cavity walls
        // OR user does not know and property built after 1930
        // User year built is used if provided, over the EPC construction date
        public static bool HasCavityWalls(PropertyData propertyData)
        {
            return propertyData.WallConstruction == WallConstruction.Cavity
                   || propertyData.WallConstruction == WallConstruction.DoNotKnow
                   && (propertyData.Epc?.WallConstruction == WallConstruction.Cavity
                       || propertyData.Epc?.WallConstruction == null
                       && (!propertyData.YearBuilt.HasValue &&
                           propertyData.Epc?.ConstructionAgeBand >= HomeAge.From1930To1949 || propertyData.YearBuilt >= 1930));
        }

        // User has solid walls
        // OR user do not know and the EPC states they have solid walls
        // OR user does not know and property built before 1930
        // User year built is used if provided, over the EPC construction date
        public static bool HasSolidWalls(PropertyData propertyData)
        {
            return propertyData.WallConstruction == WallConstruction.Solid
                   || propertyData.WallConstruction == WallConstruction.DoNotKnow 
                   && (propertyData.Epc?.WallConstruction == WallConstruction.Solid
                       || propertyData.Epc?.WallConstruction == null 
                       && (!propertyData.YearBuilt.HasValue && propertyData.Epc?.ConstructionAgeBand < HomeAge.From1930To1949 || propertyData.YearBuilt < 1930));
        }

        // User has solid walls
        // AND user knows they are insulated OR they do not know and the EPC states they have solid insulated walls
        public static bool HasInsulatedSolidWalls(PropertyData propertyData)
        {
            return HasSolidWalls(propertyData)
                   && (propertyData.SolidWallsInsulated == SolidWallsInsulated.All
                       || propertyData.SolidWallsInsulated == SolidWallsInsulated.DoNotKnow &&
                       propertyData.Epc?.SolidWallsInsulated == SolidWallsInsulated.All);
        }

        // User has cavity walls
        // AND user knows they are insulated
        // OR user do not know and the EPC states they have insulated walls
        // OR user does not know and property built is after 1991
        // User year built is used if provided, over the EPC construction date
        public static bool HasInsulatedCavityWalls(PropertyData propertyData)
        {
            return HasCavityWalls(propertyData)
                   && (propertyData.CavityWallsInsulated == CavityWallsInsulated.All
                       || propertyData.CavityWallsInsulated == CavityWallsInsulated.DoNotKnow
                       && (propertyData.Epc?.CavityWallsInsulated == CavityWallsInsulated.All
                           || propertyData.Epc?.CavityWallsInsulated == null
                           && (!propertyData.YearBuilt.HasValue &&
                               propertyData.Epc?.ConstructionAgeBand > HomeAge.From1991To1995 ||
                               propertyData.YearBuilt > 1991)));
        }

        // User has a pitched roof (or some pitched)
        // User has accessible loft space, or does not know
        // User knows the roof is insulated or does not know and property built after 2002
        // User year built is used if provided, over the EPC construction date
        public static bool HasRoofInsulation(PropertyData propertyData)
        {
            return propertyData.RoofConstruction is RoofConstruction.Pitched or RoofConstruction.Mixed
                   && propertyData.AccessibleLoftSpace is AccessibleLoftSpace.Yes or AccessibleLoftSpace.DoNotKnow
                   && (propertyData.RoofInsulated == RoofInsulated.Yes || propertyData.RoofInsulated == RoofInsulated.DoNotKnow
                       && (!propertyData.YearBuilt.HasValue && propertyData.Epc?.ConstructionAgeBand > HomeAge.From1996To2002 ||
                           propertyData.YearBuilt > 2002));
        }

        public static bool HasFloor(PropertyData propertyData)
        {
            return (propertyData.PropertyType, propertyData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.GroundFloor) => true,
                _ => false
            };
        }
        
        public static bool HasRoof(PropertyData propertyData)
        {
            return (propertyData.PropertyType, propertyData.FlatType) switch
            {
                (PropertyType.House, _) or (PropertyType.Bungalow, _) or (PropertyType.ApartmentFlatOrMaisonette, FlatType.TopFloor) => true,
                _ => false
            };
        }
        
        public static void ResetUnusedFields(PropertyData propertyData)
        {
            if (propertyData.PropertyType is not PropertyType.House)
            {
                propertyData.HouseType = null;
            }
            
            if (propertyData.PropertyType is not PropertyType.Bungalow)
            {
                propertyData.BungalowType = null;
            }
            
            if (propertyData.PropertyType is not PropertyType.ApartmentFlatOrMaisonette)
            {
                propertyData.FlatType = null;
            }
            
            if (propertyData.WallConstruction is not WallConstruction.Cavity and not WallConstruction.Mixed)
            {
                propertyData.CavityWallsInsulated = null;
            }
            
            if (propertyData.WallConstruction is not WallConstruction.Solid and not WallConstruction.Mixed)
            {
                propertyData.SolidWallsInsulated = null;
            }
            
            if (!HasFloor(propertyData))
            {
                propertyData.FloorConstruction = null;
            }

            if (propertyData.FloorConstruction is not FloorConstruction.SolidConcrete
                and not FloorConstruction.SuspendedTimber and not FloorConstruction.Mix)
            {
                propertyData.FloorInsulated = null;
            }

            if (!HasRoof(propertyData))
            {
                propertyData.RoofConstruction = null;
            }

            if (propertyData.RoofConstruction is not RoofConstruction.Mixed and not RoofConstruction.Pitched)
            {
                propertyData.AccessibleLoftSpace = null;
            }

            if (propertyData.AccessibleLoftSpace is not AccessibleLoftSpace.Yes)
            {
                propertyData.RoofInsulated = null;
            }

            if (propertyData.HeatingType is not HeatingType.Other)
            {
                propertyData.OtherHeatingType = null;
            }

            if (propertyData.HeatingType is not HeatingType.GasBoiler and not HeatingType.OilBoiler
                and not HeatingType.LpgBoiler)
            {
                propertyData.HasHotWaterCylinder = null;
            }
        }
    }
}