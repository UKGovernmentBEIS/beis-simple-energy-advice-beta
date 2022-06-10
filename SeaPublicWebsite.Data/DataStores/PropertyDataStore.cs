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
    public async Task<PropertyData> LoadPropertyData(string reference)
    {
        if (!await IsReferenceValid(reference))
        {
            throw new PropertyReferenceNotFoundException
            {
                Reference = reference
            };
        }
        return await dataAccessProvider.GetSinglePropertyData(reference.ToUpper());
    }

    public async Task<bool> IsReferenceValid(string reference)
    {
        var allPropertyData = await dataAccessProvider.GetAllPropertyData();
        return allPropertyData.Any(p => p.Reference == reference.ToUpper());
    }

    public void SavePropertyData(PropertyData propertyData)
    {
        dataAccessProvider.UpdatePropertyData(propertyData);
    }

    public async Task<string> GenerateNewReferenceAndSaveEmptyPropertyData()
    {
        string reference;
        do
        {
            reference = RandomHelper.Generate8CharacterReference();
        } while (await IsReferenceValid(reference));

        PropertyData propertyData = new()
        {
            Reference = reference
        };
        
        dataAccessProvider.AddPropertyData(propertyData);

        return reference;
    }
}