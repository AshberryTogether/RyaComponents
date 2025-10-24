using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    internal class DateTimeNullableDateType : DateType
    {
        private DateTime? _value = default;
        private static DateTimeNullableDateType _emptyValue;
        internal static DateTimeNullableDateType Empty
        {
            get
            {
                if (_emptyValue == null)
                {
                    _emptyValue = new DateTimeNullableDateType();
                }
                return _emptyValue;
            }
        }

        internal DateTimeNullableDateType() { }

        internal DateTimeNullableDateType(DateTime? value)
        {
            _value = value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateTimeNullableDateType dateOnlyDateType)
            {
                return _value.Equals(dateOnlyDateType._value);
            }
            return base.Equals(obj);
        }

        internal override object GetSourceObject()
        {
            return _value;
        }
        internal override int Year => _value.GetValueOrDefault().Year;
        internal override int Month => _value.GetValueOrDefault().Month;
        internal override int Day => _value.GetValueOrDefault().Day;
        internal override bool HasValue => _value.HasValue;

        internal override bool IsDateToday()
        {
            return _value == DateTime.Today;
        }

        internal override DateType GetDateInCalendar(int diff)
        {
            if (!_value.HasValue)
            {
                throw new InvalidOperationException("Cannot get date in calendar for a null DateOnly value.");
            }

            DateTime? date;

            if (DateFirstMonthFirstYear())
            {
                date = GetDateFromFirstYear(diff);
            }
            else
            {
                date = GetFirstVisibleDate(_value.Value).AddDays(diff);
            }
            if (date.HasValue)
            {
                return new DateTimeNullableDateType(date.Value);
            }
            return Empty;
        }

        internal override DateType AddMonths(int month)
        {
            if (!_value.HasValue)
            {
                throw new InvalidOperationException("Cannot add months to a null DateOnly value.");
            }
            _value = _value.Value.AddMonths(month);
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
