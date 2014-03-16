using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class TestResult
    {
        public TestResult(int matchCount)
        {
            MatchCount = matchCount;
        }

        private readonly int MatchCount;

        public override string ToString()
        {
            return MatchCount.ToString();
        }
    }
}
