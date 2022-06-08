using Microsoft.EntityFrameworkCore;
using SeaPublicWebsite.Data.EnergyEfficiency.QuestionOptions;

namespace SeaPublicWebsite.Data;

public class SeaDbContext : DbContext
{
    public SeaDbContext(DbContextOptions<SeaDbContext> options) : base(options)
    {
        
    }
    public DbSet<PropertyData> PropertyData { get; set; }
    public DbSet<Epc> Epc { get; set; }
    public DbSet<PropertyRecommendation> PropertyRecommendations { get; set; }
}
