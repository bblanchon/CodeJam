using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DancingWithTheGooglers
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
                    var tokens = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                    
                    testCases.Add(new TestCase
                    {
                        N = tokens[0],
                        S = tokens[1],
                        p = tokens[2],
                        t = tokens.Skip(3).ToArray(),
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
