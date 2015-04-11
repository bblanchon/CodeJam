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
            return GetMinutesFor(Dinners);
        }

        private static int GetMinutesFor(IEnumerable<int> dinners)
        {
            var biggestStack = dinners.Max();
            if (biggestStack <= 2) return biggestStack;

            var minutesWithSplit = GetMinutesFor(SplitBiggestDinner(dinners)) + 1;

            var minutesWithWait = GetMinutesFor(WaitOneMinute(dinners)) + 1;

            var fastest = Math.Min(minutesWithWait, minutesWithSplit);

            Debug.Assert(fastest <= biggestStack);

            return fastest;
        }

        private static IEnumerable<int> SplitBiggestDinner(IEnumerable<int> dinners)
        {
            var sortedDinners = new List<int>(dinners);
            sortedDinners.Sort();

            var biggestStack = sortedDinners[sortedDinners.Count - 1];
            Debug.Assert(biggestStack>1);

            sortedDinners[sortedDinners.Count - 1] -= biggestStack / 2;
            sortedDinners.Add(biggestStack / 2);

            Debug.Assert(dinners.Sum() == sortedDinners.Sum());

            return sortedDinners;
        }

        private static IEnumerable<int> WaitOneMinute(IEnumerable<int> dinners)
        {
            return dinners.Select(x => x - 1).Where(x => x > 0);
        }
    }
}
