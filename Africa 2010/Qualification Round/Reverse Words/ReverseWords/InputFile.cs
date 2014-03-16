using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace ReverseWords
{
    class InputFile : IEnumerable<TestCase>
    {
        public InputFile(string filePath)
        {
            FilePath = filePath;
        }

        public readonly string FilePath;

        public IEnumerator<TestCase> GetEnumerator()
        {
            using (var file = File.OpenText(FilePath))
            {
                var testCaseCount = int.Parse(file.ReadLine());

                for (var i = 0; i < testCaseCount; i++)
                {
                    var sentence = file.ReadLine();

                    yield return new TestCase(sentence);
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
