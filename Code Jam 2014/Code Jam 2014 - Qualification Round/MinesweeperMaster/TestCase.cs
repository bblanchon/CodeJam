// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;

namespace MinesweeperMaster
{
    class TestCase
    {
        public int C { get; set; }
        public int R { get; set; }
        public int M { get; set; }

        public object Solve()
        {
            var map = new Map(R, C);
            map.PlaceMines(M);
            map.Click();

            return map.IsWon ? map.Render() : "Impossible";
        }
    }
}
