using System.Globalization;

namespace RyaComponents.Utilities
{
    public static class CalendarHelper
    {
        public const int VISIBLE_WEEKS_COUNT = 6;
        public const int WEEK_DAYS_COUNT = 7;

        private static DateOnly GetFirstVisibleDate(DateOnly date)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateOnly firstOfMonth = new DateOnly(date.Year, date.Month, 1);

            int diff = (WEEK_DAYS_COUNT + (firstOfMonth.DayOfWeek - firstDayOfWeek)) % WEEK_DAYS_COUNT;

            return firstOfMonth.AddDays(-diff);
        }

        private static DateOnly? GetDateFromFirstYear(int offsetFromFirstVisibleDate)
        {
            if (offsetFromFirstVisibleDate == 0) return default;
            return DateOnly.MinValue.AddDays(offsetFromFirstVisibleDate - 1);
        }

        public static bool DateFirstMonthFirstYear(DateOnly date)
        {
            return date < DateOnly.MinValue.AddMonths(1);
        }

        public static bool DateLastMonthLastYear(DateOnly date)
        {
            return date > DateOnly.MaxValue.AddMonths(-1);
        }

        public static DateOnly? GetDateInCalendar(DateOnly date, int offsetFromFirstVisibleDate)
        {
            if (DateFirstMonthFirstYear(date))
            {
                return GetDateFromFirstYear(offsetFromFirstVisibleDate);
            }

            return GetFirstVisibleDate(date).AddDays(offsetFromFirstVisibleDate);
        }

        public static IEnumerable<string> GetWeekDayNames()
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            var neutralCultureDayOfWeeks = Enum.GetValues<DayOfWeek>();
            return neutralCultureDayOfWeeks
                    .SkipWhile(d => d != firstDayOfWeek)
                    .Union(neutralCultureDayOfWeeks.TakeWhile(d => d != firstDayOfWeek))
                    .Select(d => d.ToString().Substring(0, 2));
        }
    }
}
