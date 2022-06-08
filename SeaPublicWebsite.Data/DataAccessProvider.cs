﻿using Microsoft.EntityFrameworkCore;

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

    public void DeletePropertyData(string reference)
    {
        var entity = context.PropertyData.FirstOrDefault(p => p.Reference == reference);
        context.PropertyData.Remove(entity);
        context.SaveChanges();
    }

    public PropertyData GetSinglePropertyData(string reference)
    {
        return context.PropertyData.FirstOrDefault(p => p.Reference == reference);
    }

    public List<PropertyData> GetAllPropertyData()
    {
        return context.PropertyData.ToList();
    }
}