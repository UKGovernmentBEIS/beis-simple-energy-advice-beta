using System.Collections.Generic;
using System.Linq;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.Models.EnergyEfficiency;

namespace SeaPublicWebsite.DataStores
{
    public static class RecommendationService
    {
        private static readonly List<Recommendation> Recommendations =
             new()
             {
                new Recommendation
                {
                    Key = RecommendationKey.AddLoftInsulation,
                    Title = "Add more loft insulation",
                    MinInstallCost = 300,
                    MaxInstallCost = 700,
                    Saving = 45,
                    Disruption = "Minimal",
                    InstallationTime = "Less than a day",
                    Description = new List<string>{
                        "It looks like you have some insulation in your loft but it’s less " +
                        "than the recommended amount. It could be worthwhile adding some more " +
                        "to bring it up to the recommended thickness of 300mm, or 12 inches. ",
                        "Adding extra insulation can help trap the heat inside your property so your " +
                        "heating doesn’t need to be on as much."
                    },
                    Considerations = 
                        "If some of your loft is boarded so that you " +
                        "can store things up there you will either need to raise this " +
                        "boarding up and insulate underneath it, or leave that part of " +
                        "the loft with thinner insulation.",
                    FurtherInfo = "loft insulation",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "You have a pitched (sloping) roof with an unheated loft space that " +
                            "someone could easily get into.",
                            "Your loft has less than the recommended amount of insulation. This is " +
                            "300mm or 12 inches thick. "
                        }
                    }
                },
                new Recommendation
                {
                    Key = RecommendationKey.GroundFloorInsulation,
                    Title = "Insulate the ground floor",
                    MinInstallCost = 1200,
                    MaxInstallCost = 1800,
                    Saving = 75,
                    Disruption = "Definitely",
                    InstallationTime = "1 - 2 days",
                    Description = new List<string>{
                        "From the information we have it looks like you have a suspended timber floor. " +
                        "This means your ground floor is made of wooden floorboards with a gap underneath.",
                        "Adding extra insulation can help trap the heat inside your property so your " +
                        "heating doesn’t need to be on as much."
                    },
                    Considerations = 
                        "Your floor could be insulated by lifting the floorboards, fitting " +
                        "insulation between the joists and then putting the floorboards back. This is normally a " +
                        "job for a professional installer .",
                    Caution = 
                        "You will need to completely empty the rooms that are being insulated, " +
                        "and remove any carpet or other floor covering, before the floor boards can be lifted.",
                    FurtherInfo = "floor insulation",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "Your property has a suspended timber floor that has not yet been insulated. " +
                            "If you are unsure, a qualified installer will be able to confirm this for you."
                        }
                    }
                },
                new Recommendation
                {
                    Key = RecommendationKey.UpgradeHeatingControls,
                    Title = "Upgrade your heating controls",
                    MinInstallCost = 150,
                    MaxInstallCost = 400,
                    Saving = 75,
                    Disruption = "Minimal",
                    InstallationTime = "Less than a day",
                    Description = new List<string>{
                        "If you have a central heating system then it’s important you can control it " +
                        "effectively, so that you can stay warm without wasting energy.",
                        "The standard way to do that is to have a central programmer and thermostat to " +
                        "manage when the heating is on and how much, as well as thermostatic radiator valves " +
                        "(TRVs) to control the heating in each room.",
                        "Alternatively, you may want to choose a smart heating controller to make it easier to " +
                        "adjust the settings. "
                    },
                    Caution = " You will need a competent installer to fit new controls to your heating system.",
                    FurtherInfo = "heating controls",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "You have a central heating system with radiators and you do not already have a " +
                            "full set of controls. "
                        }
                    }
                },
                new Recommendation
                {
                    Key = RecommendationKey.FitNewWindows,
                    Title = "Fit new windows",
                    MinInstallCost = 3000,
                    MaxInstallCost = 5000,
                    Saving = 175,
                    Disruption = "Moderate",
                    InstallationTime = "1 or 2 days",
                    Description = new List<string>{
                        "It looks like you have some single glazed windows in your home. Double or triple glazed " +
                        "windows are much better at keeping the heat in, so would help you keep warm while " +
                        "reducing your heating bills.",
                        "The fuel bill savings from replacing windows are unlikely to pay back the full installation " +
                        "cost, but there are other benefits such as reduced maintenance and better sound proofing " +
                        "that may affect your decision. "
                    },
                    Caution = 
                        "If you live in a conservation area or listed building there may be limits on what " +
                        "changes you can make to your windows. You should check with your local authority to " +
                        "see what is allowed.",
                    FurtherInfo = "efficient windows",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "You have any single glazed windows."
                        }
                    }
                },
                new Recommendation
                {
                    Key = RecommendationKey.InsulateCavityWalls,
                    Title = "Insulate your cavity walls",
                    MinInstallCost = 700,
                    MaxInstallCost = 1200,
                    Saving = 185,
                    Disruption = "Minimal",
                    InstallationTime = "1 day",
                    Description = new List<string>{
                        "From the information we have it looks like the outside walls of your home " +
                        "are cavity walls, and that the cavity has not been insulated.",
                        "Specialist contractors can inject insulation into the cavity, reducing heat " +
                        "loss significantly and helping to cut your bills."
                    },
                    Caution =
                        "Not all buildings are suitable for standard cavity wall insulation. You will " +
                        "need a specialist contractor to inspect your property to work out what insulation " +
                        "options you have.",
                    FurtherInfo = "efficient windows",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "Your property was built with uninsulated cavity walls - this was common between the " +
                            "1920s and the 1980s.",
                            "The cavity has not been insulated since."
                        }
                    }
                },
                new Recommendation
                {
                    Key = RecommendationKey.SolarElectricPanels,
                    Title = "Fit solar electric panels",
                    MinInstallCost = 3500,
                    MaxInstallCost = 5500,
                    Saving = 220,
                    Disruption = "Moderate",
                    InstallationTime = "2 days",
                    Description = new List<string>{
                        "If you have a suitable roof area you may be able to fit solar photovoltaic panels, " +
                        "or solar PV, to generate electricity.",
                        "You will be able to use some of this electricity to reduce the amount you buy from " +
                        "your energy supplier, cutting your bills. At times you will generate more than you " +
                        "are using – you can sell any surplus to your electricity supplier to save even more " +
                        "money.  "
                    },
                    Caution =
                        "The savings you get from a solar PV system will depend on many factors, including " +
                        "whether there are people at home during the day using electricity.",
                    FurtherInfo = "solar PV",
                    Suitability = new RecommendationSuitability
                    {
                        IntroText = "You should consider this improvement for your property if:",
                        SuitabilityPoints = new List<string>
                        {
                            "You have a roof area that faces somewhere south of due east or due west.",
                            "This roof area is not often shaded by trees or other buildings."
                        }
                    }
                }
            };

        public static List<Recommendation> GetRecommendations()
        {
            return Recommendations;
        }

        public static Recommendation GetRecommendation(int id)
        {
            return Recommendations.First(r => (int)r.Key == id);
        }

        public static List<RecommendationKey> GetUserRecommendations(UserDataModel answers)
        {
            var userRecommendationKeys = new List<RecommendationKey>
            {

                // Always shown
                RecommendationKey.UpgradeHeatingControls,

                // Glazing is single or both
                RecommendationKey.FitNewWindows,

                // User has uninsulated cavity walls OR don't know and property 1930-1995
                RecommendationKey.InsulateCavityWalls,

                // Not for ground floor or mid floor flat, or if loft is insulated/ flat roof
                RecommendationKey.AddLoftInsulation,

                // Not for mid or top floor flat
                RecommendationKey.GroundFloorInsulation
            };

            return userRecommendationKeys;
        }
    }
}