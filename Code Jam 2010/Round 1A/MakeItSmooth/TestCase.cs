using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MakeItSmooth
{
    class TestCase
    {
        public int D, I, M;
        public List<int> Pixels;

        public override string ToString()
        {
            return string.Format("D={0}, I={1}, M={2}, Pixels={3}", D, I, M, string.Join(" ",Pixels));
        }

        private int minCost;

        public int Solve()
        {
            Debug.Assert(D >= 0);
            Debug.Assert(I >= 0);
            Debug.Assert(M >= 0);
            Debug.Assert(Pixels.Count >= 1);

            minCost = D * (Pixels.Count - 1);

            Solve(Pixels.Count, 0);

            if (I == 0 && M > 0)
            {
                Debug.Assert(minCost == 0);
            }

            return minCost;   
        }

        private void Solve(int N, int currentCost)
        {
            Debug.Assert(N >= 0);
            Debug.Assert(N <= Pixels.Count);
            Debug.Assert(currentCost >= 0);

            if (currentCost >= minCost) return;

            if (N == 0)
            {
                minCost = currentCost;
                return;
            }

           Solve(N - 1, currentCost+D);

           for (int i = 0; i < 256; i++)
           {
               var mod_cost = Math.Abs(i - Pixels[N - 1]);
               Solve(N - 1, i, currentCost + mod_cost);
           }
        }

        private void Solve(int N, int finalValue, int currentCost)
        {            
            Debug.Assert(N >= 0);
            Debug.Assert(N < Pixels.Count);
            Debug.Assert(finalValue >= 0);
            Debug.Assert(finalValue <= 255);
            Debug.Assert(currentCost >= 0);

            if (currentCost >= minCost) return;

            if (N == 0)
            {
                minCost = currentCost;
                return;
            }

            Solve(N - 1, finalValue, currentCost + D);

            if (M > 0)
            {
                for (int i = 0; i < 256; i++)
                {
                    var mod_cost = Math.Abs(i - Pixels[N - 1]);
                    var gap = Math.Abs(finalValue - i);
                    var insert_count = gap > M ? (gap-1) / M : 0;
                    var insert_cost = insert_count * I;
                    Debug.Assert(mod_cost >= 0);
                    Debug.Assert(insert_cost >= 0);
                    Debug.Assert(insert_cost == 0 || Math.Abs(finalValue - i) > M);
                    Solve(N - 1, i, currentCost + mod_cost + insert_cost);
                }
            }
            else
            {
                var mod_cost = Math.Abs(finalValue - Pixels[N - 1]);
                Solve(N - 1, finalValue, currentCost + mod_cost);
            }
        }
    }
}
