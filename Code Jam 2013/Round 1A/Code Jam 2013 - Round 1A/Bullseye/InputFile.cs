using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace Bullseye
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
                    var rt = ReadInts(file);

                    testCases.Add(new TestCase(rt[0], rt[1]));
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
    }
}
