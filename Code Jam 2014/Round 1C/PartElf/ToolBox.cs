// Copyright © Benoit Blanchon 2014
// All Rights Reserved

using System;
using System.Diagnostics;
using System.Numerics;

namespace PartElf
{
    static class ToolBox
    {
        public static long ComputeGcd(long a, long b)
        {
            while (b != 0)
            {
                var remainder = a % b;
                a = b;
                b = remainder;
            }

            return a;
        }
    }
}
