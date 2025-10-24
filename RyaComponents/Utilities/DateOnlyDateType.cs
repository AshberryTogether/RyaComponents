using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    internal class DateOnlyDateType : DateType
    {
        private DateOnly _value = default;
        private static DateOnlyDateType _emptyValue;
        internal static DateOnlyDateType Empty
        {
            get
            {
                if (_emptyValue == null)
                {
                    _emptyValue = new DateOnlyDateType();
                }
                return _emptyValue;
            }
        }

        internal DateOnlyDateType() { }

        internal DateOnlyDateType(DateOnly value)
        {
            _value = value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateOnlyDateType dateOnlyDateType)
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
            return _value == DateOnly.FromDateTime(DateTime.Now);
        }

        internal override DateType GetDateInCalendar(int diff) {
            DateOnly? date;

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
                return new DateOnlyDateType(date.Value);
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
        internal override bool DateFirstMonthFirstYear() {
            return _value < DateOnly.MinValue.AddMonths(1);
        }
        internal override bool DateLastMonthLastYear() {
            return _value > DateOnly.MaxValue.AddMonths(-1);
        }

        private static DateOnly? GetDateFromFirstYear(int offsetFromFirstVisibleDate)
        {
            if (offsetFromFirstVisibleDate == 0) return default;
            return DateOnly.MinValue.AddDays(offsetFromFirstVisibleDate - 1);
        }

        private static DateOnly GetFirstVisibleDate(DateOnly date)
        {
            var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
            DateOnly firstOfMonth = new DateOnly(date.Year, date.Month, 1);

            int diff = (WEEK_DAYS_COUNT + (firstOfMonth.DayOfWeek - firstDayOfWeek)) % WEEK_DAYS_COUNT;

            return firstOfMonth.AddDays(-diff);
        }
    }
}
