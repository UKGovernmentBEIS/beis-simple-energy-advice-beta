namespace SeaPublicWebsite.ExternalServices.Models
{
    public class RequestEpc
    {
        public string postcode { get; set; }
        
        public bool isConnectedtoMainsGas { get; set; }
        
        public string propertyType { get; set; }
        
        public string builtForm { get; set; }
        
        public string floorLevel { get; set; }
        
        public string heatLossCorridor { get; set; }
        
        public int numberHabitableRooms { get; set; }

        public string totalFloorArea { get; set; }

        public string wallsDescription { get; set; }

        public string roofDescription { get; set; }

        public string multiGlazeProportion { get; set; }

        public string glazedArea { get; set; }

        public string mainheatDescription { get; set; }

        public string mainheatcontDescription { get; set; }

        public string numberOpenFireplaces { get; set; }

        public string lowEnergyLighting { get; set; }

        public string solarWaterHeatingFlag { get; set; }
        
        public string photoSupply { get; set; }
        
        public string windTurbineCount { get; set; }
    }
}