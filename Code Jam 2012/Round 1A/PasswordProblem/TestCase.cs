using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PasswordProblem
{
    class TestCase
    {
        readonly int A, B;
        readonly double[] p;
        readonly double[] P;

        public TestCase(int A, int B, double[] p)
        {
            Debug.Assert(B >= A);
            Debug.Assert(p.Length == A);

            this.A = A;
            this.B = B;
            this.p = p;

            // P[i] = p1 * ... * pi
            P = new double[A + 1];
            P[0] = 1;
            for (int i = 0; i < A; i++)
            {
                P[i + 1] = P[i] * p[i];
            }
        }

        public object Solve()
        {
            return EnumerateSolutions().Min();
        }

        public IEnumerable<double> EnumerateSolutions()
        {
            // simplest solution: hit enter now
            yield return B + 2;

            // other cases: hit backspace 'k' times
            for (int k = 0; k <= A; k++)
            {
                yield return 2 * B - A + 2 * k + 2 - P[A - k] * (B + 1);
            }
        }
    }
}
