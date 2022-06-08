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
    public PropertyData LoadPropertyData(string reference)
    {
        if (!IsReferenceValid(reference))
        {
            throw new PropertyReferenceNotFoundException
            {
                Reference = reference
            };
        }
        return dataAccessProvider.GetSinglePropertyData(reference.ToUpper());
    }

    public bool IsReferenceValid(string reference)
    {
        return dataAccessProvider.GetAllPropertyData().Exists(p => p.Reference == reference.ToUpper());
    }

    public void SavePropertyData(PropertyData propertyData)
    {
        dataAccessProvider.UpdatePropertyData(propertyData);
    }

    public string GenerateNewReferenceAndSaveEmptyPropertyData()
    {
        string reference;
        do
        {
            reference = RandomHelper.Generate8DigitReference();
        } while (IsReferenceValid(reference));

        PropertyData propertyData = new()
        {
            Reference = reference
        };
        
        dataAccessProvider.AddPropertyData(propertyData);

        return reference;
    }
}