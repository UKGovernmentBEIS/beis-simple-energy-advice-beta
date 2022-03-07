using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public class OpenEpcApi
    {
        public static List<Epc> GetEpcsForPostcode(string postcode)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return null;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://epc.opendatacommunities.org");
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                        "Basic", Convert.ToBase64String(
                            System.Text.ASCIIEncoding.ASCII.GetBytes(
                                $"{Global.EpcAuthUsername}:{Global.EpcAuthPassword}")));
                    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                    string path = $"/api/v1/domestic/search?postcode={postcode}&size=100";

                    HttpResponseMessage response = httpClient.GetAsync(path).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string bodyString = response.Content.ReadAsStringAsync().Result;
                        var openEpcResponse = JsonConvert.DeserializeObject<OpenEpcResponse>(bodyString);

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

                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<Epc> FixFormatting(List<Epc> epcs)
        {
            epcs.ForEach(e =>
            {
                var tI = new CultureInfo("en-GB", false).TextInfo;
                e.Address1 = tI.ToTitleCase(e.Address1.ToLower().Replace(",", ""));
                e.Address2 = tI.ToTitleCase(e.Address2.ToLower().Replace(",", ""));
            });

            return epcs;
        }

        public static List<Epc> RemoveDuplicates(List<Epc> epcs)
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

        public static int SortEpcsByHouseNumberOrAlphabetically(Epc a, Epc b)
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

        public static int SortEpcsAlphabetically(Epc a, Epc b)
        {
            return string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase) == 0
                ? string.Compare(a.Address1, b.Address1, StringComparison.OrdinalIgnoreCase)
                : string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase);
        }


        public static HeatingType? GetHeatingTypeFromEpc(EpcDto epc)
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

        public static PropertyType? GetPropertyTypeFromEpc(EpcDto epc)
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

        public static WallConstruction? GetWallConstructionFromEpc(EpcDto epc)
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

        public static CavityWallsInsulated? GetCavityWallsInsulatedFromEpc(EpcDto epc)
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

        public static SolidWallsInsulated? GetSolidWallsInsulatedFromEpc(EpcDto epc)
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

        public static FloorConstruction? GetFloorConstructionFromEpc(EpcDto epc)
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

        public static FloorInsulated? GetFloorInsulationFromEpc(EpcDto epc)
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

        private static HomeAge? GetConstructionAgeBandFromEpc(EpcDto epc)
        {
            if (epc.ConstructionAgeBand != null)
            {
                var ageBand = epc.ConstructionAgeBand.Replace("England and Wales: ", "");
                switch (ageBand)
                {
                    case ("before 1900"):
                    {
                        return HomeAge.Pre1900;
                    }
                    case ("1900-1929"):
                    {
                        return HomeAge.From1900To1929;
                    }
                    case ("1930-1949"):
                    {
                        return HomeAge.From1900To1929;
                    }
                    case ("1950-1966"):
                    {
                        return HomeAge.From1950To1966;
                    }
                    case ("1967-1975"):
                    {
                        return HomeAge.From1967To1975;
                    }
                    case ("1976-1982"):
                    {
                        return HomeAge.From1976To1982;
                    }
                    case ("1983-1990"):
                    {
                        return HomeAge.From1983To1990;
                    }
                    case ("1991-1995"):
                    {
                        return HomeAge.From1991To1995;
                    }
                    case ("1996-2002"):
                    {
                        return HomeAge.From1996To2002;
                    }
                    case ("2003-2006"):
                    {
                        return HomeAge.From2003To2006;
                    }
                    case ("2007 onwards"):
                    {
                        return HomeAge.From2007ToPresent;
                    }
                    default:
                        return null;
                }
            }

            return null;
        }
    }

    internal class OpenEpcResponse
    {
        public List<EpcDto> rows { get; set; }
    }
}