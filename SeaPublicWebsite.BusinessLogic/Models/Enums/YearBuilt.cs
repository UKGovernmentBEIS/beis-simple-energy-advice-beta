using System.ComponentModel.DataAnnotations;

namespace SeaPublicWebsite.BusinessLogic.Models.Enums;

public enum YearBuilt
{
    // ⚠ IMPORTANT: Do not reorder this enum!
    // Historic data is saved by enum value as an integer in the PropertyData table,
    // so we must keep this order to maintain the correct mapping.

    // The bands were redefined in PC-572: https://beisdigital.atlassian.net/browse/PC-572
    // The deprecated bands from before this change are listed first, followed by the active bands.

    // DEPRECATED AGE BANDS

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.Pre1930))]
    [YearBuilt(MaxYear = 1929, Deprecated = true)]
    Pre1930,

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1930To1966))]
    [YearBuilt(MaxYear = 1966, Deprecated = true)]
    From1930To1966,

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1967To1982))]
    [YearBuilt(MaxYear = 1982, Deprecated = true)]
    From1967To1982,

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1983To1995))]
    [YearBuilt(MaxYear = 1995, Deprecated = true)]
    From1983To1995,

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From1996To2011))]
    [YearBuilt(MaxYear = 2011, Deprecated = true)]
    From1996To2011,

    // ACTIVE AGE BANDS

    // From2012ToPresent and DoNotKnow were introduced with the original (deprecated) age bands, but are still active.

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.From2012ToPresent))]
    [YearBuilt(MaxYear = int.MaxValue)]
    From2012ToPresent,

    [Display(ResourceType = typeof(Resources.Enum.YearBuilt), Description = nameof(Resources.Enum.YearBuilt.DoNotKnow))]
    DoNotKnow,

    // The age bands below replaced the deprecated ones, in order to align with home age bands in EPCs.

    [GovUkRadioCheckboxLabelText(Text = "Before 1900")]
    [YearBuilt(MaxYear = 1899)]
    Pre1900,

    [GovUkRadioCheckboxLabelText(Text = "1900 to 1929")]
    [YearBuilt(MaxYear = 1929)]
    From1900To1929,

    [GovUkRadioCheckboxLabelText(Text = "1930 to 1949")]
    [YearBuilt(MaxYear = 1949)]
    From1930To1949,

    [GovUkRadioCheckboxLabelText(Text = "1950 to 1966")]
    [YearBuilt(MaxYear = 1966)]
    From1950To1966,

    [GovUkRadioCheckboxLabelText(Text = "1967 to 1975")]
    [YearBuilt(MaxYear = 1975)]
    From1967To1975,

    [GovUkRadioCheckboxLabelText(Text = "1976 to 1982")]
    [YearBuilt(MaxYear = 1982)]
    From1976To1982,

    [GovUkRadioCheckboxLabelText(Text = "1983 to 1990")]
    [YearBuilt(MaxYear = 1990)]
    From1983To1990,

    [GovUkRadioCheckboxLabelText(Text = "1991 to 1995")]
    [YearBuilt(MaxYear = 1995)]
    From1991To1995,

    [GovUkRadioCheckboxLabelText(Text = "1996 to 2002")]
    [YearBuilt(MaxYear = 2002)]
    From1996To2002,

    [GovUkRadioCheckboxLabelText(Text = "2003 to 2006")]
    [YearBuilt(MaxYear = 2006)]
    From2003To2006,

    [GovUkRadioCheckboxLabelText(Text = "2007 to 2011")]
    [YearBuilt(MaxYear = 2011)]
    From2007To2011
}

[AttributeUsage(AttributeTargets.Field)]
public class YearBuiltAttribute : Attribute
{
    public int MaxYear { get; set; }
    public bool Deprecated { get; set; }
}

public static class YearBuiltExtensions
{
    public static bool? IsBefore(this YearBuilt yearBuilt, int year)
    {
        return yearBuilt == YearBuilt.DoNotKnow ? null : yearBuilt.MaxYear() < year;
    }

    public static int? MaxYear(this YearBuilt yearBuilt)
    {
        return yearBuilt.GetAttribute()?.MaxYear;
    }

    public static bool IsDeprecated(this YearBuilt yearBuilt)
    {
        return yearBuilt.GetAttribute()?.Deprecated ?? false;
    }

    private static YearBuiltAttribute GetAttribute(this YearBuilt yearBuilt)
    {
        var enumMember = typeof(YearBuilt).GetMember(yearBuilt.ToString()).SingleOrDefault();
        return (YearBuiltAttribute)enumMember?.GetCustomAttributes(typeof(YearBuiltAttribute), false).SingleOrDefault();
    }
}