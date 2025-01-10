using System.Collections.Generic;
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

    public bool HeatingControlsIsValid
    {
        get
        {
            bool containsExclusiveOption = HeatingControls.Contains(BusinessLogic.Models.Enums.HeatingControls.None) ||
                                           HeatingControls.Contains(
                                               BusinessLogic.Models.Enums.HeatingControls.DoNotKnow);
            if (containsExclusiveOption) return HeatingControls.Count == 1;
            return HeatingControls.Count > 0;
        }
    }
}