using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices
{
    public class EpbEpcApi: IEpcApi
    {
        private readonly IMemoryCache memoryCache;
        private readonly string epcAuthUsername;
        private readonly string epcAuthPassword;
        private readonly string cacheTokenKey = "EpbEpcToken";
        
        public EpbEpcApi(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            epcAuthUsername = Global.EpbEpcAuthUsername;
            epcAuthPassword = Global.EpbEpcAuthPassword;
        }
        
        public async Task<List<Epc>> GetEpcsForPostcode(string postcode)
        {
            var token = await RequestTokenIfNeeded();
            var response = HttpRequestHelper.SendGetRequestAsync<string>(
                new RequestParameters
                {
                    BaseAddress = Global.EpbEpcBaseAddress,
                    Path = "/api/greendeal/rhi/assessments/0000-0000-0000-0476-5172/latest",
                    Auth = new AuthenticationHeaderValue("Bearer", token)
                });
            // TODO: Once we have access to the full API, map the response to the EPC class
            return new List<Epc>();
        }

        private async Task<string> RequestTokenIfNeeded()
        {
            if (memoryCache.TryGetValue(cacheTokenKey, out string token))
            {
                return token;
            }
            var response = await HttpRequestHelper.SendPostRequestAsync<TokenRequestResponse>(
                new RequestParameters
                {
                    BaseAddress = Global.EpbEpcBaseAddress,
                    Path = "/auth/oauth/token",
                    Auth = new AuthenticationHeaderValue("Basic",
                        HttpRequestHelper.ConvertToBase64(epcAuthUsername, epcAuthPassword))
                }
            );
            // We divide by 2 to avoid edge cases of sending requests on the exact expiration time
            var expiryTimeInSeconds = response.ExpiryTimeInSeconds / 2;
            token = response.Token;
            
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(expiryTimeInSeconds));

            memoryCache.Set(cacheTokenKey, token, cacheEntryOptions);
            return token;
        }
    }

    internal class TokenRequestResponse
    {
        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }
        [JsonProperty(PropertyName = "expires_in")]
        public int ExpiryTimeInSeconds { get; set; }
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}