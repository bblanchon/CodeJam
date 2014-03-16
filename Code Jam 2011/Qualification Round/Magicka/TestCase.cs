using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Magicka
{
    class TestCase
    {
        public override string ToString()
        {
            return Invokation;
        }

        Dictionary<string, char> combinations = new Dictionary<string, char>();
        Dictionary<char, List<char>> oppositions = new Dictionary<char, List<char>>();

        internal void AddCombination(string baseElements, char nonBaseElement)
        {
            Debug.Assert(baseElements.Length == 2);
            Debug.Assert(!combinations.ContainsKey(baseElements));

            combinations.Add(baseElements, nonBaseElement);

            if (baseElements[0] != baseElements[1])
            {
                var rev = new string(baseElements.Reverse().ToArray());
                combinations.Add(rev, nonBaseElement);
            }
        }

        internal void AddOpposition(char element1, char element2)
        {
            if (oppositions.ContainsKey(element1))
            {
                oppositions[element1].Add(element2);
            }
            else
            {
                oppositions.Add(element1, new List<char> { element2 });
            }

            if (oppositions.ContainsKey(element2))
            {
                oppositions[element2].Add(element1);
            }
            else
            {
                oppositions.Add(element2, new List<char> { element1 });
            }
        }

        public string Invokation;

        public object Solve()
        {
            var elements = "";

            var log = false;

            foreach (var e in Invokation)
            {
                if (log) Console.Write(elements + " + " + e + " = ");
                elements = Invoke(elements, e);
                if (log) Console.WriteLine(elements);
            }

            return "[" + string.Join(", ", elements.AsEnumerable()) + "]";
        }

        public string Invoke(string elements, char invoked)
        {
            if (elements.Length == 0)
                return invoked.ToString();

            var twoLast = elements.Substring(elements.Length - 1) + invoked;

            if (combinations.ContainsKey(twoLast))
                return Invoke(elements.Substring(0, elements.Length - 1), combinations[twoLast]);

            if (oppositions.ContainsKey(invoked) && elements.Any(x=>oppositions[invoked].Contains(x)))
                return "";

            return elements + invoked;
        }
    }
}
