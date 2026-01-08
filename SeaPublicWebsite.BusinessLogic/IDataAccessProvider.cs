using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.BusinessLogic;

public interface IDataAccessProvider
{
    Task AddPropertyDataAsync(PropertyData propertyData);
    Task UpdatePropertyDataAsync(PropertyData propertyData);
    Task<PropertyData> GetPropertyDataAsync(string reference);
    Task<bool> PropertyDataExistsAsync(string reference);
    Task<EmergencyMaintenanceHistory> GetLatestEmergencyMaintenanceHistoryAsync();
    Task AddEmergencyMaintenanceHistoryAsync(EmergencyMaintenanceHistory history);
}