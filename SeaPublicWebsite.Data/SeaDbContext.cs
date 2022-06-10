using Microsoft.EntityFrameworkCore;
using SeaPublicWebsite.Data.DataModels;

namespace SeaPublicWebsite.Data;

public class SeaDbContext : DbContext
{
    public DbSet<PropertyData> PropertyData { get; set; }
    public DbSet<Epc> Epc { get; set; }
    public DbSet<PropertyRecommendation> PropertyRecommendations { get; set; }
    
    public SeaDbContext(DbContextOptions<SeaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyData>()
            .HasIndex(p => p.Reference)
            .IsUnique();
    }
}
