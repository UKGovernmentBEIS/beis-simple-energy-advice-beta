using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum YearBuilt
{
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.Pre1930))]
    Pre1930,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1930To1966))]
    From1930To1966,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1967To1982))]
    From1967To1982,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1983To1995))]
    From1983To1995,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1996To2011))]
    From1996To2011,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From2012ToPresent))]
    From2012ToPresent,
    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.DoNotKnow))]
    DoNotKnow
}