using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Store_Credit
{
    class TestCase
    {
        public TestCase(int credit, IList<int> items)
        {
            Credit = credit;
            Items = items;
        }

        public readonly int Credit;
        public readonly IList<int> Items;
    }
}
