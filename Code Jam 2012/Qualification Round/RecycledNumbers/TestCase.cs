using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RecycledNumbers
{
    class TestCase
    {
        public int A { get; set; }
        public int B { get; set; }

        public object Solve()
        {
            Debug.Assert(A <= B);

            int digitCount = 1 + (int)Math.Log10(A);
            int factor = (int)Math.Pow(10, digitCount - 1);
            int result = 0;

            for (var n = A; n <= B; n++)
            {
                int m = n;

                var countedMs = new HashSet<int>();

                for (var i = 0; i < digitCount - 1; i++)
                {
                    // rotate
                    int digit = m % 10;                    
                    m = m / 10 + digit * factor;

                    if (digit == 0) continue;
                    if (n >= m) continue;
                    if (m < A) continue;
                    if (m > B) continue;
                    if (countedMs.Contains(m)) continue;

                    countedMs.Add(m);
                    result++;
                }
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format("A={0}, B={1}", A, B);
        }
    }
}
