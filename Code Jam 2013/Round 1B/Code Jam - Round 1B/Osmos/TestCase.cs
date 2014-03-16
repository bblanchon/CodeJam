// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Osmos
{
    class TestCase
    {
        readonly int A;
        readonly int[] motes;        

        public TestCase(int a, IEnumerable<int> motes)
        {
            this.A = a;
            this.motes = motes.ToArray();

            Array.Sort(this.motes);
        }

        public object Solve()
        {
            var answer = ComputeCost(A, 0, motes.Length);

            Debug.Assert(answer >= 0);
            Debug.Assert(answer <= motes.Length);

            return answer;
        }

        int ComputeCost(int currentSize, int nextMote, int maxCost)
        {
            Debug.Assert(currentSize > 0);
            Debug.Assert(currentSize >= 0);
            Debug.Assert(maxCost >= 0);
            Debug.Assert(maxCost <= motes.Length);

            if (maxCost <= 0) return 0;

            while (nextMote < motes.Length && currentSize > motes[nextMote])
            {
                Debug.Assert(motes[nextMote] > 0);

                currentSize += motes[nextMote];
                nextMote++;
            }

            if (nextMote >= motes.Length) return 0;

            var costOfRemoving = motes.Length-nextMote;
            var costOfAdding = 1 + ComputeCost(currentSize+(currentSize-1), nextMote, Math.Min(maxCost-1, costOfRemoving));

            Debug.Assert(costOfRemoving > 0);
            Debug.Assert(costOfAdding > 0);

            return Math.Min(costOfAdding, costOfRemoving);
        }
    }
}
