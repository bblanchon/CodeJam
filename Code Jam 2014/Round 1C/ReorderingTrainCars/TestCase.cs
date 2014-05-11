// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ReorderingTrainCars
{
    class TestCase
    {
        public string[] Cars;
        long counter;

        public object Solve()
        {
            counter = 0;
            CountValidPermutations(0);
            return counter;
        }

        void CountValidPermutations(int startIndex)
        {
            //for (int j = 0; j < startIndex; j++) Console.Write(" ");
            //Console.WriteLine(string.Join(" ", Cars.Take(startIndex)));

            if (!IsValidInRange(startIndex-1, startIndex))
            {
                //for (int j = 0; j < startIndex; j++) Console.Write(" ");
                //Console.WriteLine(string.Join(" ", Cars.Take(startIndex)) + " -> No");
                return;
            }

            if (startIndex >= Cars.Length)
            {
                //for (int j = 0; j < startIndex; j++) Console.Write(" ");
                //Console.WriteLine(string.Join(" ", Cars.Take(startIndex)) + " -> Yes");
                IncrementCounter();
                return;
            }

            for (int i = startIndex; i < Cars.Length; i++)
            {
                SwapCars(startIndex, i);
                CountValidPermutations(startIndex + 1);
                SwapCars(startIndex, i);
            }
        }

        bool IsValidInRange(int startIndex, int stopIndex)
        {
            if (stopIndex == 0) return true;

            var indexOfChar = new int[26];
            var seen = new bool[26];

            var index = 0;
            for (int i = startIndex; i < stopIndex; i++)
            {
                for (int j = 0; j < Cars[i].Length; j++)
                {
                    index++;
                    var c = Cars[i][j] - 'a';

                    if (seen[c] && indexOfChar[c] != index - 1)
                        return false;

                    indexOfChar[c] = index;
                    seen[c] = true;
                }
            }

            return true;
        }

        void IncrementCounter()
        {
            counter++;
            if (counter == 1000000007)
                counter = 0;
        }

        void SwapCars(int index1, int index2)
        {
            if(index1 == index2) return;
            var tmp = Cars[index1];
            Cars[index1] = Cars[index2];
            Cars[index2] = tmp;
        }
    }
}
