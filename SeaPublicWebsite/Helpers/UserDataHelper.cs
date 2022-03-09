using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Helpers
{
    public class UserDataHelper
    {
        public static bool HasSolidFloor(UserDataModel userData)
        {
            return userData.FloorConstruction == FloorConstruction.SolidConcrete
                || (userData.FloorConstruction == FloorConstruction.DoNotKnow
                    && (userData.Epc?.FloorConstruction == FloorConstruction.SolidConcrete
                        || (userData.Epc?.FloorConstruction == null
                            && (userData.Epc?.ConstructionAgeBand != null && (int)userData.Epc?.ConstructionAgeBand > (int)HomeAge.From1950To1966)
                            || userData.YearBuilt > 1950)
                        )
                    );
        }

        public static bool HasInsulatedFloor(UserDataModel userData)
        {
            return userData.FloorInsulated == FloorInsulated.Yes
                || (userData.FloorInsulated == FloorInsulated.DoNotKnow
                    && (userData.Epc?.FloorInsulated == FloorInsulated.Yes
                        || (userData.Epc?.FloorInsulated == null 
                            && (userData.Epc?.ConstructionAgeBand != null && (int)userData.Epc?.ConstructionAgeBand >= (int)HomeAge.From1996To2002)
                            || userData.YearBuilt >= 1996)
                        )
                    );
        }

        public static bool HasCavityWalls(UserDataModel userData)
        {
            return userData.WallConstruction == WallConstruction.Cavity
                || (userData.WallConstruction == WallConstruction.DoNotKnow
                    && (userData.Epc?.WallConstruction == WallConstruction.Cavity
                        || (userData.Epc?.WallConstruction == null 
                            && (userData.Epc?.ConstructionAgeBand != null && (int)userData.Epc?.ConstructionAgeBand >= (int)HomeAge.From1930To1949)
                            || userData.YearBuilt >= 1930)
                        )
                    ); 
        }
        public static bool HasSolidWalls(UserDataModel userData)
        {
            return userData.WallConstruction == WallConstruction.Solid
                || (userData.WallConstruction == WallConstruction.DoNotKnow
                    && (userData.Epc?.WallConstruction == WallConstruction.Solid
                        || (userData.Epc?.WallConstruction == null 
                            && (userData.Epc?.ConstructionAgeBand != null && (int)userData.Epc?.ConstructionAgeBand < (int)HomeAge.From1930To1949)
                            || userData.YearBuilt < 1930)
                        )
                    );
        }

        public static bool HasInsulatedSolidWalls(UserDataModel userData)
        {
            return HasSolidWalls(userData) && userData.Epc?.SolidWallsInsulated == SolidWallsInsulated.All;
        }

        public static bool HasInsulatedCavityWalls(UserDataModel userData)
        {
            return HasCavityWalls(userData)
                && (userData.Epc?.CavityWallsInsulated == CavityWallsInsulated.All
                    || userData.Epc?.ConstructionAgeBand > HomeAge.From1991To1995
                    || userData.YearBuilt > 1991);
        }

        public static bool HasRoofInsulation(UserDataModel userData)
        {
            return userData.RoofConstruction is RoofConstruction.Pitched or RoofConstruction.Mixed
                   && userData.AccessibleLoftSpace is AccessibleLoftSpace.Yes or AccessibleLoftSpace.DoNotKnow
                   && userData.YearBuilt > 2002;
        }
    } 
}
