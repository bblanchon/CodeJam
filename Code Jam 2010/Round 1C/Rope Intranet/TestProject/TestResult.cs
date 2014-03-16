using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class TestResult
    {
        public TestResult(int intersectionCount)
        {
            IntersectionCount = intersectionCount;
        }

        private readonly int IntersectionCount;

        public override string ToString()
        {
            return IntersectionCount.ToString();
        }
    }
}
