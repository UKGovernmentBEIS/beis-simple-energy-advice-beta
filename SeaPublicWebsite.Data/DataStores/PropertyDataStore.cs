using SeaPublicWebsite.Data.ErrorHandling;
using SeaPublicWebsite.Data.Helpers;

namespace SeaPublicWebsite.Data.DataStores;

public class PropertyDataStore
{
    private readonly IDataAccessProvider dataAccessProvider;

    public PropertyDataStore(IDataAccessProvider dataAccessProvider)
    {
        this.dataAccessProvider = dataAccessProvider;
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

    public async Task<string> GenerateNewReferenceAndSaveEmptyPropertyDataAsync()
    {
        string reference;
        do
        {
            reference = RandomHelper.Generate8CharacterReference();
        } while (await IsReferenceValidAsync(reference));

        PropertyData propertyData = new()
        {
            Reference = reference
        };
        
        await dataAccessProvider.AddPropertyDataAsync(propertyData);

        return reference;
    }
}