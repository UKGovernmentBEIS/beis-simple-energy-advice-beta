using System;
using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using SeaPublicWebsite.BusinessLogic.Models;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Helpers;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class HeatingControlsViewModel : QuestionFlowViewModel
{
    public string Reference { get; set; }
    public Epc Epc { get; set; }

    [ModelBinder(typeof(GovUkCheckboxEnumSetBinder<HeatingControls>))]
    [GovUkHasCustomValidator(CustomerValidatorPropertyName = nameof(HeatingControlsIsValid),
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = nameof(ErrorMessages.HeatingControlsRequired))]
    public List<HeatingControls> HeatingControls { get; set; } = [];

    public bool HeatingControlsIsValid
    {
        get
        {
            var containsExclusiveOption = HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls.None) ||
                                          HeatingControls.Contains(
                                              BusinessLogic.Models.Enums.HeatingControls.DoNotKnow);
            //If the answer contains an exclusive option, check it's the only option. Otherwise, ensure at least one non-exclusive option.
            if (containsExclusiveOption) return HeatingControls.Count == 1;
            return HeatingControls.Count > 0;
        }
    }

    public string GetHeatingControlsEpcHintTextKey()
    {
        if (Epc.HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls.None))
        {
            return "NoHeatingControlsEPCHintString";
        }
                                
        return Epc.HeatingControls.Count switch
        {
            1 => "OneHeatingControlEPCHintString",
            2 => "TwoHeatingControlsEPCHintString",
            3 => "ThreeHeatingControlsEPCHintString",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public object[] GetHeatingControlsEpcHintTextParams(IHtmlLocalizer<SharedResources> sharedLocalizer)
    {
        if (Epc.HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls.None))
        {
            return [Epc.LodgementYear];
        }
        var localisedControls = Epc.HeatingControls.Select(control => sharedLocalizer[control.HeatingControlEnumToEpcHintResourceKey()]);
                                
        return [Epc.LodgementYear, ..localisedControls];
    }
}