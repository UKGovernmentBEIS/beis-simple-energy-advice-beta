using SeaPublicWebsite.Data.DataModels;

namespace SeaPublicWebsite.Data;

public interface IDataAccessProvider
{
    Task AddPropertyDataAsync(PropertyData propertyData);
    Task UpdatePropertyDataAsync(PropertyData propertyData);
    Task<PropertyData> GetPropertyDataAsync(string reference);
    Task<bool> PropertyDataExistsAsync(string reference);
}