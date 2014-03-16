using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Numerics;

namespace FairAndSquare
{
    class TestCase
    {
        private BigInteger A;
        private BigInteger B;

        public TestCase(BigInteger a, BigInteger b)
        {
            this.A = a;
            this.B = b;
        }

        public object Solve()
        {
            var minSqrt = BigInteger.Parse("1" + new string('0', GetSqrtDigitCount(A) - 1));
            var maxSqrt = BigInteger.Parse("4" + new string('0', GetSqrtDigitCount(B) - 1));
            int count = 0;

            foreach (var sqrt in EnumeratePalindromesBetween(minSqrt, maxSqrt))
            {                
                var square = sqrt * sqrt;
                if (square >= A && square <= B && IsPalindrome(square))
                    count++;
            }

            return count;
        }

        private static bool IsPalindrome(BigInteger value)
        {
            var str = value.ToString();
            return Enumerable.SequenceEqual(str, str.Reverse());
        }

        private static bool IsSquareable(BigInteger value)
        {
            Debug.Assert(IsPalindrome(value));
            return IsPalindrome(value * value);
        }

        private static int GetSqrtDigitCount(BigInteger value)
        {
            int n = value.ToString().Length;
            return (n & 1) == 0 ? n / 2 : n / 2 + 1;
        }        

        private static IEnumerable<BigInteger> EnumeratePalindromesBetween(BigInteger min, BigInteger max)
        {
            var minDigits = Math.Min(min.ToString().Length, max.ToString().Length);
            var maxDigits = Math.Max(max.ToString().Length, max.ToString().Length);

            for (var digitCount = minDigits; digitCount <= maxDigits; digitCount++)
            {
                foreach (var value in EnumerateCandidatesOfLength(digitCount))
                {
                    Debug.Assert(IsPalindrome(value));
                    if (value >= min && value <= max)
                        yield return value;
                }
            }
        }

        private static IEnumerable<BigInteger> EnumerateCandidatesOfLength(int n)
        {
            Debug.Assert(n >= 1);

            if (n == 1)
            {
                for (var digit = 1; digit <= 3; digit++)
                    yield return new BigInteger(digit);

                yield break;
            }

            var digits = new char[n];

            for (var digit = '1'; digit <= '2'; digit++)
            {
                digits[0] = digit;
                digits[digits.Length - 1] = digit;
                foreach(var candidate in RecursivelyEnumerateCandidates(digits, 1) )
                    yield return candidate;
            }
        }

        private static IEnumerable<BigInteger> RecursivelyEnumerateCandidates(char[] digits, int level)
        {
            // zero digit to set
            if (level * 2 > digits.Length - 1)
            {
                var candidate = BigInteger.Parse(new string(digits));
                if (IsSquareable(candidate)) yield return candidate;
                yield break;
            }

            // one digit to set, the one at the center
            if (level * 2 == digits.Length - 1)
            {
                for (var digit = '0'; digit <= '2'; digit++)
                {
                    digits[level] = digit;

                    var candidate = BigInteger.Parse(new string(digits));

                    // don't need to higher values, if it already fails
                    if (!IsSquareable(candidate)) break;
                    
                    yield return candidate;
                }

                yield break;
            }

            // two digits to set
            for (var digit = '0'; digit <= '1'; digit++)
            {
                digits[level] = digit;
                digits[digits.Length - 1 - level] = digit;
                int count = 0;
                foreach (var candidate in RecursivelyEnumerateCandidates(digits, level + 1))
                {
                    count++;
                    yield return candidate;
                }

                if (count == 0) break; // don't need to higher values, if it already fails
            }
        }        
    }
}
