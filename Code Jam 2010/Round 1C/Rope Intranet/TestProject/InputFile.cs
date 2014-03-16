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
                    var wireCount = int.Parse(file.ReadLine());

                    var wires = new List<Wire>(wireCount);

                    for (var j = 0; j < wireCount; j++)
                    {
                        var tokens = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                        Debug.Assert(tokens.Length == 2);

                        wires.Add(new Wire(tokens[0], tokens[1]));
                    }

                    testCases.Add(new TestCase(wires.AsReadOnly()));
                }
            }
        }

        private readonly List<string> dictionnary;
        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }

        public IEnumerable<string> Dictionnary
        {
            get { return dictionnary; }
        }
    }
}
