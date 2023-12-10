namespace Time.Commerce.Core.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfTheDay(this DateTime d) => new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
    public static DateTime EndOfTheDay(this DateTime d) => new DateTime(d.Year, d.Month, d.Day, 23, 59, 59);
    public static DateTimeOffset StartOfTheDay(this DateTimeOffset d) => new DateTimeOffset(d.Year, d.Month, d.Day, 0, 0, 0, TimeSpan.Zero);
    public static DateTimeOffset EndOfTheDay(this DateTimeOffset d) => new DateTimeOffset(d.Year, d.Month, d.Day, 23, 59, 59, TimeSpan.Zero);
    public static int GetDaysInMonth(this DateTimeOffset date) => DateTime.DaysInMonth(date.Year, date.Month);

    public static (DateTimeOffset startDate, DateTimeOffset endDate) GetStartDateAndEndDateInMonth(this DateTimeOffset date)
    {
        var days = date.GetDaysInMonth();
        var startDate = new DateTimeOffset(date.Year, date.Month, 1, 0, 0, 0, TimeSpan.Zero);
        var endDate = new DateTimeOffset(date.Year, date.Month, days, 23, 59, 59, TimeSpan.Zero);
        return (startDate, endDate);
    }

    public static DateTimeOffset GetNextDateForDay(this DateTimeOffset startDate, DayOfWeek desiredDay)
    {
        while (startDate.DayOfWeek != desiredDay)
            startDate = startDate.AddDays(1D);

        return startDate;
    }

    /// <summary>
    /// Calculates the age in years of the current System.DateTime object today.
    /// </summary>
    /// <param name="birthDate">The date of birth</param>
    /// <returns>Age in years today. 0 is returned for a future date of birth.</returns>
    public static int Age(this DateTimeOffset birthDate)
    {
        return Age(birthDate, DateTimeOffset.Now);
    }

    /// <summary>
    /// Calculates the age in years of the current System.DateTime object on a later date.
    /// </summary>
    /// <param name="birthDate">The date of birth</param>
    /// <param name="laterDate">The date on which to calculate the age.</param>
    /// <returns>Age in years on a later day. 0 is returned as minimum.</returns>
    public static int Age(this DateTimeOffset birthDate, DateTimeOffset laterDate)
    {
        int age;
        age = laterDate.Year - birthDate.Year;

        if (age > 0)
        {
            age -= Convert.ToInt32(laterDate.Date < birthDate.Date.AddYears(age));
        }
        else
        {
            age = 0;
        }

        return age;
    }
}
