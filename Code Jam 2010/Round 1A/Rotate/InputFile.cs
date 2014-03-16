using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Rotate
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

                    var N = tokens[0];
                    var K = tokens[1];
                    var table = new Table(N);

                    for (var row = 0; row < N; row++)
                    {
                        var input = file.ReadLine().ToArray();
                        Debug.Assert(input.Length == N);

                        for (var col = 0; col < N; col++)
                            table[row,col] = input[col];
                    }

                    testCases.Add(new TestCase { Table = table, K = K });
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
