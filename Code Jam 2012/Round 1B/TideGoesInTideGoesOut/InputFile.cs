using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace TideGoesInTideGoesOut
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
                    var hnm = ReadInts(file);
                    var H = hnm[0];
                    var N = hnm[1];
                    var M = hnm[2];
                    var ceilingMap = new int[N][];
                    var floorMap = new int[N][];

                    for (var j = 0; j < N; j++)
                    {
                        ceilingMap[j] = ReadInts(file);
                    }

                    for (var j = 0; j < N; j++)
                    {
                        floorMap[j] = ReadInts(file);
                    }

                    testCases.Add(new TestCase(H,N,M, ceilingMap, floorMap));
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
