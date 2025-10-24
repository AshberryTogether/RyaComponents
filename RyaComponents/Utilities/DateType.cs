using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    public class DateType
    {
        public const int WEEK_DAYS_COUNT = 7;
        internal virtual object GetSourceObject()
        {
            return null;
        }

        internal virtual int Year { get; }
        internal virtual int Month { get; }
        internal virtual int Day { get; }
        internal virtual bool HasValue { get; }

        internal virtual bool IsDateToday() { return false; }

        internal virtual DateType GetDateInCalendar(int diff) { return new DateType(); }

        internal virtual DateType AddMonths(int month) { return new DateType(); }
        internal virtual string GetMonthName() { return string.Empty; }
        internal virtual bool DateFirstMonthFirstYear() { return true; }
        internal virtual bool DateLastMonthLastYear() { return true; }

    }
}
