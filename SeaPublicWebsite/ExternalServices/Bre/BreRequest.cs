namespace SeaPublicWebsite.ExternalServices.Models
{
    public class BreRequest
    {
        public string construction_date { get; set; }

        public int num_storeys { get; set; }

        public string heating_fuel { get; set; }

        public string property_type { get; set; }

        public string built_form { get; set; }

        public int num_bedrooms { get; set; }

        public string? flat_level { get; set; }

        public string? postcode { get; set; }

        public bool? hot_water_cylinder { get; set; }

        public decimal? living_room_temperature { get; set; }

        public int? heating_pattern_type { get; set; }

        public int? occupants { get; set; }

        public bool? measures { get; set; }

        public string[]? measures_package { get; set; }

        public int? roof_type { get; set; }

        public int? wall_type { get; set; }

        public int? glazing_type { get; set; }
    }
}