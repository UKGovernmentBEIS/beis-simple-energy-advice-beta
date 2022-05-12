using System.Collections.Generic;
using NUnit.Framework;
using SeaPublicWebsite.DataModels;
using SeaPublicWebsite.ExternalServices;
using SeaPublicWebsite.ExternalServices.Models;
using FluentAssertions;

namespace Tests;

public class BreApiTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SetupTest()
    {
        BreRequest request = new BreRequest()
        {
            construction_date = "A",
            num_storeys = 1,
            heating_fuel = "26",
            property_type = "0",
            built_form = "1",
            num_bedrooms = 2,
            measures = true,
            
        };
        List<Recommendation> recommendations = BreApi.GetRecommendationsForUserRequest(request).Result;
        recommendations.Should().HaveCountGreaterThan(0);
    }
}