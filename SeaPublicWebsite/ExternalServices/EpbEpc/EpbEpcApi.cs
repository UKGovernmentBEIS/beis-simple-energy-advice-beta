using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.Models.Epb;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.ExternalServices.EpbEpc
{
    public class EpbEpcApi: IEpcApi
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
        
        public async Task<List<EpcInformation>> GetEpcsInformationForPostcodeAndBuildingNameOrNumber(string postcode, string buildingNameOrNumber = null)
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
            } catch (ApiException e)
            {
                if (e.StatusCode is not HttpStatusCode.NotFound)
                {
                    logger.Log(LogLevel.Warning, "{Message}", e.Message);
                }

                return null;
            }
            
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

            var epc = response.Data.Assessment;
            return new Epc
            {
                EpcId = epcId,
                Address1 = epc.Address.Address1,
                Address2 = epc.Address.Address2,
                Postcode = epc.Address.Postcode,
                LodgementDate = epc.LodgementDate, // No inspection date; Lodgement instead
                PropertyType = GetPropertyTypeFromEpc(epc),
                HouseType = GetHouseTypeFromEpc(epc),
                BungalowType = GetBungalowTypeFromEpc(epc),
                FlatType = GetFlatTypeFromEpc(epc),
                HeatingType = GetHeatingTypeFromEpc(epc),
                WallConstruction = GetWallConstructionFromEpc(epc),
                SolidWallsInsulated = GetSolidWallsInsulatedFromEpc(epc),
                CavityWallsInsulated = GetCavityWallsInsulatedFromEpc(epc),
                FloorConstruction = GetFloorConstructionFromEpc(epc),
                FloorInsulated = GetFloorInsulationFromEpc(epc),
                ConstructionAgeBand = GetConstructionAgeBandFromEpc(epc),
                RoofConstruction = GetRoofConstructionFromEpc(epc),
                RoofInsulated = GetRoofInsulationFromEpc(epc),
                GlazingType = GetGlazingTypeFromEpc(epc)
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

        private static HeatingType? GetHeatingTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            // TODO: Main heating description: What do the numbers mean? 1-5
            // TODO: Confirm these values
            return epc.MainFuelType switch
            {
                "26" => HeatingType.GasBoiler,
                "27" => HeatingType.LpgBoiler,
                "28" => HeatingType.OilBoiler,
                "29" => HeatingType.DirectActionElectric,
                _ => null
            };
        }

        private static PropertyType? GetPropertyTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.PropertyType is null)
            {
                return null;
            }
            
            if (epc.PropertyType.Contains("House", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.House;
            }

            if (epc.PropertyType.Contains("Bungalow", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.Bungalow;
            }

            if (epc.PropertyType.Contains("Flat", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyType.Contains("Maisonette", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.ApartmentFlatOrMaisonette;
            }

            return null;
        }
        
        private static HouseType? GetHouseTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (GetPropertyTypeFromEpc(epc) is not PropertyType.House)
            {
                return null;
            }
            
            if (epc.PropertyType.Contains("detached", StringComparison.OrdinalIgnoreCase))
            {
                return HouseType.Detached;
            }

            if (epc.PropertyType.Contains("semi-detached", StringComparison.OrdinalIgnoreCase))
            {
                return HouseType.SemiDetached;
            }

            if (epc.PropertyType.Contains("mid-terrace", StringComparison.OrdinalIgnoreCase))
            {
                return HouseType.Terraced;
            }
            
            if (epc.PropertyType.Contains("end-terrace", StringComparison.OrdinalIgnoreCase))
            {
                return HouseType.EndTerrace;
            }

            return null;
        }
        
        private static BungalowType? GetBungalowTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (GetPropertyTypeFromEpc(epc) is not PropertyType.Bungalow)
            {
                return null;
            }
            
            if (epc.PropertyType.Contains("detached", StringComparison.OrdinalIgnoreCase))
            {
                return BungalowType.Detached;
            }

            if (epc.PropertyType.Contains("semi-detached", StringComparison.OrdinalIgnoreCase))
            {
                return BungalowType.SemiDetached;
            }

            if (epc.PropertyType.Contains("mid-terrace", StringComparison.OrdinalIgnoreCase))
            {
                return BungalowType.Terraced;
            }
            
            if (epc.PropertyType.Contains("end-terrace", StringComparison.OrdinalIgnoreCase))
            {
                return BungalowType.EndTerrace;
            }

            return null;
        }
        
        private static FlatType? GetFlatTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (GetPropertyTypeFromEpc(epc) is not PropertyType.ApartmentFlatOrMaisonette)
            {
                return null;
            }
            
            if (epc.PropertyType.Contains("basement", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyType.Contains("ground", StringComparison.OrdinalIgnoreCase))
            {
                return FlatType.GroundFloor;
            }

            if (epc.PropertyType.Contains("mid", StringComparison.OrdinalIgnoreCase))
            {
                return FlatType.MiddleFloor;
            }
            
            if (epc.PropertyType.Contains("top", StringComparison.OrdinalIgnoreCase))
            {
                return FlatType.TopFloor;
            }

            return null;
        }

        private static WallConstruction? GetWallConstructionFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.WallsDescription is null)
            {
                return null;
            }
            
            var hasCavity = epc.WallsDescription.Any(description => 
                description.Contains("cavity", StringComparison.OrdinalIgnoreCase));
            var hasSolid = epc.WallsDescription.Any(description => 
                description.Contains("solid", StringComparison.OrdinalIgnoreCase));

            return (hasCavity, hasSolid) switch
            {
                (true, true) => WallConstruction.Mixed,
                (true, false) => WallConstruction.Cavity,
                (false, true) => WallConstruction.Solid,
                (false, false) => null
            };
        }

        private static CavityWallsInsulated? GetCavityWallsInsulatedFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.WallsDescription is null)
            {
                return null;
            }

            if (epc.WallsDescription.Any(description =>
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                    (description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                     description.Contains("internal insulation", StringComparison.OrdinalIgnoreCase) ||
                     description.Contains("external insulation", StringComparison.OrdinalIgnoreCase))))
            {
                return CavityWallsInsulated.All;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("partial insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return CavityWallsInsulated.Some;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return CavityWallsInsulated.No;
            }

            return null;
        }

        private static SolidWallsInsulated? GetSolidWallsInsulatedFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.WallsDescription is null)
            {
                return null;
            }

            if (epc.WallsDescription.Any(description =>
                    description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                    (description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                     description.Contains("internal insulation", StringComparison.OrdinalIgnoreCase) ||
                     description.Contains("external insulation", StringComparison.OrdinalIgnoreCase))))
            {
                return SolidWallsInsulated.All;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("partial insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return SolidWallsInsulated.Some;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return SolidWallsInsulated.No;
            }

            return null;
        }

        private static FloorConstruction? GetFloorConstructionFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.FloorDescription is null)
            {
                return null;
            }

            var hasSolid = epc.FloorDescription.Any(description =>
                description.Contains("solid", StringComparison.OrdinalIgnoreCase));
            var hasSuspended = epc.FloorDescription.Any(description =>
                description.Contains("suspended", StringComparison.OrdinalIgnoreCase));

            return (hasSolid, hasSuspended) switch
            {
                (true, true) => FloorConstruction.Mix,
                (true, false) => FloorConstruction.SolidConcrete,
                (false, true) => FloorConstruction.SuspendedTimber,
                (false, false) => null
            };
        }

        private static FloorInsulated? GetFloorInsulationFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.FloorDescription is null)
            {
                return null;
            }

            if (epc.FloorDescription.All(description =>
                    description.Contains("insulated", StringComparison.OrdinalIgnoreCase)))
            {
                return FloorInsulated.Yes;
            }
            
            if (epc.FloorDescription.Any(description =>
                    description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return FloorInsulated.No;
            }

            return null;
        }

        private static HomeAge? GetConstructionAgeBandFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.PropertyAgeBand is null)
            {
                return null;
            }

            return epc.PropertyAgeBand switch
            {
                "A" => HomeAge.Pre1900,
                "B" => HomeAge.From1900To1929,
                "C" => HomeAge.From1930To1949,
                "D" => HomeAge.From1950To1966,
                "E" => HomeAge.From1967To1975,
                "F" => HomeAge.From1976To1982,
                "G" => HomeAge.From1983To1990,
                "H" => HomeAge.From1991To1995,
                "I" => HomeAge.From1996To2002,
                "J" => HomeAge.From2003To2006,
                "K" or "L" => HomeAge.From2007ToPresent,
                _ => null
            };
        }

        private static RoofConstruction? GetRoofConstructionFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.RoofDescription is null)
            {
                return null;
            }

            var hasFlat = epc.RoofDescription.Any(description =>
                description.Contains("flat", StringComparison.OrdinalIgnoreCase));
            var hasPitched = epc.RoofDescription.Any(description =>
                description.Contains("pitched", StringComparison.OrdinalIgnoreCase));

            return (hasFlat, hasPitched) switch
            {
                (true, true) => RoofConstruction.Mixed,
                (true, false) => RoofConstruction.Flat,
                (false, true) => RoofConstruction.Pitched,
                (false, false) => null
            };
        }

        private static RoofInsulated? GetRoofInsulationFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.RoofDescription is null)
            {
                return null;
            }

            if (epc.RoofDescription.All(description =>
                    description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                    description.Contains("loft insulation", StringComparison.OrdinalIgnoreCase) ||
                    description.Contains("limited insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return RoofInsulated.Yes;
            }

            if (epc.RoofDescription.Any(description =>
                    description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return RoofInsulated.No;
            }

            return null;
        }

        private static GlazingType? GetGlazingTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.WindowsDescription is null)
            {
                return null;
            }

            // We consider 'mostly' as the point where the number of single-glazed windows is insignificant
            var hasSingle = epc.WindowsDescription.Any(description =>
                description.Contains("single", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("some", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("partial", StringComparison.OrdinalIgnoreCase));
            var hasDoubleOrTriple = epc.WindowsDescription.Any(description =>
                description.Contains("some", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("partial", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("mostly", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("full", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("high", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("multiple glazing throughout", StringComparison.OrdinalIgnoreCase));
            
            return (hasSingle, hasDoubleOrTriple) switch
            {
                (true, true) => GlazingType.Both,
                (true, false) => GlazingType.SingleGlazed,
                (false, true) => GlazingType.DoubleOrTripleGlazed,
                (false, false) => null
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