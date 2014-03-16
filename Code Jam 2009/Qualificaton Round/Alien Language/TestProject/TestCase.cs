using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class TestCase
    {
        public TestCase(string pattern)
        {
            Pattern = pattern;
        }

        
        public readonly string Pattern;

        public override string ToString()
        {
            return Pattern;
        }
    }
}
