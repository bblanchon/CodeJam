// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System.Collections.Generic;
using System.Linq;

namespace MagicTrick
{
    class TestCase
    {
        public IEnumerable<int> FirstSelectedRow { get; set; }
        public IEnumerable<int> SecondSelectedRow { get; set; }

        public object Solve()
        {
            var candidates = FirstSelectedRow.Intersect(SecondSelectedRow).ToArray();

            if (candidates.Length == 0)
                return "Volunteer cheated!";
                
            if (candidates.Length > 1)
                return "Bad magician!";

            return candidates[0];
        }
    }
}
