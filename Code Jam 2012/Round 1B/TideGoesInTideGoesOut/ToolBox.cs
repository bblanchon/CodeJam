using System;
using System.Diagnostics;

namespace TideGoesInTideGoesOut
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

        [Conditional("TRACE")]
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
