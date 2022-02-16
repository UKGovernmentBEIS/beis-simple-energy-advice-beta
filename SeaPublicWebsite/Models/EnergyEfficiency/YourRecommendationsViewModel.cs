using GovUkDesignSystem;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.Models.EnergyEfficiency.QuestionOptions;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SeaPublicWebsite.Models.EnergyEfficiency.Recommendations;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class RecommendationViewModel : GovUkViewModel
    {
        public UserRecommendation UserRecommendation { get; set; }
        public UserDataModel UserDataModel { get; set; }
        [GovUkValidateRequired(ErrorMessageIfMissing = "Select what to do with this recommendation")]
        public RecommendationAction? RecommendationAction { get; set; }

        public int GetCurrentIndex()
        {
            return UserDataModel.UserRecommendations.FindIndex(r => r.Key == UserRecommendation.Key);
        }
        public bool HasNextIndex()
        {
            var nextIndex = GetCurrentIndex() + 1;
            return nextIndex < UserDataModel.UserRecommendations.Count;
        }

        public bool HasPreviousIndex()
        {
            var previousIndex = GetCurrentIndex() - 1;
            return previousIndex >= 0;
        }

        public RecommendationKey? NextRecommendationKey()
        {
            if (HasNextIndex())
            {
                return UserDataModel.UserRecommendations[GetCurrentIndex() + 1].Key;
            }
            return null;
        }

        public RecommendationKey? PreviousRecommendationKey()
        {
            if (HasPreviousIndex())
            {
                return UserDataModel.UserRecommendations[GetCurrentIndex() - 1].Key;
            }

            return null;
        }
    }
}