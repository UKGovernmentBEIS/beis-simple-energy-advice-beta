using System;
using GovUkDesignSystem.Attributes;
using SeaPublicWebsite.BusinessLogic.Models.Enums;

namespace SeaPublicWebsite.Helpers;

public static class EnumDisplayExtensions
{
    public static string DescriptionForAnswerSummary(this FloorConstruction? floorConstruction)
    {
        return floorConstruction is FloorConstruction.Mix
            ? "A mix of both suspended timber and solid concrete"
            : GovUkRadioCheckboxLabelTextAttribute.GetLabelText(floorConstruction);
    }

    public static string HeatingControlEnumToEpcHintResourceKey(this HeatingControls heatingControl)
    {
        return heatingControl switch
        {
            HeatingControls.Programmer => "ProgrammerHeatingControlEPCHintString",
            HeatingControls.RoomThermostats => "RoomThermostatHeatingControlEPCHintString",
            HeatingControls.ThermostaticRadiatorValves => "ThermostaticRadiatorValvesHeatingControlEPCHintString",
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}