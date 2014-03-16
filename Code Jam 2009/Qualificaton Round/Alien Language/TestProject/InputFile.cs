using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace TestProject
{
    class InputFile
    {
        public InputFile(string filePath)
        {
            using (var file = File.OpenText(filePath))
            {
                var firstLineTokens = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var wordLength = firstLineTokens[0];
                var wordCount = firstLineTokens[1];
                var testCaseCount = firstLineTokens[2];

                dictionnary = new List<string>(wordCount);

                for (var i = 0; i < wordCount; i++)
                {
                    var word = file.ReadLine().Trim();
                    Debug.Assert(word.Length == wordLength);
                    dictionnary.Add(word);
                }

                testCases = new List<TestCase>(testCaseCount);

                for (var i = 0; i < testCaseCount; i++)
                {
                    var pattern = file.ReadLine().Trim();
                    Debug.Assert(pattern.Length >= wordLength);
                    testCases.Add(new TestCase(pattern));
                }
            }
        }

        private readonly List<string> dictionnary;
        private readonly List<TestCase> testCases;

        public IEnumerable<TestCase> TestCases
        {
            get { return testCases; }
        }

        public IEnumerable<string> Dictionnary
        {
            get { return dictionnary; }
        }
    }
}
