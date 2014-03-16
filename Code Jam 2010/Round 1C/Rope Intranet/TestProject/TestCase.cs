using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class TestCase
    {
        public TestCase(IList<Wire> wires)
        {
            Wires = wires;
        }

        public readonly IList<Wire> Wires;

        public override string ToString()
        {
            return string.Join(",", Wires);
        }
    }
}
