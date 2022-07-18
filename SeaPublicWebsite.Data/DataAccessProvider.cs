using Microsoft.EntityFrameworkCore;
using SeaPublicWebsite.BusinessLogic.Models;

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
        SetDaysSinceEpoch(propertyData);
        context.PropertyData.Add(propertyData);
        await context.SaveChangesAsync();
    }

    public async Task UpdatePropertyDataAsync(PropertyData propertyData)
    {
        SetDaysSinceEpoch(propertyData);
        context.PropertyData.Update(propertyData);
        await context.SaveChangesAsync();
    }

    public async Task<PropertyData> GetPropertyDataAsync(string reference)
    {
        return await context.PropertyData
            .Include(p => p.Epc)
            .Include(p => p.PropertyRecommendations)
            .Include(p => p.UneditedData)
            .SingleOrDefaultAsync(p => p.Reference == reference);
    }

    public async Task<bool> PropertyDataExistsAsync(string reference)
    {
        return await context.PropertyData.AnyAsync(p => p.Reference == reference);
    }

    public void DeleteOldPropertyData()
    {
        var daysSinceEpochSixMonthsAgo = DateTime.UtcNow.AddMonths(-6).Subtract(DateTime.UnixEpoch).Days;
        var entities = context.PropertyData.Where(p => p.DaysSinceEpochWhenUpdated <= daysSinceEpochSixMonthsAgo);
        context.RemoveRange(entities);
        context.SaveChanges();
    }

    public PropertyData SetDaysSinceEpoch(PropertyData propertyData)
    {
        propertyData.DaysSinceEpochWhenUpdated = DateTime.UtcNow.Subtract(DateTime.UnixEpoch).Days;
        return propertyData;
    }
}