using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RevengeOfTheHotDogs
{
    class TestCase
    {
        public int D;
        public Dictionary<int, int> Positions;

        public int C
        {
            get { return Positions.Count; }
        }

        public override string ToString()
        {
            return string.Format("C = {0}, D = {0}", C, D);
        }

        public object Solve()
        {
            double maxTime = 0;

            foreach (var P in Positions.Keys)
            {
                double left = P - D / 2.0;
                double right = P + D / 2.0; 
                int count = Positions[P];

                for (int p = P - D +1 ; p < P + D; p++)
                {
                    if ( p != P && Positions.ContainsKey(p))
                    {
                        left = Math.Min(left, p - D / 2.0);
                        right = Math.Max(right, p + D / 2.0);
                        count += Positions[p];
                    }
                }

                var currentWidth = right - left;
                Debug.Assert(currentWidth > 0);
                Debug.Assert(currentWidth < D * 3);

                var targetWidth = count * D;

                var timeToReach = (targetWidth - currentWidth) / 2;

                Debug.Assert(timeToReach >= 0);

                maxTime = Math.Max(maxTime, timeToReach);
            }

            return maxTime;
        }
    }
}
