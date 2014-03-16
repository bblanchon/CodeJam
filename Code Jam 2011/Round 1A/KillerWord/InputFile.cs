using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace KillerWord
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

                    int N = tokens[0];
                    int M = tokens[1];

                    var words = new List<string>(N);
                    for (var j = 0; j < N; j++)                    
                        words.Add(file.ReadLine());

                    var lists = new List<string>(M);
                    for (var j = 0; j < M; j++)
                        lists.Add(file.ReadLine());

                    testCases.Add(new TestCase
                    {
                        Words = words,
                        Lists = lists
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
