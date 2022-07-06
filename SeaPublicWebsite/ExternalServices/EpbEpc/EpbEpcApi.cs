﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.Models.Epb;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.ExternalServices.EpbEpc
{
    public class EpbEpcApi : IEpcApi
    {
        private readonly IMemoryCache memoryCache;
        private readonly EpbEpcConfiguration configuration;
        private readonly ILogger<EpbEpcApi> logger;
        private readonly string cacheTokenKey = "EpbEpcToken";

        public EpbEpcApi(IOptions<EpbEpcConfiguration> options, IMemoryCache memoryCache, ILogger<EpbEpcApi> logger)
        {
            this.memoryCache = memoryCache;
            this.configuration = options.Value;
            this.logger = logger;
        }

        public async Task<List<EpcSearchResult>> GetEpcsInformationForPostcodeAndBuildingNameOrNumber(string postcode,
            string buildingNameOrNumber = null)
        {
            var token = await RequestTokenIfNeeded();
            var query = $"postcode={postcode}";
            if (buildingNameOrNumber is not null)
            {
                query += $"&buildingNameOrNumber={buildingNameOrNumber}";
            }

            EpbAssessmentsDto response = null;
            try
            {
                response = await HttpRequestHelper.SendGetRequestAsync<EpbAssessmentsDto>(
                    new RequestParameters
                    {
                        BaseAddress = configuration.BaseUrl,
                        Path = $"/api/assessments/domestic-epcs/search?{query}",
                        Auth = new AuthenticationHeaderValue("Bearer", token)
                    });
            }
            catch (ApiException e)
            {
                if (e.StatusCode is not HttpStatusCode.NotFound)
                {
                    logger.Log(LogLevel.Warning, "{Message}", e.Message);
                }

                return new List<EpcSearchResult>();
            }

            var epcsInformation = response.Data.Assessments.Select(epcInfo => new EpcSearchResult(
                epcInfo.EpcId,
                epcInfo.Address.Address1,
                epcInfo.Address.Address2,
                epcInfo.Address.Postcode)).ToList();

            return EpcSearchResult.SortEpcsInformation(epcsInformation);
        }

        public async Task<Epc> GetEpcForId(string epcId)
        {
            var token = await RequestTokenIfNeeded();
            EpbEpcDto response = null;
            try
            {
                response = await HttpRequestHelper.SendGetRequestAsync<EpbEpcDto>(
                    new RequestParameters
                    {
                        BaseAddress = configuration.BaseUrl,
                        Path = $"/api/retrofit-advice/assessments/{epcId}",
                        Auth = new AuthenticationHeaderValue("Bearer", token)
                    });
            }
            catch (ApiException e)
            {
                logger.Log(LogLevel.Warning, "{Message}", e.Message);
                return null;
            }

            var epc = response.Data.Assessment;

            return epc.Parse();
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
                    BaseAddress = configuration.BaseUrl,
                    Path = "/auth/oauth/token",
                    Auth = new AuthenticationHeaderValue("Basic",
                        HttpRequestHelper.ConvertToBase64(configuration.Username, configuration.Password))
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
