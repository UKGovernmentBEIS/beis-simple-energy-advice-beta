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
                            InspectionDate = r.InspectionDate
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
            if (houseNumberA != null && houseNumberB != null) {
                if (a.GetHouseNumber() < b.GetHouseNumber()) { return -1; }
                if (a.GetHouseNumber() > b.GetHouseNumber()) { return 1; }
                return SortEpcsAlphabetically(a, b);
            }
            if (houseNumberA != null) { return -1; }
            if (houseNumberB != null) { return 1; }
            return SortEpcsAlphabetically(a, b);
        }

        public static int SortEpcsAlphabetically(Epc a, Epc b)
        {
            return string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase) == 0
                ? string.Compare(a.Address1, b.Address1, StringComparison.OrdinalIgnoreCase)
                : string.Compare(a.Address2, b.Address2, StringComparison.OrdinalIgnoreCase);
        }
    }

    internal class OpenEpcResponse
    {
        public List<EpcDTO> rows { get; set; }
    }
}