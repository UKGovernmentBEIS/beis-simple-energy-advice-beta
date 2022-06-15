using NUnit.Framework;
using SeaPublicWebsite.ExternalServices.Models;
using FluentAssertions;

namespace Tests;

public class BreApi
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RequestsMeasures()
    {
        //Arrange
        
        //Act
        BreRequest request = new(
            brePostcode: "",
            brePropertyType: BrePropertyType.House,
            breBuiltForm: BreBuiltForm.Detached,
            breFlatLevel: null,
            breConstructionDate: "A",
            breWallType: BreWallType.CavityWallsWithoutInsulation,
            breRoofType: BreRoofType.PitchedRoofWithoutInsulation,
            breGlazingType: BreGlazingType.SingleGlazed,
            breHeatingFuel: BreHeatingFuel.MainsGas,
            breHotWaterCylinder: null,
            breOccupants: null,
            breHeatingPatternType: BreHeatingPatternType.MorningAndEvening,
            breNormalDaysOffHours: null,
            breTemperature: null
        );
        
        //Assert
        request.measures.Should().Be(true);
    }
}