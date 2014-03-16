using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace EqualSums
{
    class TestCase
    {
        readonly long[] originalSet;
        readonly Random rand;


        public TestCase(long[] set)
        {
            originalSet = set;
            rand = new Random();
        }
        
        public object Solve()
        {
            const int subsetSize = 6;

            var sumsMap = new Dictionary<long, long[]>();

            while( true )
            {
                var subset = GetRandomSubset(subsetSize);
                var sum = subset.Sum();

                long[] otherSubset;
                sumsMap.TryGetValue(sum, out otherSubset);

                if (otherSubset == null)
                {
                    sumsMap.Add(sum, subset);
                    continue;
                }

                var sub1 = subset.Where(x => !otherSubset.Contains(x)).ToArray();
                var sub2 = otherSubset.Where(x => !subset.Contains(x)).ToArray();

                if (!sub1.SequenceEqual(sub2))
                {
                    ToolBox.Log("Found in {0} subsets", sumsMap.Count);
                    Debug.Assert(sub1.Sum() == sub2.Sum());

                    return "\r\n"
                        + string.Join(" ", sub1)
                        + "\r\n"
                        + string.Join(" ", sub2);
                }
            }
        }

        /*
        long[] GetRandomSubset(int size)
        {          
            var subset = new long[size];

            for (var i = 0; i < size; i++)
            {
                long newValue;

                do {
                    newValue = originalSet[rand.Next(originalSet.Length)];
                } while( subset.Take(i).Contains(newValue) );

                subset[i] = newValue;
            }

            return subset;
        }*/
        long[] GetRandomSubset(int size)
        {
            var indexes = new int[size];

            for (var i = 0; i < size; i++)
            {
                do
                {
                    indexes[i] = rand.Next(originalSet.Length);
                } while (indexes.Take(i).Contains(indexes[i]));
            }

            Array.Sort(indexes);
            return indexes.Select(x=>originalSet[x]).ToArray();
        }
    }
}
