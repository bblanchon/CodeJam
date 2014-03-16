using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BotTrust
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
                    var tokens = file.ReadLine().Split(' ');

                    Debug.Assert(tokens.Length > 0);

                    var N = int.Parse(tokens[0]);

                    Debug.Assert(tokens.Length == 2*N + 1);

                    var sequence = new List<Button>(N);

                    for (var j = 0; j < N; j++)
                    {
                        var btn = new Button
                        {
                            Robot = tokens[2*j+1][0],
                            Position = int.Parse(tokens[2*j+2]),
                        };
                        Debug.Assert(btn.Robot == 'O' || btn.Robot == 'B');
                        Debug.Assert(btn.Position >= 1 || btn.Position <= 100);
                        sequence.Add(btn);
                    }

                    testCases.Add(new TestCase
                    {
                        Sequence = sequence
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
