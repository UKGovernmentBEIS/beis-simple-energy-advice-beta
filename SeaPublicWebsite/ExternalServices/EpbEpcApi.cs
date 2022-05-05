using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.OAuth;
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
        private int expiryTimeInSeconds = 30 * 60;

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
                var response = HttpRequestHelper.SendPostRequest<TokenRequestResponse>(
                    new RequestParameters
                    {
                        BaseAddress = "https://api.epb-staging.digital.communities.gov.uk",
                        Path = "/auth/oauth/token",
                        Auth = new AuthenticationHeaderValue("Basic",
                            HttpRequestHelper.ConvertToBase64(epcAuthUsername, epcAuthPassword))
                    }
                );
                if (response is null)
                {
                    throw new Exception();
                }
                token = response.Token;
                // We divide by 2 to avoid edge cases of sending requests on the exact expiration time
                expiryTimeInSeconds = response.ExpiryTimeInSeconds / 2;
                tokenRequestDate = DateTime.Now;
            }
        }

        private bool IsTokenExpired()
        {
            var currentDate = DateTime.Now;
            var diff = currentDate.Subtract(tokenRequestDate);
            return diff.Seconds >= expiryTimeInSeconds;
        }
    }

    internal class TokenRequestResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token;
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiryTimeInSeconds;
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType;
    }
}