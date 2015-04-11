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

        private static int GetMinutesFor(IEnumerable<int> dinners, int minutesAllowed)
        {
            //if (minutesAllowed < 1) return int.MaxValue;

            var biggestStack = dinners.Max();
            if (biggestStack <= 1) return biggestStack;

            var minutesWithSplit = GetMinutesFor(SplitBiggestDinner(dinners), biggestStack - 1) + 1;

            var minutesWithWait = GetMinutesFor(WaitOneMinute(dinners), biggestStack - 1) + 1;

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

            return sortedDinners;
        }

        private static IEnumerable<int> WaitOneMinute(IEnumerable<int> dinners)
        {
            return dinners.Select(x => x - 1).Where(x => x > 0);
        }
    }
}
