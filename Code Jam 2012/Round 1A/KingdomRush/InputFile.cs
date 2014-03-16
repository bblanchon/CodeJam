using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace KingdomRush
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
                    var N = ReadInts(file)[0];
                    var levels = new List<Level>(N);

                    for (var j = 0; j < N; j++)
                    {
                        var ab = ReadInts(file);
                        levels.Add(new Level(ab[0], ab[1]));
                    }

                    testCases.Add(new TestCase(levels));
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
            return line.Split(' ').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
