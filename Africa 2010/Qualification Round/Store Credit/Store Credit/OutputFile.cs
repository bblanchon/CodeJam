using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Store_Credit
{
    class OutputFile
    {
        public OutputFile(int testCaseCount)
        {
            cases = new List<Tuple<int, int>>(testCaseCount);
        }

        private List<Tuple<int, int>> cases;

        public void Add(int index1, int index2)
        {
            Debug.Assert(index1 < index2);
            cases.Add(new Tuple<int, int>(index1, index2));
        }

        public void Save(string filePath)
        {
            using (var file = new StreamWriter(filePath))
            {
                for (var i = 0; i < cases.Count; i++)
                    file.WriteLine("Case #{0}: {1} {2}", i + 1, cases[i].Item1 + 1, cases[i].Item2 + 1);  
            }
        }
    }
}
