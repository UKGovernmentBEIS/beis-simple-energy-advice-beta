using SeaPublicWebsite.Data.DataStores;

namespace SeaPublicWebsite.Data;

public interface IDataAccessProvider
{
    void AddPropertyData(PropertyData propertyData);
    void UpdatePropertyData(PropertyData propertyData);
    PropertyData GetSinglePropertyData(string reference);
    List<PropertyData> GetAllPropertyData();
}