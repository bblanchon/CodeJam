using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReverseWords
{
    class TestCase
    {
        public TestCase(string sentence)
        {
            Sentence = sentence;
        }

        public readonly string Sentence;

        public override string ToString()
        {
            return Sentence;
        }
    }
}
