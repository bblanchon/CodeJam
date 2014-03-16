using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace CruiseControl
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

        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        [Conditional("TRACE")]
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
