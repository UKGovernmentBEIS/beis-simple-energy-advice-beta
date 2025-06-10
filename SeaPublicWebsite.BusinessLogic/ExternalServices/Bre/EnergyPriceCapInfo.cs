namespace SeaPublicWebsite.BusinessLogic.ExternalServices.Bre;

public abstract class EnergyPriceCapInfo;

public class EnergyPriceCapInfoNotRequested : EnergyPriceCapInfo;

public class EnergyPriceCapInfoNotParsed : EnergyPriceCapInfo;

public class EnergyPriceCapInfoParsed(int year, int monthIndex) : EnergyPriceCapInfo
{
    public readonly int Year = year;
    public readonly int MonthIndex = monthIndex;
}