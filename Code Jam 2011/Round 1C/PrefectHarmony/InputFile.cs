using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace PrefectHarmony
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
                    var NLH = file.ReadLine().Split().Select(int.Parse).ToArray();

                    var N = NLH[0];
                    var L = NLH[1];
                    var H = NLH[2];

                    var notes = file.ReadLine().Split().Select(int.Parse).ToArray();
                    Debug.Assert(notes.Length == N);

                    testCases.Add(new TestCase
                    {
                        Notes = notes,
                        L = L,
                        H = H,
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
