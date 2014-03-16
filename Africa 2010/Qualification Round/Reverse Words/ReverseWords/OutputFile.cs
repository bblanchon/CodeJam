using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ReverseWords
{
    class OutputFile : IDisposable
    {
        public OutputFile(string filePath)
        {
            file = new StreamWriter(filePath);
            caseCount = 0;
        }

        public void Dispose()
        {
            file.Dispose();
        }

        private StreamWriter file;
        private int caseCount;

        public void Add(TestResult result)
        {
            file.WriteLine("Case #{0}: {1}", ++caseCount, result);
        }
    }
}
