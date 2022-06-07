using GovUkDesignSystem.Attributes.ValidationAttributes;

namespace SeaPublicWebsite.Models.Feedback;

public class FeedbackFormViewModel
{
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter what your were doing")]
    public string WhatUserWasDoing { get; set; }
    
    [GovUkValidateRequired(ErrorMessageIfMissing = "Enter what you would like to tell us")]
    public string WhatUserToldUs { get; set; }
}