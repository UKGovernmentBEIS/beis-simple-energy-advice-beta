using System.Collections.Generic;
using GovUkDesignSystem.Attributes;
using GovUkDesignSystem.Attributes.ValidationAttributes;
using GovUkDesignSystem.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace SeaPublicWebsite.Models.Feedback;

public class FeedbackSurveyViewModel
{
    [ModelBinder(typeof(GovUkCheckboxEnumSetBinder<VisitReason>))]
    public List<VisitReason> VisitReasonList { get; set; } = new();
    public string OtherReason { get; set; }
    
    [GovUkValidateRequired(ErrorMessage = "Select 'Yes' if you found all the information you were looking for")]
    public FoundInformation? FoundInformation { get; set; }
    public string NotFoundInformationDetails { get; set; }
    
    [ModelBinder(typeof(GovUkCheckboxEnumSetBinder<HowInformationHelped>))]
    public List<HowInformationHelped> HowInformationHelpedList { get; set; } = new();
    public string OtherHelp { get; set; }
    
    [ModelBinder(typeof(GovUkCheckboxEnumSetBinder<WhatPlannedToDo>))]
    public List<WhatPlannedToDo> WhatPlannedToDoList { get; set; } = new();
    public string OtherPlan { get; set; }
}

public enum VisitReason
{
    [GovUkRadioCheckboxLabelText(Text = "To reduce my energy bill")]
    ReduceEnergyBill,
    
    [GovUkRadioCheckboxLabelText(Text = "To reduce my carbon emissions")]
    ReduceCarbonEmissions,

    [GovUkRadioCheckboxLabelText(Text = "I want to install a specific measure")]
    InstallSpecificMeasure,

    [GovUkRadioCheckboxLabelText(Text = "I want to improve my EPC rating")]
    ImproveEpcRating,

    [GovUkRadioCheckboxLabelText(Text = "To find out if I am eligible for government funding/scheme")]
    CheckForFundingEligibility,

    [GovUkRadioCheckboxLabelText(Text = "Something else")]
    Other
}

public enum FoundInformation
{
    [GovUkRadioCheckboxLabelText(Text = "Yes")]
    Yes,
    
    [GovUkRadioCheckboxLabelText(Text = "No")]
    No
}

public enum HowInformationHelped
{
    [GovUkRadioCheckboxLabelText(Text = "I know what I can install to improve the energy efficiency of my home")]
    ImproveEnergyEfficiency,
    
    [GovUkRadioCheckboxLabelText(Text = "I know what I can do to reduce carbon emissions")]
    ReduceCarbonEmissions,

    [GovUkRadioCheckboxLabelText(Text = "I know the cost to install specific measures in my home")]
    CostForSpecificMeasure,

    [GovUkRadioCheckboxLabelText(Text = "I found an accredited installer")]
    FoundAccreditedInstaller,

    [GovUkRadioCheckboxLabelText(Text = "I know how to improve my EPC rating")]
    ImproveEpcRating,
    
    [GovUkRadioCheckboxLabelText(Text = "I have found government funding/schemees I am eligible for")]
    FoundGovernmentFunding,
    
    [GovUkRadioCheckboxLabelText(Text = "Nothing I didn’t already know")]
    Nothing,

    [GovUkRadioCheckboxLabelText(Text = "Something else")]
    Other
}

public enum WhatPlannedToDo
{
    [GovUkRadioCheckboxLabelText(Text = "Install a measure myself")]
    InstallMeasure,
    
    [GovUkRadioCheckboxLabelText(Text = "Contact an installer/local tradesperson")]
    ContactInstaller,

    [GovUkRadioCheckboxLabelText(Text = "Look for funding (government or private)")]
    SearchForFunding,

    [GovUkRadioCheckboxLabelText(Text = "More research")]
    MoreResearch,

    [GovUkRadioCheckboxLabelText(Text = "Nothing")]
    Nothing,

    [GovUkRadioCheckboxLabelText(Text = "Something else")]
    Other
}

