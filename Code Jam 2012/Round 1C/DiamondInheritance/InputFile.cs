using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace DiamondInheritance
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
                    var testCase = new TestCase();

                    int classCount = int.Parse(file.ReadLine());

                    for (var j = 0; j < classCount; j++)
                    {
                        testCase.Classes.Add(new TestCase.Class
                        {
                            Number = j + 1,
                        });
                    }

                    for (var j = 0; j < classCount; j++)
                    {
                        var tokens = file.ReadLine().Split(' ').Select(int.Parse).ToArray();

                        foreach( var childNumber in tokens.Skip(1) )
                            testCase.Classes[j].Children.Add(testCase.Classes[childNumber-1]);
                    }

                    testCases.Add(testCase);
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
