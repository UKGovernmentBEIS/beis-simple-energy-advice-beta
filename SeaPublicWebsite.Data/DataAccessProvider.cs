using Microsoft.EntityFrameworkCore;

namespace SeaPublicWebsite.Data;

public class DataAccessProvider : IDataAccessProvider
{
    private readonly SeaDbContext context;

    public DataAccessProvider(SeaDbContext context)
    {
        this.context = context;
    }
    
    public async Task AddPropertyDataAsync(PropertyData propertyData)
    {
        context.PropertyData.Add(propertyData);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePropertyDataAsync(PropertyData propertyData)
    {
        context.PropertyData.Update(propertyData);
        await context.SaveChangesAsync();
    }

    public async Task<PropertyData> GetSinglePropertyDataAsync(string reference)
    {
        return await context.PropertyData
            .Include(p => p.Epc)
            .Include(p => p.PropertyRecommendations)
            .FirstOrDefaultAsync(p => p.Reference == reference);
    }

    public async Task<bool> PropertyDataExistsAsync(string reference)
    {
        return await context.PropertyData.AnyAsync(p => p.Reference == reference);
    }
}