using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices.OpenEpc
{
    public class OpenEpcApi : IEpcApi
    {
        private readonly OpenEpcConfiguration configuration;

        public OpenEpcApi(IOptions<OpenEpcConfiguration> options)
        {
            configuration = options.Value;
        }

        public async Task<List<Epc>> GetEpcsForPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return null;
            }

            try
            {
                var openEpcResponse = await HttpRequestHelper.SendGetRequestAsync<OpenEpcResponse>(
                    new RequestParameters
                    {
                        BaseAddress = configuration.BaseUrl,
                        Path = $"/api/v1/domestic/search?postcode={postcode}&size=100",
                        Auth = new AuthenticationHeaderValue("Basic",
                            HttpRequestHelper.ConvertToBase64(configuration.Username, configuration.Password))
                    });

                if (openEpcResponse is null)
                {
                    return null;
                }

                var epcs = openEpcResponse.rows.Select(r => new Epc()
                {
                    Address1 = r.Address1,
                    Address2 = r.Address2,
                    Postcode = r.Postcode,
                    BuildingReference = r.BuildingReference,
                    EpcId = r.LmkKey,
                    InspectionDate = r.InspectionDate,
                    HeatingType = GetHeatingTypeFromEpc(r),
                    PropertyType = GetPropertyTypeFromEpc(r),
                    WallConstruction = GetWallConstructionFromEpc(r),
                    CavityWallsInsulated = GetCavityWallsInsulatedFromEpc(r),
                    SolidWallsInsulated = GetSolidWallsInsulatedFromEpc(r),
                    FloorConstruction = GetFloorConstructionFromEpc(r),
                    FloorInsulated = GetFloorInsulationFromEpc(r),
                    ConstructionAgeBand = GetConstructionAgeBandFromEpc(r)
                }).ToList();

                epcs = FixFormatting(epcs);
                epcs = RemoveDuplicates(epcs);
                epcs.Sort(SortEpcsByHouseNumberOrAlphabetically);

                return epcs;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static List<Epc> FixFormatting(List<Epc> epcs)
        {
            epcs.ForEach(e =>
            {
                var tI = new CultureInfo("en-GB", false).TextInfo;
                e.Address1 = tI.ToTitleCase(e.Address1.ToLower().Replace(",", ""));
                e.Address2 = tI.ToTitleCase(e.Address2.ToLower().Replace(",", ""));
            });

            return epcs;
        }

        private static List<Epc> RemoveDuplicates(List<Epc> epcs)
        {
            return epcs
                .GroupBy(e => e.BuildingReference)
                .Select(grp => grp.OrderBy(i => i.InspectionDate).FirstOrDefault())
                .GroupBy(e => new
                {
                    e.Address1,
                    e.Address2,
                    e.Postcode
                })
                .Select(grp => grp.OrderBy(i => i.InspectionDate).FirstOrDefault())
                .ToList();
        }

        private static int SortEpcsByHouseNumberOrAlphabetically(Epc a, Epc b)
        {
            var houseNumberA = a.GetHouseNumber();
            var houseNumberB = b.GetHouseNumber();
            if (houseNumberA != null && houseNumberB != null)
            {
                if (a.GetHouseNumber() < b.GetHouseNumber())
                {
                    return -1;
                }

                if (a.GetHouseNumber() > b.GetHouseNumber())
                {
                    return 1;
                }

                return SortEpcsAlphabetically(a, b);
            }

            if (houseNumberA != null)
            {
                return -1;
            }

            if (houseNumberB != null)
            {
                return 1;
            }

            return SortEpcsAlphabetically(a, b);
        }

        private static int SortEpcsAlphabetically(Epc a, Epc b)
        {
            return string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase) == 0
                ? string.Compare(a.Address1, b.Address1, StringComparison.OrdinalIgnoreCase)
                : string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase);
        }


        private static HeatingType? GetHeatingTypeFromEpc(OpenEpcDto epc)
        {
            // This is not a complete mapping but there are too many options for mainHeatDescription and mainFuel to parse them all.
            // mainFuel is marked as deprecated in some places so we should try mainHeatDescription first
            if (epc.MainHeatDescription != null &&
                epc.MainHeatDescription.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            else if (epc.MainFuel != null && epc.MainFuel.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            else if (epc.MainHeatDescription != null &&
                     epc.MainHeatDescription.Contains("electric", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.DirectActionElectric;
            }
            else if (epc.MainFuel != null && epc.MainFuel.Contains("electric", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.DirectActionElectric;
            }
            else
            {
                return null;
            }
        }

        private static PropertyType? GetPropertyTypeFromEpc(OpenEpcDto epc)
        {
            if (epc.PropertyType != null && epc.PropertyType.Contains("House", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.House;
            }

            if (epc.PropertyType != null && epc.PropertyType.Contains("Bungalow", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Bungalow;
            }

            if (epc.PropertyType != null &&
                (epc.PropertyType.Contains("Apartment", StringComparison.OrdinalIgnoreCase) ||
                 epc.PropertyType.Contains("Maisonette", StringComparison.OrdinalIgnoreCase)))
            {
                return PropertyType.ApartmentFlatOrMaisonette;
            }
            else
            {
                return null;
            }
        }

        private static WallConstruction? GetWallConstructionFromEpc(OpenEpcDto epc)
        {
            if (epc.WallsDescription != null &&
                epc.WallsDescription.Contains("cavity", StringComparison.OrdinalIgnoreCase))
            {
                return WallConstruction.Cavity;
            }

            if (epc.WallsDescription != null &&
                epc.WallsDescription.Contains("solid", StringComparison.OrdinalIgnoreCase))
            {
                return WallConstruction.Solid;
            }
            else
            {
                return null;
            }
        }

        private static CavityWallsInsulated? GetCavityWallsInsulatedFromEpc(OpenEpcDto epc)
        {
            if (epc.WallsDescription != null &&
                epc.WallsDescription.Contains("cavity", StringComparison.OrdinalIgnoreCase))
            {
                if (epc.WallsDescription.Contains("no insulation", StringComparison.OrdinalIgnoreCase) 
                    || epc.WallsDescription.Contains("partial insulation", StringComparison.OrdinalIgnoreCase))
                {
                    return CavityWallsInsulated.No;
                }
                else if (epc.WallsDescription.Contains("insulated", StringComparison.OrdinalIgnoreCase))
                {
                    return CavityWallsInsulated.All;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static SolidWallsInsulated? GetSolidWallsInsulatedFromEpc(OpenEpcDto epc)
        {
            if (epc.WallsDescription != null &&
                epc.WallsDescription.Contains("solid", StringComparison.OrdinalIgnoreCase))
            {
                if (epc.WallsDescription.Contains("no insulation", StringComparison.OrdinalIgnoreCase))
                {
                    return SolidWallsInsulated.No;
                }
                else if (epc.WallsDescription.Contains("insulated", StringComparison.OrdinalIgnoreCase))
                {
                    return SolidWallsInsulated.All;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static FloorConstruction? GetFloorConstructionFromEpc(OpenEpcDto epc)
        {
            if (epc.FloorDescription != null)
            {
                if (epc.FloorDescription.Contains("solid", StringComparison.OrdinalIgnoreCase))
                {
                    return FloorConstruction.SolidConcrete;
                }

                if (epc.FloorDescription.Contains("suspended", StringComparison.OrdinalIgnoreCase))
                {
                    return FloorConstruction.SuspendedTimber;
                }
            }

            return null;
        }

        private static FloorInsulated? GetFloorInsulationFromEpc(OpenEpcDto epc)
        {
            if (epc.FloorDescription != null && GetFloorConstructionFromEpc(epc).HasValue)
            {
                if (epc.FloorDescription.Contains("no insulation", StringComparison.OrdinalIgnoreCase))
                {
                    return FloorInsulated.No;
                }

                if (epc.FloorDescription.Contains("insulated", StringComparison.OrdinalIgnoreCase))
                {
                    return FloorInsulated.Yes;
                }
            }

            return null;
        }

        private static HomeAge? GetConstructionAgeBandFromEpc(OpenEpcDto epc)
        {
            if (epc.ConstructionAgeBand == null) return null;
            var ageBand = epc.ConstructionAgeBand.Replace("England and Wales: ", "");
            return ageBand switch
            {
                ("before 1900") => HomeAge.Pre1900,
                ("1900-1929") => HomeAge.From1900To1929,
                ("1930-1949") => HomeAge.From1930To1949,
                ("1950-1966") => HomeAge.From1950To1966,
                ("1967-1975") => HomeAge.From1967To1975,
                ("1976-1982") => HomeAge.From1976To1982,
                ("1983-1990") => HomeAge.From1983To1990,
                ("1991-1995") => HomeAge.From1991To1995,
                ("1996-2002") => HomeAge.From1996To2002,
                ("2003-2006") => HomeAge.From2003To2006,
                ("2007 onwards") => HomeAge.From2007ToPresent,
                _ => null
            };

        }
    }

    internal class OpenEpcResponse
    {
        public List<OpenEpcDto> rows { get; set; }
    }
}