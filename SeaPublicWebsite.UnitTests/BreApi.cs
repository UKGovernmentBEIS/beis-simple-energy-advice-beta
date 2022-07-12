using NUnit.Framework;
using SeaPublicWebsite.ExternalServices.Models;
using FluentAssertions;

namespace Tests;

public class BreApi
{
    [Test]
    public void BreRequestMeasuresShouldBeTrue()
    {
        // Act
        BreRequest request = new(
            brePropertyType: BrePropertyType.House,
            breBuiltForm: BreBuiltForm.Detached,
            breFlatLevel: null,
            breConstructionDate: "A",
            breWallType: BreWallType.CavityWallsWithoutInsulation,
            breRoofType: BreRoofType.PitchedRoofWithoutInsulation,
            breGlazingType: BreGlazingType.SingleGlazed,
            breHeatingSystem: BreHeatingSystem.GasBoiler,
            breHotWaterCylinder: null,
            breOccupants: null,
            breHeatingPatternType: BreHeatingPatternType.MorningAndEvening,
            breNormalDaysOffHours: null,
            breTemperature: null,
            breFloorType: BreFloorType.DontKnow
        );
        
        // Assert
        request.measures.Should().Be(true);
    }
}