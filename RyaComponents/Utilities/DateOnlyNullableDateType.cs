using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyaComponents.Utilities
{
    internal class DateOnlyNullableDateType : DateType
    {
        private DateOnly? _value = default;
        private static DateOnlyNullableDateType _emptyValue;
        internal static DateOnlyNullableDateType Empty
        {
            get
            {
                if (_emptyValue == null)
                {
                    _emptyValue = new DateOnlyNullableDateType();
                }
                return _emptyValue;
            }
        }

        internal DateOnlyNullableDateType() { }

        internal DateOnlyNullableDateType(DateOnly? value)
        {
            _value = value;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj is DateOnlyNullableDateType dateOnlyDateType)
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
            return _value == DateOnly.FromDateTime(DateTime.Now);
        }

        internal override DateType GetDateInCalendar(int diff)
        {
            if (!_value.HasValue)
            {
                throw new InvalidOperationException("Cannot get date in calendar for a null DateOnly value.");
            }

            DateOnly? date;

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
                return new DateOnlyNullableDateType(date.Value);
            }
            return Empty;
        }

        internal override DateType AddMonths(int month)
        {
            if(!_value.HasValue)
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
            return _value < DateOnly.MinValue.AddMonths(1);
        }
        internal override bool DateLastMonthLastYear()
        {
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
