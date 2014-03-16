using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReverseWords
{
    class TestResult
    {
        public TestResult(string sentence)
        {
            Sentence = sentence;
        }

        private readonly string Sentence;

        public override string ToString()
        {
            return Sentence;
        }
    }
}
