// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MagicTrick
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
                    testCases.Add(new TestCase
                    {
                        FirstSelectedRow = ReadSelectedRow(file),
                        SecondSelectedRow = ReadSelectedRow(file)
                    });
                }
            }
        }

        static int[] ReadSelectedRow(StreamReader file)
        {
            var selectedRowNumber = ReadInts(file)[0];

            for (var j = 1; j < selectedRowNumber; j++)
                ReadInts(file);
            var selectedRow = ReadInts(file);

            for (var j = selectedRowNumber; j < 4; j++)
                ReadInts(file);

            return selectedRow;
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
