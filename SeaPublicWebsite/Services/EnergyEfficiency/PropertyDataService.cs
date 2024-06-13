using System;
using System.Linq;
using System.Threading.Tasks;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.DataStores;

namespace SeaPublicWebsite.Services.EnergyEfficiency;

public class PropertyDataService
{
    private readonly IPropertyDataStore propertyDataStore;
    private readonly IRecommendationService recommendationService;
    
    public PropertyDataService(
        IPropertyDataStore propertyDataStore,
        IRecommendationService recommendationService)
    {
        this.propertyDataStore = propertyDataStore;
        this.recommendationService = recommendationService;
    }
    
    public async Task<PropertyData> UpdatePropertyDataWithRecommendations(string reference)
    {
        var propertyData = await propertyDataStore.LoadPropertyDataAsync(reference);
        if (propertyData.PropertyRecommendations is null || propertyData.PropertyRecommendations.Count == 0)
        {
            var recommendationsForPropertyAsync = await recommendationService.GetRecommendationsForPropertyAsync(propertyData);
            propertyData.PropertyRecommendations = recommendationsForPropertyAsync.Select(r => 
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
        }

        propertyData.RecommendationsFirstRetrievedAt ??= DateTime.Now.ToUniversalTime();
        propertyData.HasSeenRecommendations = true;
        await propertyDataStore.SavePropertyDataAsync(propertyData);
        return propertyData;
    }
}