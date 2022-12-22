using Newtonsoft.Json;

namespace SeaPublicWebsite.BusinessLogic.ExternalServices.HistoricEngland;

public class HistoricEnglandDto
{
    [JsonProperty(PropertyName = "objectFieldName")]
    public string ObjectFieldName { get; set; }
    
    [JsonProperty(PropertyName = "uniqueIdField" )]
    public UniqueIdFieldDto UniqueIdField { get; set; }
    
    [JsonProperty(PropertyName = "globalFieldName" )]
    public string GlobalFieldName { get; set; }

    [JsonProperty(PropertyName = "serverGens" )]
    public ServerGensDto ServerGens { get; set; }
    
    [JsonProperty(PropertyName = "geometryType" )]
    public string GeometryType { get; set; }
    
    [JsonProperty(PropertyName = "spatialReference" )]
    public SpatialReferenceDto SpatialReference { get; set; }
    
    [JsonProperty(PropertyName = "fields" )]
    public List<FieldDto> Fields { get; set; }
    
    [JsonProperty(PropertyName = "features" )]
    public List<FeatureDto> Features { get; set; }
}

public class UniqueIdFieldDto
{
    [JsonProperty(PropertyName = "name" )]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "isSystemMaintained" )]
    public bool IsSystemMaintained { get; set; }
}

public class ServerGensDto
{
    [JsonProperty(PropertyName = "minServerGens" )]
    public uint MinServerGen { get; set; }
    
    [JsonProperty(PropertyName = "propertyName" )]
    public uint ServerGen { get; set; }
}

public class SpatialReferenceDto
{
    [JsonProperty(PropertyName = "wkid" )]
    public uint Wkid { get; set; }
    
    [JsonProperty(PropertyName = "latestWkid" )]
    public uint LatestWkid { get; set; }
}

public class FieldDto
{
    [JsonProperty(PropertyName = "name" )]
    public string Name { get; set; }

    [JsonProperty(PropertyName = "type" )]
    public string Type { get; set; }

    [JsonProperty(PropertyName = "propertyName" )]
    public string Alias { get; set; }

    [JsonProperty(PropertyName = "sqlType" )]
    public string SqlType { get; set; }

    [JsonProperty(PropertyName = "length" )]
    public uint? Length { get; set; }

    [JsonProperty(PropertyName = "domain" )]
    public dynamic Domain { get; set; }
    
    [JsonProperty(PropertyName = "defaultValue" )]
    public dynamic DefaultValue { get; set; }
    
}

public class FeatureDto
{
    [JsonProperty(PropertyName = "attributes" )]
    public AttributesDto Attributes { get; set; }
    
    [JsonProperty(PropertyName = "geometry" )]
    public GeometryDto Geometry { get; set; }
}

public class AttributesDto
{
    [JsonProperty(PropertyName = "OBJECTID" )]
    public uint ObjectId { get; set; }
    
    [JsonProperty(PropertyName = "ListEntry" )]
    public uint ListEntry { get; set; }
    
    [JsonProperty(PropertyName = "Name" )]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "Grade" )]
    public string Grade { get; set; }
    
    [JsonProperty(PropertyName = "ListDate" )]
    public long ListDate { get; set; }
    
    [JsonProperty(PropertyName = "AmendDate" )]
    public long? AmendDate { get; set; }
    
    [JsonProperty(PropertyName = "CaptureScale" )]
    public string CaptureScale { get; set; }
    
    [JsonProperty(PropertyName = "hyperlink" )]
    public string Hyperlink { get; set; }
}

public class GeometryDto
{
    [JsonProperty(PropertyName = "points" )]
    public List<List<int>> Points { get; set; }
}