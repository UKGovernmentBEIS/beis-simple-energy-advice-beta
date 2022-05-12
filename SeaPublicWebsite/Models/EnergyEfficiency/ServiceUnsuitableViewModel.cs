using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class ServiceUnsuitableViewModel : QuestionFlowViewModel
    {
        public string Reference { get; set; }
        
        public Country? Country { get; set; }
    }
}