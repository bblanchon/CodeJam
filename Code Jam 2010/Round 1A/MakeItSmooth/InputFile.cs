using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace MakeItSmooth
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
                    var dimn = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                    var pixels = file.ReadLine().Split(' ').Select(int.Parse).ToList();

                    Debug.Assert(pixels.Count == dimn[3]);

                    testCases.Add(new TestCase
                    {
                        D = dimn[0],
                        I = dimn[1],
                        M = dimn[2],
                        Pixels = pixels
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
