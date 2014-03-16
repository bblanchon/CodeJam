using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SafetyInNumbers
{
    class TestCase
    {
        readonly int[] J;
        readonly int N;
        readonly int X;
        const double epsilon = 1e-5;

        public TestCase(IEnumerable<int> j)
        {
            J = j.ToArray();
            N = J.Length;
            X = J.Sum();
        }

        public object Solve()
        {
            var results = Enumerable.Range(0, N).Select(SolveSingle).ToArray();
            Debug.Assert(Math.Abs(results.Sum() - 100) < epsilon);
            return string.Join(" ", results);
        }

        double SolveSingle(int myIndex)
        {
            // contestants that really matters because they can still loose
            var j = new List<int>(J);
            // sum of their scores
            var x = X;
            // how many they are
            var n = N;

            bool removedSomeone;
            do
            {
                removedSomeone = false;

                var scoreToBeSureToWin = (double) (X + x) / n;

                for (int i = 0 ; i<N ; i++)
                {
                    if (j[i] >= scoreToBeSureToWin)
                    {
                        if (i == myIndex) return 0;

                        x -= j[i];
                        n--;
                        j[i] = 0;
                        removedSomeone = true;
                    }
                }

            } while (removedSomeone);

            var result = (X + x - n * J[myIndex]) * 100.0 / (X * n);

            Debug.Assert(result > 0);
            Debug.Assert(result <= 100+epsilon);
            return result;
        }
    }
}
