using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CandySplitting
{
    class TestCase
    {
        public List<int> Candies;

        public override string ToString()
        {
            return string.Join(", ", Candies);
        }

        public object Solve()
        {
            if (Xor(Candies) == 0)
                return Candies.Sum() - Candies.Min();
            else
                return "NO";
        }

        public int Xor(int x, int y)
        {
            return x ^ y;
        }

        public int Xor(IEnumerable<int> lst)
        {
            return lst.Aggregate(0, Xor);
        }
    }
}
