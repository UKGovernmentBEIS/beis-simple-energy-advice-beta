using System;
using System.Collections.Generic;
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
        private int expiryTimeInSeconds = 30 * 60;

        public EpbEpcApi()
        {
            epcAuthUsername = Global.EpcAuthUsername;
            epcAuthPassword = Global.EpcAuthPassword;
        }
        public List<Epc> GetEpcsForPostcode(string postcode)
        {
            RequestTokenIfNeeded();
            var response = HttpRequestHelper.SendGetRequest<string>(
                new RequestParameters
                {
                    BaseAddress = "https://api.epb-staging.digital.communities.gov.uk",
                    Path = "/api/greendeal/rhi/assessments/0000-0000-0000-0476-5172/latest",
                    Auth = new AuthenticationHeaderValue("Bearer", token)
                });
            if (response is null)
            {
                throw new Exception();
            }
            Console.WriteLine("Huzaah! Here's the response");
            Console.WriteLine(response);
            return new List<Epc>();
        }

        private void RequestTokenIfNeeded()
        {
            if (token is not null && !IsTokenExpired()) return;
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