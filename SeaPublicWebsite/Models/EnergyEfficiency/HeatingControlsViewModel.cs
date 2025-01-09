using System;
using System.Collections.Generic;
using System.Linq;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using SeaPublicWebsite.BusinessLogic.Models.Enums;
using SeaPublicWebsite.Resources;

namespace SeaPublicWebsite.Models.EnergyEfficiency;

public class HeatingControlsViewModel : QuestionFlowViewModel
{
    public string Reference { get; set; }

    [ModelBinder(typeof(GovUkCheckboxEnumSetBinder<HeatingControls>))]
    [GovUkValidateCustom(CustomValidationPropertyName = nameof(HeatingControlsIsValid),
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = nameof(ErrorMessages.HeatingControlsRequired))]
    public List<HeatingControls> HeatingControls { get; set; } = [];

    public bool HeatingControlsIsValid => !(HeatingControls.Count == 0 ||
                                            ((HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls
                                                  .DoNotKnow) ||
                                              HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls
                                                  .None)) && HeatingControls.Count > 1));
}