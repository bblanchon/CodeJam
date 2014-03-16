using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace HallOfMirrors
{
    class InputFile
    {
        public InputFile(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var testCaseCount = ReadInts(file)[0];

                testCases = new List<TestCase>(testCaseCount);

                for (var i = 0; i < testCaseCount; i++)
                {
                    var hwd = ReadInts(file);
                    var height = hwd[0];
                    var width = hwd[1];

                    var grid = new char[width, height];

                    for (var y = 0; y < height; y++)
                    {
                        var line = file.ReadLine();

                        for (var x = 0; x < width; x++)
                        {
                            grid[x, y] = line[x];
                        }
                    }

                    testCases.Add(new TestCase(grid, hwd[2]));
                }
            }
        }

        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }
        
        private static int[] ReadInts(TextReader file)
        {
            return file.ReadLine().Split(' ').Select(int.Parse).ToArray();
        }
    }
}
