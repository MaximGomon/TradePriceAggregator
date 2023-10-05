namespace PriceAggregator.Common.Extensions;

public static class IntExtensions
{
    public static DateTime EpochToSystemTime(this int seconds)
    {
        var systemDate = new DateTime(1970, 1, 1);
        systemDate = systemDate.AddSeconds(seconds);

        return systemDate.TrimMinutesAndLess();
    }

    public static string SecondToBitfinexTime(this int second)
    {
        switch (second)
        {
            case 3600:
                return "1h";
            default:
                return second.ToString();
        }
    }
}