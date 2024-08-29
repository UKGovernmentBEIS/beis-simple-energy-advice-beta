using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.DataStores;
using Moq;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Data;

namespace Tests;

[TestFixture]
public class PropertyDataStoreTests
{
    private PropertyDataStore underTest ;
    private Mock<ILogger<PropertyDataStore>> logger;
    private readonly Mock<IDataAccessProvider> dataAccessProvider;
    
    public PropertyDataStoreTests()
    {
        dataAccessProvider = new Mock<IDataAccessProvider>();
        logger = new Mock<ILogger<PropertyDataStore>>();
        underTest = new PropertyDataStore(dataAccessProvider.Object, logger.Object);
    }
    
    [Test]
    public async Task CreateNewPropertyDataAsync_WhenCalled_CreatesNewPropertyDataWithTimestampSetToNow()
    {
        // Arrange
        var startTime = DateTime.Now.ToUniversalTime();
        
        // Act
        var returnedPropertyData = await underTest.CreateNewPropertyDataAsync();
        
        // Assert
        var endTime = DateTime.Now.ToUniversalTime();
        Assert.LessOrEqual(startTime, returnedPropertyData.RecommendationsFirstRetrievedAt);
        Assert.GreaterOrEqual(endTime, returnedPropertyData.RecommendationsFirstRetrievedAt);
    }
}