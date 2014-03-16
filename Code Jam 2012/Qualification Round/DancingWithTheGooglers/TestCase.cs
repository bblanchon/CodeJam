using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DancingWithTheGooglers
{
    class TestCase
    {
        public int N { get; set; }

        public int S { get; set; }

        public int p { get; set; }

        public int[] t { get; set; }

        public object Solve()
        {
            Debug.Assert(p <= 10);
            if (p < 2) return t.Count(x => x >= p);

            // number that already matches
            var sureCount = t.Count(x => 
                x > 1 && 
                Math.Ceiling(x / 3.0) >= p);

            // number that can be upgraded
            var potentialCount = t.Count(x =>
                x > 1 &&
                x % 3 != 1 &&
                Math.Ceiling(x / 3.0) == p - 1);

            var response = sureCount + Math.Min(potentialCount, S);

            Debug.Assert(response >= 0);
            Debug.Assert(response <= t.Length);

            return response;
        }

        public override string ToString()
        {
            return string.Format("N={0}, S={1}, p={2}, t=[{3}]",
                N, S, p, string.Join(",", t));
        }
    }
}
