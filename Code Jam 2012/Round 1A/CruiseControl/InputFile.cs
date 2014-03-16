using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace CruiseControl
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
                    var cars = new Car[N];

                    for (var j = 0; j < N; j++)
                    {
                        var tokens = ReadStrings(file);
                        cars[j] = new Car(
                            tokens[0] == "L" ? Lane.Left : Lane.Right,
                            int.Parse(tokens[1]),
                            int.Parse(tokens[2]));
                    }

                    testCases.Add(new TestCase(cars));
                }
            }
        }

        private readonly List<TestCase> testCases;

        public IReadOnlyList<TestCase> TestCases
        {
            get { return testCases; }
        }

        private static string[] ReadStrings(TextReader file)
        {
            return file.ReadLine().Split(' ');
        }

        private static int[] ReadInts(TextReader file)
        {
            return ReadStrings(file).Select(int.Parse).ToArray();
        }

        private static double[] ReadDoubles(TextReader file)
        {
            var line = file.ReadLine();
            return ReadStrings(file).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
