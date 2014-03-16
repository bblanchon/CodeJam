using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace EqualSums
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

        public void Add(object result)
        {
            var text = string.Format(CultureInfo.InvariantCulture, "Case #{0}:{1:0.#####}", ++caseCount, result);
            file.WriteLine(text);
            Console.WriteLine(text);
        }
    }
}
