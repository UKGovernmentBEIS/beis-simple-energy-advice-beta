using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public class EpbEpcApi: IEpcApi
    {
        private readonly string epcAuthUsername;
        private readonly string epcAuthPassword;
        
        private string token;
        private DateTime tokenRequestDate;
        public readonly int ExpiryTimeInSeconds = 30 * 60;

        public EpbEpcApi()
        {
            epcAuthUsername = Global.EpcAuthUsername;
            epcAuthPassword = Global.EpcAuthPassword;
        }
        public List<Epc> GetEpcsForPostcode(string postcode)
        {
            RequestTokenIfNeeded();
            
            var httpClient = new HttpClient();
            // TODO: Fill this
            httpClient.BaseAddress = new Uri("");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer", token);
            // TODO: Json? xml?
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            // TODO: Path?
            string path = "";
            HttpResponseMessage response = httpClient.GetAsync(path).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string bodyString = response.Content.ReadAsStringAsync().Result;
                
                // TODO: Figure the type
                var epbEpcResponse = JsonConvert.DeserializeObject<dynamic>(bodyString);
                
                // TODO: Convert response to List<Epc>
                var epcs = new List<Epc>();
                return epcs;
            }

            return null;
        }

        private void RequestTokenIfNeeded()
        {
            if (token is null || IsTokenExpired())
            {
                // TODO: Request new token
                token = null;
                tokenRequestDate = DateTime.Now;
            }
        }

        private bool IsTokenExpired()
        {
            var currentDate = DateTime.Now;
            var diff = currentDate.Subtract(tokenRequestDate);
            return diff.Seconds >= ExpiryTimeInSeconds;
        }
    }
}