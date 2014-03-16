using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Store_Credit
{
    class InputFile
    {
        public InputFile(string filePath)
        {
            TestCases = Read(filePath);
        }        

        public readonly IList<TestCase> TestCases;

        private static TestCase[] Read(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var testCaseCount = int.Parse(file.ReadLine());
                var testCases = new TestCase[testCaseCount];

                for (var testCaseIndex = 0; testCaseIndex < testCaseCount; testCaseIndex++)
                {
                    var c = int.Parse(file.ReadLine());
                    var i = int.Parse(file.ReadLine());

                    var items = file.ReadLine().Split(' ').Select(int.Parse).ToArray();

                    Debug.Assert(items.Length == i);

                    testCases[testCaseIndex] = new TestCase(c, items);
                }

                return testCases;
            }
        }
    }
}
