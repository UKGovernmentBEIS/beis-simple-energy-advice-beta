using System.ComponentModel.DataAnnotations;
using GovUkDesignSystem.Attributes;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum RecommendationAction
    {
        [Display(ResourceType = typeof(Resources.Enum.RecommendationAction), Description = nameof(Resources.Enum.RecommendationAction.SaveToActionPlan))]
        SaveToActionPlan,
        [Display(ResourceType = typeof(Resources.Enum.RecommendationAction), Description = nameof(Resources.Enum.RecommendationAction.DecideLater))]
        DecideLater,
        [Display(ResourceType = typeof(Resources.Enum.RecommendationAction), Description = nameof(Resources.Enum.RecommendationAction.Discard))]
        Discard,
    }
}
