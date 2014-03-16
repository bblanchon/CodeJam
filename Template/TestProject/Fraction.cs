// Copyright © Benoit Blanchon $year$
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProject
{
    struct Fraction : IEquatable<Fraction>, IComparable<Fraction>, IComparable, IFormattable
    {
        readonly int numerator, denominator;

        public Fraction(int numerator, int denominator)
        {
            this.numerator = denominator >= 0 ? numerator : -numerator;
            this.denominator = Math.Abs(denominator);
        }

        #region Properties

        public bool IsFinite
        {
            get { return denominator != 0; }
        }

        #endregion

        #region Operators

        public static Fraction operator +(Fraction x, Fraction y)
        {
            return new Fraction(x.numerator * y.denominator + y.numerator * x.denominator, x.denominator * y.denominator);
        }

        public static Fraction operator -(Fraction x, Fraction y)
        {
            return new Fraction(x.numerator * y.denominator - y.numerator * x.denominator, x.denominator * y.denominator);
        }

        public static bool operator >(Fraction x, Fraction y)
        {
            return x.CompareTo(y) > 0;
        }

        public static bool operator >=(Fraction x, Fraction y)
        {
            return x.CompareTo(y) >= 0;
        }

        public static bool operator <(Fraction x, Fraction y)
        {
            return x.CompareTo(y) < 0;
        }

        public static bool operator <=(Fraction x, Fraction y)
        {
            return x.CompareTo(y) <= 0;
        }

        public static bool operator ==(Fraction x, Fraction y)
        {
            return x.CompareTo(y) == 0;
        }

        public static bool operator !=(Fraction x, Fraction y)
        {
            return x.CompareTo(y) != 0;
        }

        public static Fraction operator /(Fraction x, Fraction y)
        {
            return new Fraction(x.numerator * y.denominator, x.denominator * y.numerator);
        }

        public static Fraction operator *(Fraction x, Fraction y)
        {
            return new Fraction(x.numerator * y.numerator, x.denominator * y.denominator);
        }

        public static implicit operator Fraction(int x)
        {
            return new Fraction(x, 1);
        }

        public static explicit operator double(Fraction x)
        {
            return (double)x.numerator / (double)x.denominator;
        }

        #endregion

        #region IEquatable

        public bool Equals(Fraction other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object other)
        {
            if (other is Fraction) return Equals((Fraction)other);
            return false;
        }

        public override int GetHashCode()
        {
            return ((double)this).GetHashCode();
        }

        #endregion

        #region IComparable

        public int CompareTo(Fraction other)
        {
            return (numerator * other.denominator).CompareTo(other.numerator * denominator);
        }

        public int CompareTo(object obj)
        {
            return obj is Fraction ? CompareTo((Fraction)obj) : 0;
        }

        #endregion

        #region IFormattable

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((double)this).ToString(format, formatProvider);
        }

        public override string ToString()
        {
            return ToString(null, null);
        }

        #endregion
    }

    static class FractionExtensions
    {
        public static Fraction Sum(this IEnumerable<Fraction> source)
        {
            return source.Aggregate((a, b) => a + b);
        }
    }
}
