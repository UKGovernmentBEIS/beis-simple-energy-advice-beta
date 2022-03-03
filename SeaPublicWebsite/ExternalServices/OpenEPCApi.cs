using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SeaPublicWebsite.ExternalServices.Models;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public class OpenEPCApi
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
                            SolidWallsInsulated = GetSolidWallsInsulatedFromEpc(r)

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


        public static HeatingType? GetHeatingTypeFromEpc(EpcDTO epc)
        {
            // This is not a complete mapping but there are too many options for mainHeatDescription and mainFuel to parse them all.
            // mainFuel is marked as deprecated in some places so we should try mainHeatDescription first
            if (epc.MainHeatDescription != null && epc.MainHeatDescription.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            else if (epc.MainFuel != null && epc.MainFuel.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            else if (epc.MainHeatDescription != null && epc.MainHeatDescription.Contains("electric", StringComparison.OrdinalIgnoreCase))
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
        public static PropertyType? GetPropertyTypeFromEpc(EpcDTO epc)
        {
            if (epc.PropertyType != null && epc.PropertyType.Contains("House", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.House;
            }
            if (epc.PropertyType != null && epc.PropertyType.Contains("Bungalow", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Bungalow;
            }
            if (epc.PropertyType != null && (epc.PropertyType.Contains("Apartment", StringComparison.OrdinalIgnoreCase) || epc.PropertyType.Contains("Maisonette", StringComparison.OrdinalIgnoreCase)))
            {
                return PropertyType.ApartmentFlatOrMaisonette;
            }
            else
            {
                return null;
            }
        }

        public static WallConstruction? GetWallConstructionFromEpc(EpcDTO epc)
        {
            if (epc.WallsDescription != null && epc.WallsDescription.Contains("cavity", StringComparison.OrdinalIgnoreCase))
            {
                return WallConstruction.Cavity;
            }
            if (epc.WallsDescription != null && epc.WallsDescription.Contains("solid", StringComparison.OrdinalIgnoreCase))
            {
                return WallConstruction.Solid;
            }
            else
            {
                return null;
            }
        }

        public static CavityWallsInsulated? GetCavityWallsInsulatedFromEpc(EpcDTO epc)
        {
            if (epc.WallsDescription != null && epc.WallsDescription.Contains("cavity", StringComparison.OrdinalIgnoreCase))
            {
                if (epc.WallsDescription.Contains("no insulation", StringComparison.OrdinalIgnoreCase))
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

        public static SolidWallsInsulated? GetSolidWallsInsulatedFromEpc(EpcDTO epc)
        {
            if (epc.WallsDescription != null && epc.WallsDescription.Contains("solid", StringComparison.OrdinalIgnoreCase))
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
    }

    internal class OpenEpcResponse
    {
        public List<EpcDTO> rows { get; set; }
    }
}