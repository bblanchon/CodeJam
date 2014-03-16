// Copyright © Benoit Blanchon $year$
// All Rights Reserved

using System;
using System.Diagnostics;
using System.Numerics;

namespace TestProject
{
    static class ToolBox
    {
        public static bool AreCoprime(int a, int b)
        {
            var gcd = ComputeGcd(a, b);
            return gcd == 1 || gcd == -1;
        }

        public static int ComputeGcd(int a, int b)
        {
            int remainder;

            while (b != 0)
            {
                remainder = a % b;
                a = b;
                b = remainder;
            }

            return a;
        }

        public static BigInteger Factorial(BigInteger n)
        {
            BigInteger result = 1;

            for (BigInteger i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }

        [Conditional("TRACE")]
        public static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
