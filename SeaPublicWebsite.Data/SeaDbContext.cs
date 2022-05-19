using Microsoft.EntityFrameworkCore;

namespace SeaPublicWebsite.Data;

public class SeaDbContext : DbContext
{
    public SeaDbContext(DbContextOptions<SeaDbContext> options) : base(options)
    {
        
    }
    public DbSet<User> Users { get; set; }
}
