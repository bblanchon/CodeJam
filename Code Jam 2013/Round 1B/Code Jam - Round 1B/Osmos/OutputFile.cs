// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Globalization;
using System.IO;

namespace Osmos
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
            var text = string.Format(CultureInfo.InvariantCulture, "Case #{0}: {1:0.#####}", ++caseCount, result);
            file.WriteLine(text);
            Console.WriteLine(text);
        }
    }
}
