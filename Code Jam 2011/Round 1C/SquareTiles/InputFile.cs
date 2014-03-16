using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace SquareTiles
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
                    var RC = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                    var R = RC[0];
                    var C = RC[1];

                    var picture = new char[R, C];

                    for (var row = 0; row < R; row++)
                    {
                        var line = file.ReadLine();

                        for (var col = 0; col < C; col++)
                        {
                            picture[row, col] = line[col];
                        }
                    }

                    testCases.Add(new TestCase
                    {
                        Picture = picture
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
