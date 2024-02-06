using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums
{
    public enum HomeAge
    {
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.Pre1900))]
        Pre1900,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1900To1929))]
        From1900To1929,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1930To1949))]
        From1930To1949,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1950To1966))]
        From1950To1966,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1967To1975))]
        From1967To1975,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1976To1982))]
        From1976To1982,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1983To1990))]
        From1983To1990,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1991To1995))]
        From1991To1995,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From1996To2002))]
        From1996To2002,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From2003To2006))]
        From2003To2006,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From2007To2011))]
        From2007To2011,
        [Display(ResourceType = typeof(Resources.Enum.HomeAge), Description = nameof(Resources.Enum.HomeAge.From2012ToPresent))]
        From2012ToPresent
    }
}