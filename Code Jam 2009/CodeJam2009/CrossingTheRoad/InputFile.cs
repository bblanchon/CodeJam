using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace CrossingTheRoad
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
                    var nm = file.ReadLine().Split(' ').Select(int.Parse).ToArray();

                    var N = nm[0];
                    var M = nm[1];

                    var S = new int[N, M];
                    var W = new int[N, M];
                    var T = new int[N, M];

                    for (int row = 0; row < N; row++)
                    {
                        var tokens = file.ReadLine().Split(' ').Select(int.Parse).ToArray();

                        for (int col = 0; col < M; col++)
                        {
                            S[row, col] = tokens[3 * col + 0];
                            W[row, col] = tokens[3 * col + 1];
                            T[row, col] = tokens[3 * col + 2];
                        }
                    }

                    testCases.Add(new TestCase
                    {
                        N = N,
                        M = M,
                        S = S, 
                        W = W, 
                        T = T,
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
