using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NumberGame
{
    class TestCase
    {
        public int A1, A2, B1, B2;

        public override string ToString()
        {
            return string.Format("A={0}-{1}, B={2}-{3}", A1, A2, B1, B2);
        }

        static Dictionary<int, Dictionary<int, bool>> cache =
            new Dictionary<int, Dictionary<int, bool>>();

        public int Solve()
        {
            int count = 0;
            
            for (var A = A1; A <= A2; A++)
            {
                for (var B = B1; B <= B2; B++)
                {
                    if (IsWinning(A, B))
                        count++;
                }
            }

            return count;
        }

        private void AddToCache(int A, int B, bool isWinning)
        {
            Debug.Assert(A < B);

            if (cache.ContainsKey(A))
            {
                cache[A].Add(B, isWinning);
            }
            else
            {
                cache.Add(A, new Dictionary<int, bool> { { B, isWinning } });
            }
        }

        private bool? GetFromCache(int A, int B)
        {
            Debug.Assert(A < B);

            if (cache.ContainsKey(A) && cache[A].ContainsKey(B))
                return cache[A][B];

            return null;
        }

        private bool IsWinning(int A, int B)
        {
            Debug.Assert(A > 0);
            Debug.Assert(A <= Math.Max(A2,B2));
            Debug.Assert(B > 0);
            Debug.Assert(B <= Math.Max(A2, B2));

            if (B == A) return false;
            if (B < A) return IsWinning(B, A);

            Debug.Assert(A < B);
            if (B % A == 0) return true;

            var cachedResult = GetFromCache(A, B);
            if (cachedResult.HasValue) return cachedResult.Value;            

            int n = B / A;                 

            for (int k = n; k >= 1; k--)
            {
                if (!IsWinning(A, B - k * A))
                {
                    AddToCache(A, B, true);
                    return true;
                }
            }

            AddToCache(A, B, false);
            return false;
        }
    }
}
