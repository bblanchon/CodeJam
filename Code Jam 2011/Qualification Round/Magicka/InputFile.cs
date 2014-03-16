using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Magicka
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
                    var tokenIndex = 0;

                    var C = int.Parse(tokens[tokenIndex++]);

                    var testCase = new TestCase();

                    for (var c = 0; c < C; c++)
                    {
                        var combination = tokens[tokenIndex++];
                        Debug.Assert(combination.Length == 3);
                        testCase.AddCombination(combination.Substring(0, 2), combination[2]);
                    }

                    var D = int.Parse(tokens[tokenIndex++]);

                    for (var d = 0; d < D; d++)
                    {
                        var opposition = tokens[tokenIndex++];
                        Debug.Assert(opposition.Length == 2);
                        testCase.AddOpposition(opposition[0], opposition[1]);
                    }

                    var N = int.Parse(tokens[tokenIndex++]);

                    var invokation = tokens[tokenIndex++];

                    Debug.Assert(invokation.Length == N);

                    testCase.Invokation = invokation;

                    Debug.Assert(tokenIndex == tokens.Length);

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
