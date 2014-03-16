using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace KillerWord
{
    class TestCase
    {
        public IList<string> Words, Lists;

        public object Solve()
        {
            return string.Join(" ", Lists.Select(SolveList));
        }

        private string SolveList(string list)
        {
            var scores = Words.Select(x=>ComputeScore(x,list)).ToArray();

            var bestScore = scores.Max();

            return Words[Array.IndexOf(scores, bestScore)];
        }

        private int ComputeScore(string word, string list)
        {
            var candidates = Words.Where(x => x.Length == word.Length);

            var currentPattern = new StringBuilder(new string('.', word.Length));
            var currentScore = 0;

            foreach( var letter in list )
            {
                if( !candidates.Any(x=>x.Contains(letter))) continue;

                if (word.Contains(letter))
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] == letter)
                            currentPattern[i] = letter;
                    }

                    var regex = new Regex(currentPattern.ToString());
                    candidates = candidates.Where(x => regex.IsMatch(x));
                }
                else
                {
                    currentScore++;

                    candidates = candidates.Where(x => !x.Contains(letter));
                }

                if (!currentPattern.ToString().Contains('.')) break;
            }

            return currentScore;
        }

        public override string ToString()
        {
            return string.Format("N={0} M={1}", Words.Count, Lists.Count);
        }
    }
}
