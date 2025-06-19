using System.Linq;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.DataStores;

namespace SeaPublicWebsite.Services.EnergyEfficiency;

public class PropertyDataService(
    IPropertyDataStore propertyDataStore,
    IRecommendationService recommendationService)
{
    public async Task<PropertyData> UpdatePropertyDataWithRecommendations(string reference)
    {
        var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
        if (propertyData.PropertyRecommendations is null || propertyData.PropertyRecommendations.Count == 0)
        {
            var recommendationsWithPriceCap = await recommendationService.GetRecommendationsWithPriceCapForPropertyAsync(propertyData);

            propertyData.PropertyRecommendations = recommendationsWithPriceCap.Recommendations.Select(r =>
                new PropertyRecommendation
                {
                    Key = r.Key,
                    Title = r.Title,
                    MinInstallCost = r.MinInstallCost,
                    MaxInstallCost = r.MaxInstallCost,
                    Saving = r.Saving,
                    LifetimeSaving = r.LifetimeSaving,
                    Lifetime = r.Lifetime,
                    Summary = r.Summary
                }
            ).ToList();

            propertyData.EnergyPriceCapInfoRequested = true;

            if (recommendationsWithPriceCap.EnergyPriceCapInfo is not null)
            {
                propertyData.EnergyPriceCapYear = recommendationsWithPriceCap.EnergyPriceCapInfo.Year;
                propertyData.EnergyPriceCapMonthIndex = recommendationsWithPriceCap.EnergyPriceCapInfo.MonthIndex;
            }
            else
            {
                propertyData.EnergyPriceCapYear = null;
                propertyData.EnergyPriceCapMonthIndex = null;
            }
        }

        propertyData.HasSeenRecommendations = true;
        await propertyDataStore.SavePropertyDataAsync(propertyData);
        return propertyData;
    }
}