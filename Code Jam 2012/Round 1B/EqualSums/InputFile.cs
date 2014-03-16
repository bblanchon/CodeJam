using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace EqualSums
{
    class InputFile
    {
        public InputFile(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var testCaseCount = int.Parse(file.ReadLine());

                testCases = new List<TestCase>(testCaseCount);

                for (var i = 0; i < testCaseCount; i++)
                {
                    testCases.Add(new TestCase(ReadInts(file)));
                }
            }
        }

        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }

        private static string[] ReadStrings(TextReader file)
        {
            return file.ReadLine().Split(' ');
        }

        private static long[] ReadInts(TextReader file)
        {
            return ReadStrings(file).Select(long.Parse).ToArray();
        }

        private static double[] ReadDoubles(TextReader file)
        {
            var line = file.ReadLine();
            return ReadStrings(file).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
