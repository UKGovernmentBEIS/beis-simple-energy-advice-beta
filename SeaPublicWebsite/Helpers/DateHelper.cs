using System;
using Microsoft.AspNetCore.Mvc.Localization;
using SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;

namespace SeaPublicWebsite.Helpers;

public static class DateHelper
{
    public static string GetDateString(this EnergyPriceCapInfoParsed energyPriceCapInfo,
        IHtmlLocalizer<SharedResources> sharedLocalizer)
    {
        return energyPriceCapInfo.GetMonthName(sharedLocalizer) + " " + energyPriceCapInfo.Year;
    }

    private static string GetMonthName(this EnergyPriceCapInfoParsed energyPriceCapInfo,
        IHtmlLocalizer<SharedResources> sharedLocalizer)
    {
        return energyPriceCapInfo.MonthIndex switch
        {
            0 => sharedLocalizer["MonthJanuaryString"].Value,
            1 => sharedLocalizer["MonthFebruaryString"].Value,
            2 => sharedLocalizer["MonthMarchString"].Value,
            3 => sharedLocalizer["MonthAprilString"].Value,
            4 => sharedLocalizer["MonthMayString"].Value,
            5 => sharedLocalizer["MonthJuneString"].Value,
            6 => sharedLocalizer["MonthJulyString"].Value,
            7 => sharedLocalizer["MonthAugustString"].Value,
            8 => sharedLocalizer["MonthSeptemberString"].Value,
            9 => sharedLocalizer["MonthOctoberString"].Value,
            10 => sharedLocalizer["MonthNovemberString"].Value,
            11 => sharedLocalizer["MonthDecemberString"].Value,
            _ => throw new ArgumentOutOfRangeException(nameof(energyPriceCapInfo.MonthIndex))
        };
    }
}