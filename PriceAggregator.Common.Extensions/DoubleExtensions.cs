namespace PriceAggregator.Common.Extensions;

public static class DoubleExtensions
{
    public static DateTime FromEpochMilliseconds(this double milliseconds)
    {
        var systemDate = new DateTime(1970, 1, 1);
        systemDate = systemDate.AddMilliseconds(milliseconds);

        return systemDate.TrimMinutesAndLess();
    }
}