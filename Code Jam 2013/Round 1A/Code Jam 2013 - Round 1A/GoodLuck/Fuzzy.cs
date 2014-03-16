// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;

namespace GoodLuck
{
    struct Fuzzy : IEquatable<Fuzzy>, IComparable<Fuzzy>, IComparable, IFormattable
    {
        const double epsilon = 1e-5;
        readonly double value;

        public Fuzzy(double value)
        {
            this.value = value;
        }

        #region Static properties

        public static double Epsilon { get { return epsilon; } }

        public static Fuzzy NaN { get { return new Fuzzy(double.NaN); } }

        #endregion

        #region Static methods

        public static Fuzzy Abs(Fuzzy x)
        {
            return new Fuzzy(Math.Abs(x.value));
        }

        public static int Ceiling(Fuzzy x)
        {
            return (int)Math.Ceiling(x.value - epsilon);
        }

        public static int Floor(Fuzzy x)
        {
            return (int)Math.Floor(x.value + epsilon);
        }

        public static int Round(Fuzzy x)
        {
            return (int)Math.Round(x.value);
        }

        public static int Sign(Fuzzy x)
        {
            if (x.value > epsilon) return 1;
            if (x.value < -epsilon) return -1;
            return 0;
        }

        public static Fuzzy Sqrt(Fuzzy x)
        {
            return new Fuzzy(Math.Sqrt(x.value));
        }

        #endregion

        #region Properties

        public bool IsFinite
        {
            get { return !double.IsNaN(value) && !double.IsInfinity(value); }
        }

        #endregion

        #region Operators

        public static Fuzzy operator +(Fuzzy x, Fuzzy y)
        {
            return new Fuzzy(x.value + y.value);
        }

        public static Fuzzy operator -(Fuzzy x, Fuzzy y)
        {
            return new Fuzzy(x.value - y.value);
        }

        public static bool operator >(Fuzzy x, Fuzzy y)
        {
            return x.value > y.value + epsilon;
        }

        public static bool operator >=(Fuzzy x, Fuzzy y)
        {
            return x.value > y.value - epsilon;
        }

        public static bool operator <(Fuzzy x, Fuzzy y)
        {
            return x.value < y.value - epsilon;
        }

        public static bool operator <=(Fuzzy x, Fuzzy y)
        {
            return x.value < y.value + epsilon;
        }

        public static bool operator ==(Fuzzy x, Fuzzy y)
        {
            return Math.Abs(x.value - y.value) < epsilon;
        }

        public static bool operator !=(Fuzzy x, Fuzzy y)
        {
            return Math.Abs(x.value - y.value) >= epsilon;
        }

        public static Fuzzy operator /(Fuzzy x, Fuzzy y)
        {
            if (x == 0 && y == 0) return NaN;
            if (x == 0) return 0;
            if (y == 0) return x.value / 0;
            return x.value / y.value;
        }

        public static Fuzzy operator *(Fuzzy x, Fuzzy y)
        {
            return x.value * y.value;
        }

        public static implicit operator Fuzzy(double x)
        {
            return new Fuzzy(x);
        }

        public static explicit operator double(Fuzzy x)
        {
            return x.value;
        }

        #endregion

        #region IEquatable

        public bool Equals(Fuzzy other)
        {
            return this == other;
        }

        public override bool Equals(object other)
        {
            if (other is Fuzzy) return Equals((Fuzzy)other);
            return false;
        }

        // DON'T USE Fuzzy IN HASH TABLES !!!!!!!!!!!!!!!!!!!!
        public override int GetHashCode()
        {
            // HACK: a==b && b==c is doesn't mean a==c
            // so the GetHashCode can't be implemented correctly          
            return base.GetHashCode();
        }

        #endregion

        #region IComparable

        public int CompareTo(Fuzzy other)
        {
            if (this < other) return -1;
            if (this > other) return 1;
            return 0;
        }

        public int CompareTo(object obj)
        {
            return obj is Fuzzy ? CompareTo((Fuzzy)obj) : 0;
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return value.ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        #endregion
    }
}
