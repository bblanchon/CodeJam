using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace PasswordProblem
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
                    var AB = ReadInts(file);
                    var p = ReadDoubles(file);

                    testCases.Add(new TestCase(AB[0], AB[1], p));
                }
            }
        }

        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }

        private static int[] ReadInts(TextReader file)
        {
            var line = file.ReadLine();
            return line.Split(' ').Select(int.Parse).ToArray();
        }

        private static double[] ReadDoubles(TextReader file)
        {
            var line = file.ReadLine();
            return line.Split(' ').Select(s=>double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
