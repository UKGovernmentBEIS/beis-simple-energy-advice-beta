using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SeaPublicWebsite.Data;
using SeaPublicWebsite.ErrorHandling;
using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.DataStores;

public class PropertyDataStore
{
    private readonly IDataAccessProvider dataAccessProvider;
    private readonly ILogger<PropertyDataStore> logger;
    private const int MaxRetries = 10;
    private const int SleepMilliSeconds = 500;

    public PropertyDataStore(IDataAccessProvider dataAccessProvider, ILogger<PropertyDataStore> logger)
    {
        this.dataAccessProvider = dataAccessProvider;
        this.logger = logger;
    }
    
    public async Task<PropertyData> LoadPropertyDataAsync(string reference)
    {
        var data = await dataAccessProvider.GetPropertyDataAsync(reference.ToUpper());
        
        if (data == null)
        {
            throw new PropertyReferenceNotFoundException
            {
                Reference = reference
            };
        }
        
        return data;
    }

    public async Task<bool> IsReferenceValidAsync(string reference)
    {
        return await dataAccessProvider.PropertyDataExistsAsync(reference);
    }

    public async Task SavePropertyDataAsync(PropertyData propertyData)
    {
        await dataAccessProvider.UpdatePropertyDataAsync(propertyData);
    }

    public async Task<string> CreateNewPropertyDataAsync()
    {
        var saveCount = 0;
        var attemptedReferences = new List<string>();

        while (saveCount <= MaxRetries)
        {
            PropertyData propertyData = new()
            {
                Reference = RandomHelper.Generate8CharacterReference()
            };
            attemptedReferences.Add(propertyData.Reference);
            
            try
            {
                await dataAccessProvider.AddPropertyDataAsync(propertyData);
                return propertyData.Reference;
            }
            catch (Exception)
            {
                // Just retry
                logger.LogWarning("Failed to create new property data row with reference " + propertyData.Reference);
                await Task.Delay(SleepMilliSeconds);
            }
            saveCount++;
        }

        throw new Exception("Failed to create new property data row. Tried references: " + string.Join(',', attemptedReferences));
    }
}
