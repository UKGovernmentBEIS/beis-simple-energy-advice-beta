using SeaPublicWebsite.Data.DataStores;

namespace SeaPublicWebsite.Data;

public interface IDataAccessProvider
{
    void AddPropertyData(PropertyData propertyData);
    void UpdatePropertyData(PropertyData propertyData);
    Task<PropertyData> GetSinglePropertyData(string reference);
    Task<List<PropertyData>> GetAllPropertyData();
}