using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace TestProject
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

                    var existingCount = tokens[0];
                    var createCount = tokens[1];

                    var existingDirectories = new List<string>(existingCount);
                    for (var j = 0; j < existingCount; j++)                    
                        existingDirectories.Add(file.ReadLine());

                    var directoriesToCreate = new List<string>(existingCount);
                    for (var j = 0; j < createCount; j++)
                        directoriesToCreate.Add(file.ReadLine());

                    testCases.Add(new TestCase(existingDirectories.AsReadOnly(), directoriesToCreate.AsReadOnly()));
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
