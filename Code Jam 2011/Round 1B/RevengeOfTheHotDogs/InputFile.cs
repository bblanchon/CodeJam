using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace RevengeOfTheHotDogs
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
                    var CD = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                    var C = CD[0];

                    var positions = new Dictionary<int, int>(C);

                    for (int row = 0; row < C; row++)
                    {
                        var PV = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                        positions.Add(PV[0], PV[1]);
                    }

                    testCases.Add(new TestCase
                    {
                        D = CD[1],
                        Positions = positions
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
