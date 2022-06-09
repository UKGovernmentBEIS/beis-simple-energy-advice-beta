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

    public PropertyData GetSinglePropertyData(string reference)
    {
        return context.PropertyData
            .Include(p => p.Epc)
            .Include(p => p.PropertyRecommendations)
            .FirstOrDefault(p => p.Reference == reference);
    }

    public List<PropertyData> GetAllPropertyData()
    {
        return context.PropertyData.ToList();
    }
}