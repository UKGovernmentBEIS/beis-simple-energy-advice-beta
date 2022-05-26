using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.ExternalServices.Models.Epb;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices.EpbEpc
{
    public class EpbEpcApi: IEpcApi
    {
        private readonly IMemoryCache memoryCache;
        private readonly EpbEpcConfiguration configuration;
        private readonly string cacheTokenKey = "EpbEpcToken";
        
        public EpbEpcApi(IOptions<EpbEpcConfiguration> options, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.configuration = options.Value;
        }
        
        public async Task<List<EpcInformation>> GetEpcsInformationForPostcode(string postcode)
        {
            var token = await RequestTokenIfNeeded();
            var response = await HttpRequestHelper.SendGetRequestAsync<EpbAssessmentsDto>(
                new RequestParameters
                {
                    BaseAddress = configuration.BaseUrl,
                    Path = $"/api/assessments/domestic-epcs/search?postcode={postcode}",
                    Auth = new AuthenticationHeaderValue("Bearer", token)
                });
            if (response is null)
            {
                return null;
            }

            var epcsInformation = response.Data.Assessments.Select(epcInfo => new EpcInformation
            {
                EpcId = epcInfo.EpcId,
                Address1 = epcInfo.Address.Address1,
                Address2 = epcInfo.Address.Address2,
                Postcode = epcInfo.Address.Postcode
            }.FixFormatting()).ToList();
            
            return EpcInformation.SortEpcsInformation(epcsInformation);
        }

        public async Task<Epc> GetEpcForId(string epcId)
        {
            var token = await RequestTokenIfNeeded();
            var response = await HttpRequestHelper.SendGetRequestAsync<EpbEpcDto>(
                new RequestParameters
                {
                    BaseAddress = configuration.BaseUrl,
                    Path = $"/api/retrofit-advice/assessments/{epcId}",
                    Auth = new AuthenticationHeaderValue("Bearer", token)
                });
            if (response is null)
            {
                return null;
            }

            // TODO: Convert dto to epc
            // TODO: Add missing fields i.e. glazing type, roof
            var epc = response.Data.Assessment;
            return new Epc
            {
                EpcId = epcId,
                Address1 = epc.Address.Address1,
                Address2 = epc.Address.Address2,
                Postcode = epc.Address.Postcode,
                BuildingReference = null, // No information about this
                InspectionDate = epc.LodgementDate, // No inspection date; Lodgement instead
                PropertyType = null,
                HeatingType = null,
                WallConstruction = null,
                SolidWallsInsulated = null,
                CavityWallsInsulated = null,
                FloorConstruction = null,
                FloorInsulated = null,
                ConstructionAgeBand = null
            };
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