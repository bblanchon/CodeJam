// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace PartElf
{
    class TestCase
    {
        public long P;
        public long Q;

        const int MAX_GEN = 40;
        const long MAX_ANCESTORS = 1L << MAX_GEN;

        public object Solve()
        {
            ReduceTerms();

            return IsPossible() ? (object) GetMinGenerationWithTrueElf() : "impossible";
        }

        void ReduceTerms()
        {
            var gcd = ToolBox.ComputeGcd(P, Q);
            P /= gcd;
            Q /= gcd;
        }

        bool IsPossible()
        {
            return MAX_ANCESTORS % Q == 0;
        }


        int GetMinGenerationWithTrueElf()
        {
            for (int i = 0; i <= MAX_GEN; i++)
            {
                if (P << i >= Q) return i;
            }

            throw new InvalidOperationException();
        }
    }
}
