// Copyright © Benoit Blanchon 2015
// All Rights Reserved

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace StandingOvation
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
                    var maxShyness = int.Parse(tokens[0]);
                    var audience = tokens[1].Select(x=>x-'0').ToArray();

                    Debug.Assert(maxShyness+1 == audience.Length);
                    Debug.Assert(audience.All(x=>x>=0));
                    Debug.Assert(audience.All(x=>x<=9));

                    testCases.Add(new TestCase
                    {
                        Audience = audience
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
