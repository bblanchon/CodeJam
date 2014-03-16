// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GoodLuck
{
    class TestCase
    {
        readonly int R, N, M, K;
        readonly long[][] productsList;

        public TestCase(int[] rnmk, IEnumerable<long[]> products)
        {
            R = rnmk[0];
            N = rnmk[1];
            M = rnmk[2];
            K = rnmk[3];
           productsList = products.ToArray();           
        }

        public object Solve()
        {
            var allCombinations = GenerateCombinations();
            return "\n" + string.Join("\n", productsList.Select(x=>SolveProducts(x, allCombinations)));
        }

        Combination SolveProducts(long[] products, IEnumerable<Combination> allCombinations)
        {
            var candidates = allCombinations;

            foreach (var product in products)
            {
                var remainingCandidates = new HashSet<Combination>();

                foreach (var combi in candidates)
                {
                    if (combi.HasProduct(product))
                    {
                        remainingCandidates.Add(combi);
                    }
                }

                candidates = remainingCandidates;
            }

            var result = candidates.First();

            Debug.Assert(result.Values.Count() == N);
            Debug.Assert(result.Values.All(x => x >= 2));
            Debug.Assert(result.Values.All(x => x <= M));

            return result;
        }

        IEnumerable<Combination> GenerateCombinations()
        {
            var list = new List<Combination>();
            int[] buffer = new int[N];

            RecursivelyGenerateCombinations(list, buffer, 0, 2);

            Debug.Assert(list.Count == ToolBox.Factorial(M - 1 + N - 1) / ToolBox.Factorial(N) / ToolBox.Factorial(M - 2));

            return list;
        }

        void RecursivelyGenerateCombinations(IList<Combination> list, int[] buffer, int startIndex, int startValue)
        {
            if (startIndex >= buffer.Length)
            {
                list.Add(new Combination(buffer));
                return;
            }

            for( int i=startValue ; i<=M ; i++ )
            {
                buffer[startIndex] = i;

                RecursivelyGenerateCombinations(list, buffer, startIndex + 1, i);
            }
        }

        class Combination
        {
            readonly int[] values;
            readonly HashSet<long> products;

            public Combination(int[] values)
            {
                this.values = values.ToArray();
                products = new HashSet<long>(EnumerateProducts());
            }

            IEnumerable<long> EnumerateProducts(int startIndex=0)
            {
                if (startIndex < values.Length)
                {
                    foreach (var inner in EnumerateProducts(startIndex + 1))
                    {
                        yield return inner;
                        yield return inner * values[startIndex];
                    }
                }
                else
                {
                    yield return 1;
                }
            }

            public IEnumerable<int> Values
            {
                get { return values; }
            }

            public override string ToString()
            {
 	             return string.Join("", values);
            }

            public bool HasProduct(long product)
            {
                return products.Contains(product);
            }
        }

    }
}
