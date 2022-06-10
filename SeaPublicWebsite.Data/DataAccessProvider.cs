using Microsoft.EntityFrameworkCore;

namespace SeaPublicWebsite.Data;

public class DataAccessProvider : IDataAccessProvider
{
    private readonly SeaDbContext context;

    public DataAccessProvider(SeaDbContext context)
    {
        this.context = context;
    }
    
    public void AddPropertyData(PropertyData propertyData)
    {
        context.PropertyData.Add(propertyData);
        context.SaveChanges();
    }

    public void UpdatePropertyData(PropertyData propertyData)
    {
        context.PropertyData.Update(propertyData);
        context.SaveChanges();
    }

    public async Task<PropertyData> GetSinglePropertyData(string reference)
    {
        return await context.PropertyData
            .Include(p => p.Epc)
            .Include(p => p.PropertyRecommendations)
            .FirstOrDefaultAsync(p => p.Reference == reference);
    }

    public async Task<List<PropertyData>> GetAllPropertyData()
    {
        return await context.PropertyData.ToListAsync();
    }
}