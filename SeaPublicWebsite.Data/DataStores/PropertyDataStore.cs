using SeaPublicWebsite.Helpers;

namespace SeaPublicWebsite.Data.DataStores;

public class PropertyDataStore
{
    public PropertyData LoadPropertyData(string reference)
    {
        return new PropertyData();
    }

    public bool IsReferenceValid(string reference)
    {
        return true;
    }

    public void SavePropertyData(PropertyData propertyData)
    {

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
        SavePropertyData(propertyData);

        return reference;
    }
}