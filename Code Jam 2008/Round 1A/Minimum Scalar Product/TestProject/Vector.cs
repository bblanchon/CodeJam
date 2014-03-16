using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class Vector : List<long>
    {
        public Vector(IEnumerable<long> v)
            : base(v)
        {

        }

        /*
         * NOT NEEDED BUT USEFUL
         * 
        public IEnumerable<Vector> Permutations
        {
            get
            {
                if (Count == 1) 
                {
                    yield return this;               
                }
                else
                {
                    var v0 = this[0];
                    var v1n = new Vector(this.Skip(1));
                    var v1nPermutations = v1n.Permutations.ToArray();

                    for (int pos = 0; pos < Count; pos++)
                    {
                        foreach (var w in v1nPermutations)
                        {
                            var myPermutation = new Vector(w);

                            myPermutation.Insert(pos, v0);

                            Debug.Assert(myPermutation.Count == Count);

                            yield return myPermutation;
                        }
                    }
                }
            }
        }
        */

        public static long operator*(Vector x, Vector y)
        {
            Debug.Assert(x.Count == y.Count);
            return x.Zip(y, (xi, yi) => xi * yi).Aggregate((xyi, sum) => sum + xyi);
        }

        public override string ToString()
        {
            return string.Join(" ", this);
        }
    }
}
