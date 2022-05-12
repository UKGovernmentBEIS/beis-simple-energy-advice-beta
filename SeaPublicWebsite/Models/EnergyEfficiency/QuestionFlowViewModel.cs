using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class QuestionFlowViewModel
    {
        public string BackLink { get; set; }
        public QuestionFlowPage? EntryPoint { get; set; }
        public string SkipLink { get; set; }
    }
}