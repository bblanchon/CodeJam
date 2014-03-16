using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestProject
{
    class TestResult
    {
        public TestResult(int mkdirCount)
        {
            MkdirCount = mkdirCount;
        }

        private readonly int MkdirCount;

        public override string ToString()
        {
            return MkdirCount.ToString();
        }
    }
}
