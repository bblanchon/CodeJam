using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VanishingNumbers
{
    class TestCase
    {
        public List<double> Numbers;

        public override string ToString()
        {
            return string.Join(", ", Numbers);
        }

        public object Solve()
        {
            Numbers.Sort();
            solution = new List<double>(Numbers.Count);

            for (var roundNumber = 1; solution.Count < Numbers.Count; roundNumber++)
            {
                var round = GetRound(roundNumber);

                foreach( var n in Numbers )
                {
                    if (solution.Contains(n)) continue;

                    if (round.EliminateRanges.Any(r=>r.Contains(n)))
                    {
                        Console.WriteLine("Eliminate {0} at round {1}", n, roundNumber);
                        solution.Add(n);
                    }
                }
            }

            Debug.Assert(solution.Count == Numbers.Count);

            return "\n" + string.Join("\n", solution);
        }

        List<double> solution;

        class Round
        {
            public Round()
            {
                EliminateRanges = new List<Range>();
                KeepRanges = new List<Range>();
            }

            public List<Range> EliminateRanges;
            public List<Range> KeepRanges;
        }

        class Range
        {
            public Range(double min, double max)
            {
                Debug.Assert(min < max);
                Debug.Assert(min >= 0);
                Debug.Assert(max <= 1);
                Min = min;
                Max = max;
            }

            public bool Contains(double n)
            {
                return n >= Min && n <= Max;
            }

            public bool Contains(Range r)
            {
                return Min <= r.Min && Max >= r.Max;
            }

            public bool Intersects(Range r)
            {
                return
                    Min > r.Min && Min < r.Max ||
                    Max > r.Min && Max < r.Max;
            }

            public readonly double Min, Max;
            public double Width { get { return Max - Min; } }
        }

        Dictionary<int, Round> roundCache = new Dictionary<int, Round>();

        Round GetRound(int n)
        {
            if (roundCache.ContainsKey(n)) return roundCache[n];

            var round = new Round();

            if (n == 0)
            {
                round.KeepRanges.Add (new Range(0, 1));
            }
            else
            {
                var prev = GetRound(n - 1);

                foreach (var r in prev.KeepRanges)
                {
                    var r1 = new Range(r.Min, r.Min + r.Width / 3);
                    var r2 = new Range(r.Min + r.Width / 3, r.Max - r.Width / 3);
                    var r3 = new Range(r.Max - r.Width / 3, r.Max);

                    round.KeepRanges.Add(r1);
                    round.EliminateRanges.Add(r2);
                    round.KeepRanges.Add(r3);

                    Debug.Assert(r.Contains(r1));
                    Debug.Assert(r.Contains(r2));
                    Debug.Assert(r.Contains(r3));
                    Debug.Assert(!r1.Intersects(r2));
                    Debug.Assert(!r2.Intersects(r3));
                    Debug.Assert(!r1.Intersects(r3));
                }

              //  Debug.Assert(!prev.KeepRanges.Any(r=>round.EliminateRanges.Any(x=>r.Intersects(x))));
            }

            Debug.Assert(round.KeepRanges.Count == 1 << n);
            Debug.Assert(round.EliminateRanges.Count == (n>0 ? 1 << (n-1) : 0));

            roundCache.Add (n ,round) ;

            return round;
        }
    }
}
