using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SeaPublicWebsite.BusinessLogic.ExternalServices.EpbEpc;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.Services.EnergyEfficiency.Epc;
using Tests.Builders;

namespace Tests.Website.Services;

[TestFixture]
public class EpcFilterServiceTests
{
    private EpcFilterService epcFilterService;
    private Mock<IEpcApi> mockEpcApi;
    
    [SetUp]
    public void Setup()
    {
        mockEpcApi = new Mock<IEpcApi>();
        epcFilterService = new EpcFilterService(mockEpcApi.Object);
    }

    [Test]
    public async Task FilterEpcsNotUniqueByAddressToMostRecent_WhenCalledWithDuplicateEpcSearchResults_ShouldReturnTheMostRecent()
    {
        // Arrange
        var expectedEpcs = new List<EpcSearchResult>();
        expectedEpcs.Add( new EpcSearchResultBuilder("1").WithAddress("1", "2").WithPostcode("AAA AAA").Build());
        expectedEpcs.Add( new EpcSearchResultBuilder("2").WithAddress("2", "3").WithPostcode("AAA AAA").Build());

        var epcSearchResults = new List<EpcSearchResult>(expectedEpcs);
        epcSearchResults.Add( new EpcSearchResultBuilder("3").WithAddress("1", "2").WithPostcode("AAA AAA").Build());

        mockEpcApi.Setup(epcApi => epcApi.GetEpcForId("1").Result)
            .Returns(new Epc { IsLatestAssessmentForAddress = true });
        mockEpcApi.Setup(epcApi => epcApi.GetEpcForId("3").Result)
            .Returns(new Epc { IsLatestAssessmentForAddress = false });

        // Act
        var filteredEpcs = await epcFilterService.FilterEpcsNotUniqueByAddressToMostRecent(epcSearchResults);

        // Assert
        filteredEpcs.Should().BeEquivalentTo(expectedEpcs);
    }
}