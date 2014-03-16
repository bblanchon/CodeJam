using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Treasure
{
    class TestCase
    {
        readonly int[] keys;
        readonly Chest[] chests;

        public TestCase(int[] keys, Chest[] chests)
        {
            this.keys = keys;
            this.chests = chests;
        }

        public object Solve()
        {
            var start = new Strategy(keys, Enumerable.Empty<Chest>(), chests);
            var strategy = FindWinningStrategy(start);

            if (strategy != null)
            {
                Debug.Assert(strategy.ClosedChests.Count() == 0);
                Debug.Assert(strategy.OpenedChests.Count() == chests.Count());
                Debug.Assert(strategy.OpenedChests.Distinct().Count() == chests.Count());
                var remainingKeys = strategy.KeysInHand.Count();
                var totalKeys = keys.Count() + chests.Sum(x => x.ContainedKeys.Count());
                var expected = totalKeys - chests.Count();
                Debug.Assert(remainingKeys == expected);
                return string.Join(" ", strategy.OpenedChests.Select(x => x.Id));
            }
            else
            {
                return "IMPOSSIBLE";
            }
        }

        Strategy FindWinningStrategy(Strategy strategy)
        {
            //Console.WriteLine("{0}", string.Join(" ", strategy.OpenedChests.Select(x=>x.Id)));

            Debug.Assert(strategy.ClosedChests.Count() + strategy.OpenedChests.Count() == chests.Count());

            if (!strategy.ContainsEnoughKeys || !strategy.CanReachAnyChest)
            {
                //Console.WriteLine("STOP");
                return null;
            }

            if (!strategy.ClosedChests.Any())
            {
                // we have a winner...
                return strategy;
            }

            var openeableChests = strategy.ClosedChests
                .Where(chest => strategy.KeysInHand.Contains(chest.RequiredKey));

            foreach (var chest in openeableChests)
            {
                var inner = FindWinningStrategy(strategy.OpenChest(chest));
                if( inner != null )
                    return inner;
            }

            return null;
        }
    }
}
