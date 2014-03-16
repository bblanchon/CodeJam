using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Bullseye
{
    class TestCase
    {
        readonly long r;
        readonly long t;

        public TestCase(long r, long t)
        {
            Debug.Assert(r > 0);
            Debug.Assert(t > 0);
            this.r = r;
            this.t = t;
        }

        public object Solve()
        {
            double a = 2;
            double b = 2 * r - 1;
            double c = -t;

            var delta = b * b - 4 * a * c;

            Debug.Assert(delta >= 0);

            var n = (long)((-b + Math.Sqrt(delta)) / 2 / a);

            while (ComputeArea(n) > t) n--;
            while (ComputeArea(n + 1) <= t) n++;

            Debug.Assert(ComputeArea(n) <= t);
            Debug.Assert(ComputeArea(n + 1) > t);

            return n;
        }

        private long ComputeArea(long n)
        {
            return 2 * n * n + (2 * r - 1) * n;
        }
    }
}
