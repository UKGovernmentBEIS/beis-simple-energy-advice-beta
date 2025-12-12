using System.ComponentModel.DataAnnotations.Schema;

namespace SeaPublicWebsite.BusinessLogic.Models;

public enum EmergencyMaintenanceState
{
    Enabled,
    Disabled
}

public class EmergencyMaintenanceHistory : IEntityWithRowVersioning
{
    public uint Version { get; set; }

    public int Id { get; set; }

    public EmergencyMaintenanceState State { get; set; }

    public DateTime ChangeDate { get; set; }

    public string AuthorEmail { get; set; }

    public EmergencyMaintenanceHistory()
    {
    }
}
