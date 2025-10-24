using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    internal class DateTimeDateType : DateType
    {
        private DateTime _value = default;
        private static DateTimeDateType _emptyValue;
        internal static DateTimeDateType Empty
        {
            get
            {
                if (_emptyValue == null)
                {
                    _emptyValue = new DateTimeDateType();
                }
                return _emptyValue;
            }
        }

        internal DateTimeDateType() { }

        internal DateTimeDateType(DateTime value)
        {
            _value = value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateTimeDateType dateOnlyDateType)
            {
                return _value.Equals(dateOnlyDateType._value);
            }
            return base.Equals(obj);
        }

        internal override object GetSourceObject()
        {
            return _value;
        }
        internal override int Year => _value.Year;
        internal override int Month => _value.Month;
        internal override int Day => _value.Day;
        internal override bool HasValue => this != Empty;

        internal override bool IsDateToday()
        {
            return _value == DateTime.Today;
        }

        internal override DateType GetDateInCalendar(int diff)
        {
            DateTime? date;

            if (DateFirstMonthFirstYear())
            {
                date = GetDateFromFirstYear(diff);
            }
            else
            {
                date = GetFirstVisibleDate(_value).AddDays(diff);
            }
            if (date.HasValue)
            {
                return new DateTimeDateType(date.Value);
            }
            return Empty;
        }

        internal override DateType AddMonths(int month)
        {
            _value = _value.AddMonths(month);
            return this;
        }
        internal override string GetMonthName()
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
        }
        internal override bool DateFirstMonthFirstYear()
        {
            return _value < DateTime.MinValue.AddMonths(1);
        }
        internal override bool DateLastMonthLastYear()
        {
            return _value > DateTime.MaxValue.AddMonths(-1);
        }

        private static DateTime? GetDateFromFirstYear(int offsetFromFirstVisibleDate)
        {
            if (offsetFromFirstVisibleDate == 0) return default;
            return DateTime.MinValue.AddDays(offsetFromFirstVisibleDate - 1);
        }

        private static DateTime GetFirstVisibleDate(DateTime date)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateTime firstOfMonth = new DateTime(date.Year, date.Month, 1);

            int diff = (WEEK_DAYS_COUNT + (firstOfMonth.DayOfWeek - firstDayOfWeek)) % WEEK_DAYS_COUNT;

            return firstOfMonth.AddDays(-diff);
        }
    }
}
