using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace DiamondInheritance
{
    class TestCase
    {
        public class Class
        {
            public int Number;

            public List<Class> Children = new List<Class>();

            private Dictionary<int,int> inheritanceCounts;

            private void AddInheritance(int childNumber, int count)
            {
                if (inheritanceCounts.ContainsKey(childNumber))
                    inheritanceCounts[childNumber] += count;
                else
                    inheritanceCounts.Add(childNumber, count);
            }
            
            public Dictionary<int,int> InheritanceCounts
            {
                get
                {
                    if (inheritanceCounts != null)
                        return inheritanceCounts;

                    inheritanceCounts = new Dictionary<int, int>();

                    foreach (var child in Children)
                    {
                        AddInheritance(child.Number, 1);

                        foreach (var kvp in child.InheritanceCounts)
                        {
                            AddInheritance(kvp.Key, kvp.Value);                            
                        }
                    }

                    return inheritanceCounts;
                }
            }

            public bool HasMultipleInheritance
            {
                get
                {
                    return InheritanceCounts.Values.Any(x => x > 1);
                }
            }
        }

        public List<Class> Classes = new List<Class>();

        public object Solve()
        {
            var yes = Classes.Any(x => x.HasMultipleInheritance);
            return yes ? "Yes" : "No";
        }

        public override string ToString()
        {
            return string.Format("{0} classes", Classes.Count);
        }
    }
}
