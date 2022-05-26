using Microsoft.EntityFrameworkCore;

namespace SeaPublicWebsite.Data;

public class SeaDbContext : DbContext
{
    public SeaDbContext(DbContextOptions<SeaDbContext> options) : base(options)
    {
        
    }
    public DbSet<PropertyData> PropertyData { get; set; }
    public DbSet<PropertyRecommendation> PropertyRecommendations { get; set; }
}
