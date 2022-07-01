using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.ExternalServices.Models.Epb;

public class EpbEpcDto
{
    [JsonProperty(PropertyName = "data")]
    public EpbEpcDataDto Data { get; set; }
}

public class EpbEpcDataDto
{
    [JsonProperty(PropertyName = "assessment")]
    public EpbEpcAssessmentDto Assessment { get; set; }
}

public class EpbEpcAssessmentDto
{
    [JsonProperty(PropertyName = "typeOfAssessment")]
    public string AssessmentType { get; set; }
    
    [JsonProperty(PropertyName = "address")]
    public EpbAddressDto Address { get; set; }
    
    [JsonProperty(PropertyName = "lodgementDate")]
    public string LodgementDate { get; set; }
    
    [JsonProperty(PropertyName = "isLatestAssessmentForAddress")]
    public bool IsLatestAssessmentForAddress { get; set; }
    
    [JsonProperty(PropertyName = "propertyType")]
    public string PropertyType { get; set; }
    
    [JsonProperty(PropertyName = "builtForm")]
    public string BuiltForm { get; set; }
    
    [JsonProperty(PropertyName = "PropertyAgeBand")]
    public string PropertyAgeBand { get; set; }

    [JsonProperty(PropertyName = "wallsDescription")]
    public List<string> WallsDescription { get; set; }
    
    [JsonProperty(PropertyName = "floorDescription")]
    public List<string> FloorDescription { get; set; }
    
    [JsonProperty(PropertyName = "roofDescription")]
    public List<string> RoofDescription { get; set; }
    
    [JsonProperty(PropertyName = "windowsDescription")]
    public List<string> WindowsDescription { get; set; }
    
    [JsonProperty(PropertyName = "mainHeatingDescription")]
    public string MainHeatingDescription { get; set; }
    
    [JsonProperty(PropertyName = "mainFuelType")]
    public string MainFuelType { get; set; }
    
    [JsonProperty(PropertyName = "hasHotWaterCylinder")]
    public bool? HasHotWaterCylinder { get; set; }

    public Epc Parse()
    {
        if (AssessmentType.Equals("SAP", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }
            
        return new Epc
        {
            LodgementYear = ParseLodgementDate()?.Year,
            ConstructionAgeBand = ParseConstructionAgeBand(),
            PropertyType = ParsePropertyType(),
            HouseType = ParseHouseType(),
            BungalowType = ParseBungalowType(),
            FlatType = ParseFlatType(),
            WallConstruction = ParseWallConstruction(),
            SolidWallsInsulated = ParseSolidWallsInsulated(),
            CavityWallsInsulated = ParseCavityWallsInsulated(),
            FloorConstruction = ParseFloorConstruction(),
            FloorInsulated = ParseFloorInsulation(),
            RoofConstruction = ParseRoofConstruction(),
            RoofInsulated = ParseRoofInsulation(),
            GlazingType = ParseGlazingType(),
            HeatingType = ParseHeatingType(),
            OtherHeatingType = ParseOtherHeatingType(),
            HasHotWaterCylinder = ParseHasHotWaterCylinder()
        };
    }

    private DateTime? ParseLodgementDate()
    {
        if (LodgementDate is null)
        {
            return null;
        }
        
        var date = DateTime.Parse(LodgementDate);
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
    
    private HomeAge? ParseConstructionAgeBand()
    {
        if (PropertyAgeBand is null)
        {
            return null;
        }

        if (PropertyAgeBand.Equals("A", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("before 1900"))
        {
            return HomeAge.Pre1900;
        }
        
        if (PropertyAgeBand.Equals("B", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1900-1929"))
        {
            return HomeAge.From1900To1929;
        }
        
        if (PropertyAgeBand.Equals("C", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1930-1949"))
        {
            return HomeAge.From1930To1949;
        }
        
        if (PropertyAgeBand.Equals("D", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1950-1966"))
        {
            return HomeAge.From1950To1966;
        }
        
        if (PropertyAgeBand.Equals("E", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1967-1975"))
        {
            return HomeAge.From1967To1975;
        }
        
        if (PropertyAgeBand.Equals("F", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1976-1982"))
        {
            return HomeAge.From1976To1982;
        }
        
        if (PropertyAgeBand.Equals("G", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1983-1990"))
        {
            return HomeAge.From1983To1990;
        }
        
        if (PropertyAgeBand.Equals("H", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1991-1995"))
        {
            return HomeAge.From1991To1995;
        }
        
        if (PropertyAgeBand.Equals("I", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("1996-2002"))
        {
            return HomeAge.From1996To2002;
        }
        
        if (PropertyAgeBand.Equals("J", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("2003-2006"))
        {
            return HomeAge.From2003To2006;
        }
        
        if (PropertyAgeBand.Equals("K", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("2007-2011"))
        {
            return HomeAge.From2007To2011;
        }
        
        if (PropertyAgeBand.Equals("L", StringComparison.OrdinalIgnoreCase) ||
            PropertyAgeBand.Contains("2012 onwards"))
        {
            return HomeAge.From2012ToPresent;
        }

        return null;
    }

    private PropertyType? ParsePropertyType()
    {
        if (PropertyType is null)
        {
            return null;
        }
        
        if (PropertyType.Contains("House", StringComparison.OrdinalIgnoreCase))
        {
            return BusinessLogic.Models.Enums.PropertyType.House;
        }

        if (PropertyType.Contains("Bungalow", StringComparison.OrdinalIgnoreCase))
        {
            return BusinessLogic.Models.Enums.PropertyType.Bungalow;
        }

        if (PropertyType.Contains("Flat", StringComparison.OrdinalIgnoreCase) ||
            PropertyType.Contains("Maisonette", StringComparison.OrdinalIgnoreCase))
        {
            return BusinessLogic.Models.Enums.PropertyType.ApartmentFlatOrMaisonette;
        }

        return null;
    }
    
    private HouseType? ParseHouseType()
    {
        if (ParsePropertyType() is not BusinessLogic.Models.Enums.PropertyType.House)
        {
            return null;
        }
        
        if (PropertyType.Contains("detached", StringComparison.OrdinalIgnoreCase))
        {
            return HouseType.Detached;
        }

        if (PropertyType.Contains("semi-detached", StringComparison.OrdinalIgnoreCase))
        {
            return HouseType.SemiDetached;
        }

        if (PropertyType.Contains("mid-terrace", StringComparison.OrdinalIgnoreCase))
        {
            return HouseType.Terraced;
        }
        
        if (PropertyType.Contains("end-terrace", StringComparison.OrdinalIgnoreCase))
        {
            return HouseType.EndTerrace;
        }

        return null;
    }
    
    private BungalowType? ParseBungalowType()
    {
        if (ParsePropertyType() is not BusinessLogic.Models.Enums.PropertyType.Bungalow)
        {
            return null;
        }
        
        if (PropertyType.Contains("detached", StringComparison.OrdinalIgnoreCase))
        {
            return BungalowType.Detached;
        }

        if (PropertyType.Contains("semi-detached", StringComparison.OrdinalIgnoreCase))
        {
            return BungalowType.SemiDetached;
        }

        if (PropertyType.Contains("mid-terrace", StringComparison.OrdinalIgnoreCase))
        {
            return BungalowType.Terraced;
        }
        
        if (PropertyType.Contains("end-terrace", StringComparison.OrdinalIgnoreCase))
        {
            return BungalowType.EndTerrace;
        }

        return null;
    }
    
    private FlatType? ParseFlatType()
    {
        if (ParsePropertyType() is not BusinessLogic.Models.Enums.PropertyType.ApartmentFlatOrMaisonette)
        {
            return null;
        }
        
        if (PropertyType.Contains("basement", StringComparison.OrdinalIgnoreCase) ||
            PropertyType.Contains("ground", StringComparison.OrdinalIgnoreCase))
        {
            return FlatType.GroundFloor;
        }

        if (PropertyType.Contains("mid", StringComparison.OrdinalIgnoreCase))
        {
            return FlatType.MiddleFloor;
        }
        
        if (PropertyType.Contains("top", StringComparison.OrdinalIgnoreCase))
        {
            return FlatType.TopFloor;
        }

        return null;
    }

    private WallConstruction? ParseWallConstruction()
    {
        if (WallsDescription is null)
        {
            return null;
        }
        
        var hasCavity = WallsDescription.Any(description => 
            description.Contains("cavity", StringComparison.OrdinalIgnoreCase));
        var hasSolid = WallsDescription.Any(description => 
            description.Contains("solid", StringComparison.OrdinalIgnoreCase));

        return (hasCavity, hasSolid) switch
        {
            (true, true) => WallConstruction.Mixed,
            (true, false) => WallConstruction.Cavity,
            (false, true) => WallConstruction.Solid,
            (false, false) => null
        };
    }
    
    private SolidWallsInsulated? ParseSolidWallsInsulated()
    {
        if (WallsDescription is null)
        {
            return null;
        }

        if (WallsDescription.Any(description =>
                description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                (description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                 description.Contains("internal insulation", StringComparison.OrdinalIgnoreCase) ||
                 description.Contains("external insulation", StringComparison.OrdinalIgnoreCase))))
        {
            return SolidWallsInsulated.All;
        }
        
        if (WallsDescription.Any(description =>
                description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                description.Contains("partial insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return SolidWallsInsulated.Some;
        }
        
        if (WallsDescription.Any(description =>
                description.Contains("solid", StringComparison.OrdinalIgnoreCase) &&
                description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return SolidWallsInsulated.No;
        }

        return null;
    }

    private CavityWallsInsulated? ParseCavityWallsInsulated()
    {
        if (WallsDescription is null)
        {
            return null;
        }

        if (WallsDescription.Any(description =>
                description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                (description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                 description.Contains("internal insulation", StringComparison.OrdinalIgnoreCase) ||
                 description.Contains("external insulation", StringComparison.OrdinalIgnoreCase) ||
                 description.Contains("filled cavity", StringComparison.OrdinalIgnoreCase))))
        {
            return CavityWallsInsulated.All;
        }
        
        if (WallsDescription.Any(description =>
                description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                description.Contains("partial insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return CavityWallsInsulated.Some;
        }
        
        if (WallsDescription.Any(description =>
                description.Contains("cavity", StringComparison.OrdinalIgnoreCase) &&
                description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return CavityWallsInsulated.No;
        }

        return null;
    }

    private FloorConstruction? ParseFloorConstruction()
    {
        if (FloorDescription is null)
        {
            return null;
        }

        var hasSolid = FloorDescription.Any(description =>
            description.Contains("solid", StringComparison.OrdinalIgnoreCase));
        var hasSuspended = FloorDescription.Any(description =>
            description.Contains("suspended", StringComparison.OrdinalIgnoreCase));

        return (hasSolid, hasSuspended) switch
        {
            (true, true) => FloorConstruction.Mix,
            (true, false) => FloorConstruction.SolidConcrete,
            (false, true) => FloorConstruction.SuspendedTimber,
            (false, false) => null
        };
    }

    private FloorInsulated? ParseFloorInsulation()
    {
        if (FloorDescription is null)
        {
            return null;
        }

        if (FloorDescription.All(description =>
                description.Contains("insulated", StringComparison.OrdinalIgnoreCase)))
        {
            return FloorInsulated.Yes;
        }
        
        if (FloorDescription.Any(description =>
                description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return FloorInsulated.No;
        }

        return null;
    }

    private RoofConstruction? ParseRoofConstruction()
    {
        if (RoofDescription is null)
        {
            return null;
        }

        var hasFlat = RoofDescription.Any(description =>
            description.Contains("flat", StringComparison.OrdinalIgnoreCase));
        var hasPitched = RoofDescription.Any(description =>
            description.Contains("pitched", StringComparison.OrdinalIgnoreCase));

        return (hasFlat, hasPitched) switch
        {
            (true, true) => RoofConstruction.Mixed,
            (true, false) => RoofConstruction.Flat,
            (false, true) => RoofConstruction.Pitched,
            (false, false) => null
        };
    }

    private RoofInsulated? ParseRoofInsulation()
    {
        if (RoofDescription is null)
        {
            return null;
        }

        if (RoofDescription.All(description =>
                description.Contains("insulated", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("loft insulation", StringComparison.OrdinalIgnoreCase) ||
                description.Contains("limited insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return RoofInsulated.Yes;
        }

        if (RoofDescription.Any(description =>
                description.Contains("no insulation", StringComparison.OrdinalIgnoreCase)))
        {
            return RoofInsulated.No;
        }

        return null;
    }

    private GlazingType? ParseGlazingType()
    {
        if (WindowsDescription is null)
        {
            return null;
        }
        
        var hasSingle = WindowsDescription.Any(description =>
            description.Contains("single", StringComparison.OrdinalIgnoreCase) ||
            description.Contains("some", StringComparison.OrdinalIgnoreCase) ||
            description.Contains("partial", StringComparison.OrdinalIgnoreCase) ||
            description.Contains("mostly", StringComparison.OrdinalIgnoreCase) ||
            description.Contains("multiple glazing throughout", StringComparison.OrdinalIgnoreCase));
        
        var hasDoubleOrTriple = WindowsDescription.Any(description =>
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

    private HeatingType? ParseHeatingType()
    {
        // Gas boiler check
        // 20 - mains gas (community)
        // 26 - mains gas (not community)
        if (MainFuelType.Equals("20") ||
            MainFuelType.Equals("26") ||
            MainFuelType.Contains("mains gas", StringComparison.OrdinalIgnoreCase))
        {
            return HeatingType.GasBoiler;
        }
        
        // Oil boiler check
        // 22 - oil (community)
        // 28 - oil (not community)
        if (MainFuelType.Equals("22") ||
            MainFuelType.Equals("28") ||
            MainFuelType.Contains("oil", StringComparison.OrdinalIgnoreCase))
        {
            return HeatingType.OilBoiler;
        }
        
        // Lpg boiler check
        // 17 - LPG special condition
        // 21 - LPG (community)
        // 27 - LPG (not community)
        if (MainFuelType.Equals("17") ||
            MainFuelType.Equals("21") ||
            MainFuelType.Equals("27") ||
            MainFuelType.Contains("lpg", StringComparison.OrdinalIgnoreCase))
        {
            return HeatingType.LpgBoiler;
        }
        
        // electric heating check
        // storage heating and heat pumps do not appear in RdSAPs and are considered as electric
        // 10 - electricity
        // 25 - electricity (community)
        // 29 - electricity (not community)
        if (MainFuelType.Equals("10") ||
            MainFuelType.Equals("25") ||
            MainFuelType.Equals("29") ||
            MainFuelType.Contains("electricity", StringComparison.OrdinalIgnoreCase))
        {
            return HeatingType.DirectActionElectric;
        }
        
        return null;
    }

    private OtherHeatingType? ParseOtherHeatingType()
    {
        if (MainFuelType is null)
        {
            return null;
        }
        
        // coal check
        // 14 - house coal
        // 15 - smokeless coal
        if (MainFuelType.Equals("14") ||
            MainFuelType.Equals("15") ||
            MainFuelType.Contains("coal", StringComparison.OrdinalIgnoreCase))
        {
            return OtherHeatingType.CoalOrSolidFuel;
        }
        
        // biomass boiler check
        // 7 - bulk wood pellets
        if (MainFuelType.Equals("7") ||
            MainFuelType.Contains("biomass", StringComparison.OrdinalIgnoreCase))
        {
            return OtherHeatingType.Biomass;
        }

        return null;
    }

    private HasHotWaterCylinder? ParseHasHotWaterCylinder()
    {
        if (HasHotWaterCylinder is null)
        {
            return null;
        }

        return HasHotWaterCylinder.Value
            ? BusinessLogic.Models.Enums.HasHotWaterCylinder.Yes
            : BusinessLogic.Models.Enums.HasHotWaterCylinder.No;
    }


}
