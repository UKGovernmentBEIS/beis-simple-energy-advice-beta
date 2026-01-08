using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.BusinessLogic.Services;

public class EmergencyMaintenanceService(IDataAccessProvider dataAccessProvider)
{
    public async Task<bool> SiteIsInEmergencyMaintenance()
    {
        return await GetEmergencyMaintenanceState() == EmergencyMaintenanceState.Enabled;
    }

    public async Task<EmergencyMaintenanceState> GetEmergencyMaintenanceState()
    {
        var latestHistory = await dataAccessProvider.GetLatestEmergencyMaintenanceHistoryAsync();

        return latestHistory?.State ?? EmergencyMaintenanceState.Disabled;
    }

    public async Task SetEmergencyMaintenanceState(EmergencyMaintenanceState state, string authorEmail)
    {
        var history = new EmergencyMaintenanceHistory
        {
            State = state,
            ChangeDate = DateTime.UtcNow,
            AuthorEmail = authorEmail
        };

        await dataAccessProvider.AddEmergencyMaintenanceHistoryAsync(history);
    }
}