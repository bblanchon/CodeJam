// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PartElf
{
    class InputFile
    {
        public InputFile(string filePath)
        {
            var re = new Regex(@"^(\d+)/(\d+)$");

            using (var file = File.OpenText(filePath))
            {
                var testCaseCount = int.Parse(file.ReadLine());

                testCases = new List<TestCase>(testCaseCount);

                for (var i = 0; i < testCaseCount; i++)
                {
                    var line = file.ReadLine();
                    var m = re.Match(line);

                    testCases.Add(new TestCase
                    {
                        P = long.Parse(m.Groups[1].Value),
                        Q = long.Parse(m.Groups[2].Value)
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

        private static int[] ReadInts(TextReader file)
        {
            return ReadStrings(file).Select(int.Parse).ToArray();
        }
    }
}
