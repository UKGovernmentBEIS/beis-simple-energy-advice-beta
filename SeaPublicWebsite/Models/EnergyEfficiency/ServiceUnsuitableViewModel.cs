using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ServiceUnsuitableViewModel
    {
        public string Reference { get; set; }
        
        public Country? Country { get; set; }
        
        public string BackLink { get; set; }
    }
}