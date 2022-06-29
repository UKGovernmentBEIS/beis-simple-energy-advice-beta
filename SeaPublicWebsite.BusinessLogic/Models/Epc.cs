using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.BusinessLogic.Models
{
    public class Epc
    {
        // PRIMARY KEY
        public int Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string EpcId { get; set; }
        public DateTime? LodgementDate { get; set; }
        public PropertyType? PropertyType { get; set; }
        public HouseType? HouseType { get; set; }
        public BungalowType? BungalowType { get; set; }
        public FlatType? FlatType { get; set; }
        public HeatingType? HeatingType { get; set; }
        public OtherHeatingType? OtherHeatingType { get; set; }
        public WallConstruction? WallConstruction { get; set; }
        public SolidWallsInsulated? SolidWallsInsulated { get; set; }
        public CavityWallsInsulated? CavityWallsInsulated { get; set; }
        public FloorConstruction? FloorConstruction { get; set; }
        public FloorInsulated? FloorInsulated { get; set; }
        public HomeAge? ConstructionAgeBand { get; set; }
        public RoofConstruction? RoofConstruction { get; set; }
        public RoofInsulated? RoofInsulated { get; set; }
        public GlazingType? GlazingType { get; set; }
        public HasHotWaterCylinder? HasHotWaterCylinder { get; set; }
    }
}

