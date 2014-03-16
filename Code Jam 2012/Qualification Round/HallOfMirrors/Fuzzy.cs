using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HallOfMirrors
{
    struct Fuzzy : IEquatable<Fuzzy>
    {
        const double epsilon = 0.0001;
        readonly double value;

        public Fuzzy(double value)
        {
            this.value = value;
        }

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
            return !(x < y);
        }

        public static bool operator <(Fuzzy x, Fuzzy y)
        {
            return x.value < y.value - epsilon;
        }

        public static bool operator <=(Fuzzy x, Fuzzy y)
        {
            return !(x > y);
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
            return new Fuzzy(x.value / y.value);
        }

        public static Fuzzy operator *(Fuzzy x, Fuzzy y)
        {
            return new Fuzzy(x.value * y.value);
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

        // caution: this is a strict equality to make Equals/GetHashCode() consistent
        public bool Equals(Fuzzy other)
        {
            return other.value.Equals(value);
        }

        // caution: this is a strict equality to make Equals/GetHashCode() consistent
        public override bool Equals(object other)
        {
            if( other is Fuzzy ) return Equals((Fuzzy)other);
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        #endregion
    }
}
