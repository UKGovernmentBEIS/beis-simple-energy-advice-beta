using SeaPublicWebsite.Services;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public abstract class QuestionFlowViewModel
    {
        public string BeforeMain { get; set; }
        public QuestionFlowPage? EntryPoint { get; set; }
        public string SkipLink { get; set; }
    }
}