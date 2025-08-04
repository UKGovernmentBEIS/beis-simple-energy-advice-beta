using System;
using System.Collections.Generic;
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

        var recommendationsWithPriceCap =
            await recommendationService.GetRecommendationsWithPriceCapForPropertyAsync(propertyData);

        var newRecommendations = recommendationsWithPriceCap.Recommendations.Select(r =>
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

        var recommendationsChanged = RecommendationsChanged(propertyData.PropertyRecommendations, newRecommendations);

        if (recommendationsChanged)
        {
            // if no previous recommendations remaining, then this is the users first time through the form
            // or they have changed their answers (in which case previous answers are discarded anyway).
            // if so mark this as false to show new visit wording
            propertyData.RecommendationsUpdatedSinceLastVisit = propertyData.PropertyRecommendations.Count > 0;
            
            UpdateRecommendationsWithPriorActions(propertyData.PropertyRecommendations, newRecommendations);
            propertyData.PropertyRecommendations = newRecommendations;
        }
        else
        {
            propertyData.RecommendationsUpdatedSinceLastVisit = false;
        }
        
        propertyData.RecommendationsLastRetrievedAt = DateTime.UtcNow;

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

        propertyData.HasSeenRecommendations = true;
        await propertyDataStore.SavePropertyDataAsync(propertyData);
        return propertyData;
    }

    private bool RecommendationsChanged(List<PropertyRecommendation> existingRecommendations,
        List<PropertyRecommendation> newRecommendations)
    {
        if (existingRecommendations is null || existingRecommendations.Count == 0)
        {
            return true;
        }

        if (existingRecommendations.Count != newRecommendations.Count)
        {
            return true;
        }

        return existingRecommendations.Any(er =>
            !newRecommendations.Any(nr => nr.HasSameImpactAs(er)));
    }

    private void UpdateRecommendationsWithPriorActions(List<PropertyRecommendation> existingRecommendations,
        List<PropertyRecommendation> newRecommendations)
    {
        foreach (var newRecommendation in newRecommendations)
        {
            // try to find an existing decision for this new recommendation
            var matchingExistingRecommendation = existingRecommendations
                .FirstOrDefault(er => er.HasSameImpactAs(newRecommendation));

            if (matchingExistingRecommendation != null)
            {
                newRecommendation.RecommendationAction = matchingExistingRecommendation.RecommendationAction;
            }
        }
    }
}