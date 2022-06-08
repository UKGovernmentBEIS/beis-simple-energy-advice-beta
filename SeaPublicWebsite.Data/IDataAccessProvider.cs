using SeaPublicWebsite.Data.DataStores;

namespace SeaPublicWebsite.Data;

public interface IDataAccessProvider
{
    void AddPropertyData(PropertyData propertyData);
    void UpdatePropertyData(PropertyData propertyData);
    void DeletePropertyData(string reference);
    PropertyData GetSinglePropertyData(string reference);
    List<PropertyData> GetAllPropertyData();
}