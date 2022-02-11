using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SeaPublicWebsite.Models.EnergyEfficiency;

namespace SeaPublicWebsite.ExternalServices
{
    public class GetAddressApi
    {
        public static Address getAddress(string postcode, string house)
        {
            if (string.IsNullOrWhiteSpace(postcode))
            {
                return null;
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://api.getaddress.io");
                    postcode = Regex.Replace(postcode, @"\s+", "");
                    string path = $"/find/{postcode}/{house}?expand=true&api-key=B6ngPOQpn0axO_ltgfK6ow34252";

                    HttpResponseMessage response = httpClient.GetAsync(path).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string bodyString = response.Content.ReadAsStringAsync().Result;
                        var body = JsonConvert.DeserializeObject<FindAddressResponse>(bodyString);
                        if (body.addresses.Count > 0)
                        {
                            var address = body.addresses[0];
                            return new Address
                            {
                                line_1 = address.line_1,
                                line_2 = address.line_2,
                                county = address.county,
                                town_or_city = address.town_or_city,
                                postcode = body.postcode,
                            };
                        }
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
    
    internal class FindAddressResponse
    {
        public string postcode { get; set; }
     
        public List<FindAddressDTO> addresses { get; set; }

    }

    internal class FindAddressDTO
    {
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string town_or_city { get; set; }
        public string county { get; set; }
    }
}