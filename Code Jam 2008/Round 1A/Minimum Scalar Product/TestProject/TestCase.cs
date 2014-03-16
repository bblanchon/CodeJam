using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class TestCase
    {
        public TestCase(Vector x, Vector y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);
            Debug.Assert(x.Count == y.Count);

            X = x;
            Y = y;
        }

        public readonly Vector X, Y;

        public override string ToString()
        {
            return string.Format("X={0}, Y={1}", X, Y);
        }
    }
}
