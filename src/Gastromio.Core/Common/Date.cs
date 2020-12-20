using System;

namespace Gastromio.Core.Common
{
    public readonly struct Date : IEquatable<Date>, IComparable<Date>
    {
        public Date(int year, int month, int day)
        {
            Year = year;
            Month = month;
            Day = day;
        }

        public int Year { get; }
        public int Month { get; }
        public int Day { get; }

        public bool Equals(Date other)
        {
            return Year == other.Year && Month == other.Month && Day == other.Day;
        }

        public override bool Equals(object obj)
        {
            return obj is Date other && Equals(other);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 31 + Year.GetHashCode();
            hash = hash * 31 + Month.GetHashCode();
            hash = hash * 31 + Day.GetHashCode();
            return hash;
        }

        public int CompareTo(Date other)
        {
            var yearComparison = Year.CompareTo(other.Year);
            if (yearComparison != 0)
                return yearComparison;
            var monthComparison = Month.CompareTo(other.Month);
            return monthComparison != 0 ? monthComparison : Day.CompareTo(other.Day);
        }
        
        public static bool operator ==(Date arg1, Date arg2)
        {
            return arg1.CompareTo(arg2) == 0;
        }

        public static bool operator !=(Date arg1, Date arg2)
        {
            return !(arg1 == arg2);
        }

        public static bool operator <(Date arg1, Date arg2)
        {
            return arg1.CompareTo(arg2) < 0;
        }

        public static bool operator <=(Date arg1, Date arg2)
        {
            return arg1.CompareTo(arg2) <= 0;
        }

        public static bool operator >(Date arg1, Date arg2)
        {
            return arg1.CompareTo(arg2) > 0;
        }

        public static bool operator >=(Date arg1, Date arg2)
        {
            return arg1.CompareTo(arg2) >= 0;
        }
    }
}