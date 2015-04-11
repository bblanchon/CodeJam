// Copyright © Benoit Blanchon 2015
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace InfiniteHouseOfPancakes
{
    class TestCase
    {
        public IEnumerable<int> Dinners;

        public object Solve()
        {
            return GetMinutesFor(Dinners, int.MaxValue);
        }

        private int GetMinutesFor(IEnumerable<int> dinners, int minutesAllowed)
        {
            if (minutesAllowed < 1) return int.MaxValue;

            var minutesWithoutSplit = dinners.Max();

            if (minutesWithoutSplit <= 2) return minutesWithoutSplit;
            
            var minutesWithSplit = GetMinutesFor(SplitBiggestDinner(dinners), minutesWithoutSplit - 1) + 1;

            return Math.Min(minutesWithoutSplit, minutesWithSplit);
        }

        private static IEnumerable<int> SplitBiggestDinner(IEnumerable<int> dinners)
        {
            var sortedDinners = new List<int>(dinners);
            sortedDinners.Sort();

            var biggest = sortedDinners[sortedDinners.Count - 1];
            Debug.Assert(biggest>1);

            sortedDinners[sortedDinners.Count - 1] -= biggest / 2;
            sortedDinners.Add(biggest / 2);

            return sortedDinners;
        }
    }
}
