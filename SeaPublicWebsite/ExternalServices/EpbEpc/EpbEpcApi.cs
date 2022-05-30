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
                InspectionDate = epc.LodgementDate, // No inspection date; Lodgement instead
                PropertyType = GetPropertyTypeFromEpc(epc),
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
            // This is not a complete mapping but there are too many options for mainHeatDescription and mainFuel to parse them all.
            // mainFuel is marked as deprecated in some places so we should try mainHeatDescription first
            if (epc.MainHeatingDescription is not null &&
                epc.MainHeatingDescription.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            
            if (epc.MainFuelType is not null && epc.MainFuelType.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.GasBoiler;
            }
            
            if (epc.MainHeatingDescription is not null &&
                epc.MainHeatingDescription.Contains("electric", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.DirectActionElectric;
            }
            
            if (epc.MainFuelType is not null && epc.MainFuelType.Contains("electric", StringComparison.OrdinalIgnoreCase))
            {
                return HeatingType.DirectActionElectric;
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

            if (epc.PropertyType.Contains("Apartment", StringComparison.OrdinalIgnoreCase) ||
                epc.PropertyType.Contains("Maisonette", StringComparison.OrdinalIgnoreCase))
            {
                return PropertyType.ApartmentFlatOrMaisonette;
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
                    description.Contains("insulated", StringComparison.OrdinalIgnoreCase)))
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
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("insulated", StringComparison.OrdinalIgnoreCase)))
            {
                return SolidWallsInsulated.All;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                    description.Contains("partial insulation", StringComparison.OrdinalIgnoreCase)))
            {
                return SolidWallsInsulated.Some;
            }
            
            if (epc.WallsDescription.Any(description =>
                    description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
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

            var hasInsulation = epc.FloorDescription.All(description =>
                description.Contains("insulated", StringComparison.OrdinalIgnoreCase));

            return hasInsulation ? FloorInsulated.Yes : FloorInsulated.No;
        }

        private static HomeAge? GetConstructionAgeBandFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.PropertyAgeBand is null)
            {
                return null;
            }
            
            // TODO: Age bands? Rewrite this based on the age band encoding
            var ageBand = epc.PropertyAgeBand.Replace("England and Wales: ", "");
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

            var hasInsulation = epc.RoofDescription.All(description =>
                description.Contains("loft insulation", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("limited insulation", StringComparison.OrdinalIgnoreCase));

            return hasInsulation ? RoofInsulated.Yes : RoofInsulated.No;
        }

        private static GlazingType? GetGlazingTypeFromEpc(EpbEpcAssessmentDto epc)
        {
            if (epc.WindowsDescription is null)
            {
                return null;
            }

            var hasSingle = epc.WindowsDescription.Any(description =>
                description.Contains("single", StringComparison.OrdinalIgnoreCase));
            var hasDoubleOrTriple = epc.WindowsDescription.Any(description =>
                description.Contains("double", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("triple", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("secondary", StringComparison.OrdinalIgnoreCase));
            
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