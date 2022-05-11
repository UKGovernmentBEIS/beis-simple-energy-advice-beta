namespace SeaPublicWebsite.ExternalServices.Models
{
    public enum BreHeatingPatternType
    {
        //Values specified by BRE API
        AllDayAndAllNight = 1,
        AllDayButOffAtNight = 2,
        MorningAndEvening = 3,
        JustOnceADay = 4,
        IDontKnow = 5,
        NoneOfTheAbove = 6,
    }
}