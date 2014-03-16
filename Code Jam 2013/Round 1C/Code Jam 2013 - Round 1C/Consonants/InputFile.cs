// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Consonants
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
                    var tokens = ReadStrings(file);
                    var name = tokens[0];
                    var l = int.Parse(tokens[1]);

                    testCases.Add(new TestCase(name, l));
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
            return file.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static int[] ReadInts(TextReader file)
        {
            return ReadStrings(file).Select(int.Parse).ToArray();
        }

        private static long[] ReadLongs(TextReader file)
        {
            return ReadStrings(file).Select(long.Parse).ToArray();
        }

        private static double[] ReadDoubles(TextReader file)
        {
            var line = file.ReadLine();
            return ReadStrings(file).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }

        private int[,] ReadIntGrid(TextReader file, int rowCount, int colCount)
        {
            int[,] grid = new int[rowCount, colCount];

            for (var i = 0; i < rowCount; i++)
            {
                var row = ReadInts(file);
                Debug.Assert(row.Length == colCount);
                for (var j = 0; j < colCount; j++)
                {
                    grid[i, j] = row[j];
                }
            }

            return grid;
        }
    }
}
