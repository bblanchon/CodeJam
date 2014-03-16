using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rotate
{
    class TestCase
    {
        public Table Table;
        public int K;

        public override string ToString()
        {
            return string.Format("N = {0}, K = {1}", Table.Width, K);
        }
    }
}
