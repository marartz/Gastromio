using System;

namespace FoodOrderSystem.Domain.Model
{
    public class ValueType<T> where T : IComparable
    {
        public ValueType(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public int CompareTo(object obj)
        {
            if (!(obj is ValueType<T>))
                return -1;
            return Value.CompareTo((obj as ValueType<T>).Value);
        }

        public static bool operator ==(ValueType<T> left, ValueType<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ValueType<T> left, ValueType<T> right)
        {
            return !left.Equals(right);
        }
    }
}
