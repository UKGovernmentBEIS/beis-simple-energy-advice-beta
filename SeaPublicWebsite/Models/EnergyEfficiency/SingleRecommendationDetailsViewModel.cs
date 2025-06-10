using System.Collections.Generic;
using GovUkDesignSystem.GovUkDesignSystemComponents;
using Microsoft.AspNetCore.Mvc.Localization;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Models.EnergyEfficiency
{
    public class SingleRecommendationDetailsViewModel
    {
        public int RecommendationIndex { get; set; }
        public List<PropertyRecommendation> PropertyRecommendations { get; set; }
        public TagViewModel DisruptionTagViewModel { get; set;  }
        public TagViewModel DurationTagViewModel { get; set;  }

        public PropertyRecommendation GetCurrentPropertyRecommendation() => PropertyRecommendations[RecommendationIndex];
        
        public EnergyPriceCapInfo EnergyPriceCapInfo { get; set; }

        public string GetEnergyPriceCapString(IHtmlLocalizer<SharedResources> sharedLocalizer)
        {
            return EnergyPriceCapInfo switch
            {
                EnergyPriceCapInfoNotParsed =>
                    sharedLocalizer["FiguresForTypicalInstallationWarningTextUnknownString"].Value,
                EnergyPriceCapInfoParsed e =>
                    string.Format(
                        sharedLocalizer["FiguresForTypicalInstallationWarningTextString"].Value,
                        sharedLocalizer[$"Month{e.MonthIndex}String"].Value + " " + e.Year),
                _ => sharedLocalizer["FiguresForTypicalInstallationWarningTextNoPriceCapString"].Value
            };
        }
    }
}
