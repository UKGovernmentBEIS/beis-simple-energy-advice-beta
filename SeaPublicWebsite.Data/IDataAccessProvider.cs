using SeaPublicWebsite.Data.DataStores;

namespace SeaPublicWebsite.Data;

public interface IDataAccessProvider
{
    Task AddPropertyDataAsync(PropertyData propertyData);
    Task UpdatePropertyDataAsync(PropertyData propertyData);
    Task<PropertyData> GetSinglePropertyDataAsync(string reference);
    Task<List<PropertyData>> GetAllPropertyDataAsync();
}