// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CookieClickerAlpha
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
                    var tokens = ReadDoubles(file);
                    testCases.Add(new TestCase
                    {
                        C = tokens[0],
                        F = tokens[1],
                        X = tokens[2]
                    });
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

        private static double[] ReadDoubles(TextReader file)
        {
            return ReadStrings(file).Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
        }
    }
}
