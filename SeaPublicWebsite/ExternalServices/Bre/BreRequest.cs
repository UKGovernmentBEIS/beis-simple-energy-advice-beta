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

        public string flat_level { get; set; }

        public string postcode { get; set; }

        public bool? hot_water_cylinder { get; set; }

        public decimal? living_room_temperature { get; set; }

        public int heating_pattern_type { get; set; }

        public int[] normal_days_off_hours { get; set; }
        
        public int? occupants { get; set; }

        public bool measures { get; set; }

        public string[] measures_package { get; set; }

        public int? roof_type { get; set; }

        public int wall_type { get; set; }

        public int glazing_type { get; set; }

        public BreRequest(
            string brePostcode,
            BrePropertyType brePropertyType,
            BreBuiltForm breBuiltForm,
            BreFlatLevel? breFlatLevel,
            string breConstructionDate,
            BreWallType breWallType,
            BreRoofType? breRoofType,
            BreGlazingType breGlazingType,
            BreHeatingFuel breHeatingFuel,
            bool? breHotWaterCylinder,
            int? breOccupants,
            BreHeatingPatternType breHeatingPatternType,
            int[] breNormalDaysOffHours,
            decimal? breTemperature
        )
        {
            postcode = brePostcode;
            property_type = ((int) brePropertyType).ToString();
            built_form = ((int) breBuiltForm).ToString();
            flat_level = ((int?) breFlatLevel).ToString();
            construction_date = breConstructionDate;
            wall_type = (int) breWallType;
            //no input for floor_type in BRE API
            roof_type = (int?) breRoofType;
            glazing_type = (int) breGlazingType;
            //no input for outdoor heater space in BRE API
            heating_fuel = ((int) breHeatingFuel).ToString();
            hot_water_cylinder = breHotWaterCylinder;
            occupants = breOccupants;
            heating_pattern_type = (int) breHeatingPatternType;
            normal_days_off_hours = breNormalDaysOffHours;
            living_room_temperature = breTemperature;
            //peer-reviewed assumption:
            num_storeys = brePropertyType == BrePropertyType.House ? 2 : 1;
            //peer-reviewed assumption (question to be added for this):
            num_bedrooms = breOccupants ?? 1;
            measures = true;
            //measures_package consists of all measures implemented in the BRE API as of May 2021
            measures_package = new[]
            {
                "A", "B", "Q", "Q1", "W1", "W2", "D", "C", "F", "G", "L2", "N", "Y", "O", "O3", "X",
                "U"
            };
        }
    }
}