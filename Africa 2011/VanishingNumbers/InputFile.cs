using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace VanishingNumbers
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
                    var count = int.Parse(file.ReadLine());
                    var numbers = new List<double>(count);

                    for (var j = 0; j < count; j++)
                    {
                        numbers.Add (double.Parse(file.ReadLine()));
                        Debug.Assert(numbers[j] > 0);
                        Debug.Assert(numbers[j] < 1);
                    }

                    testCases.Add(new TestCase
                    {
                        Numbers = numbers
                    });
                }
            }
        }

        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }
    }
}
