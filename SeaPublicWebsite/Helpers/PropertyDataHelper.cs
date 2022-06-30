using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Helpers
{
    public class PropertyDataHelper
    {
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
                propertyData.LoftSpace = null;
            }

            if (propertyData.LoftSpace is not LoftSpace.Yes)
            {
                propertyData.LoftAccess = null;
            }

            if (propertyData.LoftAccess is not LoftAccess.Yes)
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

            if (propertyData.HeatingPattern is not HeatingPattern.Other)
            {
                propertyData.HoursOfHeatingMorning = null;
                propertyData.HoursOfHeatingEvening = null;
            }
        }
    }
}