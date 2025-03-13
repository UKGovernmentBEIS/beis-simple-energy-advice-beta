namespace SeaPublicWebsite.BusinessLogic.Models;

public interface IEntityWithRowVersioning
{
    uint Version { get; set; }
}