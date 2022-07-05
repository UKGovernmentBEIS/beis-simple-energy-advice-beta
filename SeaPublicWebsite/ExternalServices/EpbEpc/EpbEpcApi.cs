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
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.ExternalServices.Models.Epb;
using SeaPublicWebsite.Helpers;

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
        
        public async Task<List<EpcSearchResult>> GetEpcsInformationForPostcodeAndBuildingNameOrNumber(string postcode, string buildingNameOrNumber = null)
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
            } catch (ApiException e)
            {
                logger.Log(LogLevel.Warning, "{Message}", e.Message);
                return null;
            }

            var epc = response.Data.Assessment;

            // We do not surface "SAP" report data since we can't convert it to our questions answers.
            if (epc.AssessmentType.Equals("SAP", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            
            return new Epc
            {
                LodgementYear = GetLodgementDateFromEpc(epc)?.Year,
                PropertyType = GetPropertyTypeFromEpc(epc),
                HouseType = GetHouseTypeFromEpc(epc),
                BungalowType = GetBungalowTypeFromEpc(epc),
                FlatType = GetFlatTypeFromEpc(epc),
                HeatingType = GetHeatingTypeFromEpc(epc),
                OtherHeatingType = GetOtherHeatingTypeFromEpc(epc),
                WallConstruction = GetWallConstructionFromEpc(epc),
                SolidWallsInsulated = GetSolidWallsInsulatedFromEpc(epc),
                CavityWallsInsulated = GetCavityWallsInsulatedFromEpc(epc),
                FloorConstruction = GetFloorConstructionFromEpc(epc),
                FloorInsulated = GetFloorInsulationFromEpc(epc),
                ConstructionAgeBand = GetConstructionAgeBandFromEpc(epc),
                RoofConstruction = GetRoofConstructionFromEpc(epc),
                RoofInsulated = GetRoofInsulationFromEpc(epc),
                GlazingType = GetGlazingTypeFromEpc(epc),
                HasHotWaterCylinder = GetHasHotWaterCylinderFromEpc(epc)
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

        private static DateTime? GetLodgementDateFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.LodgementDate is null)
            {
                return null;
            }
            
            var date = DateTime.Parse(epc.LodgementDate);
            return DateTime.SpecifyKind(date, DateTimeKind.Utc);
        }

        private static HeatingType? GetHeatingTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            // Gas boiler check
            // 20 - mains gas (community)
            // 26 - mains gas (not community)
            if (epc.MainFuelType.Equals("20") ||
                epc.MainFuelType.Equals("26") ||
                epc.MainFuelType.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            
            // Oil boiler check
            // 22 - oil (community)
            // 28 - oil (not community)
            if (epc.MainFuelType.Equals("22") ||
                epc.MainFuelType.Equals("28") ||
                epc.MainFuelType.Contains("oil", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.OilBoiler;
            }
            
            // Lpg boiler check
            // 17 - LPG special condition
            // 21 - LPG (community)
            // 27 - LPG (not community)
            if (epc.MainFuelType.Equals("17") ||
                epc.MainFuelType.Equals("21") ||
                epc.MainFuelType.Equals("27") ||
                epc.MainFuelType.Contains("lpg", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.LpgBoiler;
            }
            
            // electric heating check
            // storage heating and heat pumps do not appear in RdSAPs and are considered as electric
            // 10 - electricity
            // 25 - electricity (community)
            // 29 - electricity (not community)
            if (epc.MainFuelType.Equals("10") ||
                epc.MainFuelType.Equals("25") ||
                epc.MainFuelType.Equals("29") ||
                epc.MainFuelType.Contains("electricity", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.DirectActionElectric;
            }
            
            return null;
        }

        private static OtherHeatingType? GetOtherHeatingTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.MainFuelType is null)
            {
                return null;
            }
            
            // coal check
            // 14 - house coal
            // 15 - smokeless coal
            if (epc.MainFuelType.Equals("14") ||
                epc.MainFuelType.Equals("15") ||
                epc.MainFuelType.Contains("coal", StringComparison.OrdinalIgnoreCase))
            {
                return OtherHeatingType.CoalOrSolidFuel;
            }
            
            // biomass boiler check
            // 7 - bulk wood pellets
            if (epc.MainFuelType.Equals("7") ||
                epc.MainFuelType.Contains("biomass", StringComparison.OrdinalIgnoreCase))
            {
                return OtherHeatingType.Biomass;
            }

            return null;
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
                     description.Contains("external insulation", StringComparison.OrdinalIgnoreCase) ||
                     description.Contains("filled cavity", StringComparison.OrdinalIgnoreCase))))
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

            if (epc.PropertyAgeBand.Equals("A", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("before 1900"))
            {
                return HomeAge.Pre1900;
            }
            
            if (epc.PropertyAgeBand.Equals("B", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1900-1929"))
            {
                return HomeAge.From1900To1929;
            }
            
            if (epc.PropertyAgeBand.Equals("C", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1930-1949"))
            {
                return HomeAge.From1930To1949;
            }
            
            if (epc.PropertyAgeBand.Equals("D", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1950-1966"))
            {
                return HomeAge.From1950To1966;
            }
            
            if (epc.PropertyAgeBand.Equals("E", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1967-1975"))
            {
                return HomeAge.From1967To1975;
            }
            
            if (epc.PropertyAgeBand.Equals("F", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1976-1982"))
            {
                return HomeAge.From1976To1982;
            }
            
            if (epc.PropertyAgeBand.Equals("G", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1983-1990"))
            {
                return HomeAge.From1983To1990;
            }
            
            if (epc.PropertyAgeBand.Equals("H", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1991-1995"))
            {
                return HomeAge.From1991To1995;
            }
            
            if (epc.PropertyAgeBand.Equals("I", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("1996-2002"))
            {
                return HomeAge.From1996To2002;
            }
            
            if (epc.PropertyAgeBand.Equals("J", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("2003-2006"))
            {
                return HomeAge.From2003To2006;
            }
            
            if (epc.PropertyAgeBand.Equals("K", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("2007-2011"))
            {
                return HomeAge.From2007To2011;
            }
            
            if (epc.PropertyAgeBand.Equals("L", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyAgeBand.Contains("2012 onwards"))
            {
                return HomeAge.From2012ToPresent;
            }

            return null;
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
            
            var hasSingle = epc.WindowsDescription.Any(description =>
                description.Contains("single", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("some", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("partial", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("mostly", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("multiple glazing throughout", StringComparison.OrdinalIgnoreCase));
            
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

        private static HasHotWaterCylinder? GetHasHotWaterCylinderFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.HasHotWaterCylinder is null)
            {
                return null;
            }

            return epc.HasHotWaterCylinder.Value ? HasHotWaterCylinder.Yes : HasHotWaterCylinder.No;
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
