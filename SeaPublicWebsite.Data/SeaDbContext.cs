using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeaPublicWebsite.BusinessLogic.Models;

namespace SeaPublicWebsite.Data;

public class SeaDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<PropertyData> PropertyData { get; set; }
    public DbSet<Epc> Epc { get; set; }
    public DbSet<PropertyRecommendation> PropertyRecommendations { get; set; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    
    public SeaDbContext(DbContextOptions<SeaDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PropertyData>()
            .HasIndex(p => p.Reference)
            .IsUnique();
        
        modelBuilder.Entity<Epc>()
            .Property<int>("PropertyDataId");
        
        modelBuilder.Entity<Epc>()
            .HasOne<PropertyData>()
            .WithOne(d => d.Epc)
            .HasForeignKey<Epc>("PropertyDataId")
            .IsRequired();
    }
}
