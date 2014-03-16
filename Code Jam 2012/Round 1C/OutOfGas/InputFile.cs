using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Globalization;

namespace OutOfGas
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

                    var testCase = new TestCase
                    {
                        Distance = ParseDouble(tokens[0])
                    };                

                    var N = int.Parse(tokens[1]);
                    var A = int.Parse(tokens[2]);

                    for( int n=0 ; n<N ; n++ )
                    {
                        var tx = file.ReadLine().Split(' ').Select(ParseDouble).ToArray();
                        testCase.OtherCar.KnownPositions.Add(
                            new Position { Time = tx[0], X = tx[1] });
                    }

                    testCase.Accelerations = file.ReadLine().Split(' ').Select(ParseDouble).ToList();

                    testCases.Add(testCase);
                }
            }
        }

        private double ParseDouble(string s)
        {
            return double.Parse(s, CultureInfo.InvariantCulture);
        }

        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }
    }
}
