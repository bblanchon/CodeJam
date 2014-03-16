// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Pogo
{
    class TestCase
    {
        readonly int X;
        readonly int Y;

        public TestCase(int x, int y)
        {
            // TODO: Complete member initialization
            this.X = x;
            this.Y = y;
        }
        // TODO: add members

        public object Solve()
        {
            int x = 0, y = 0;
            int n = 1;
            var sequence = new StringBuilder();
                        
            while (x != X || y != Y)
            {
                if (X > x) 
                {
                    if (X > x + n)
                    {
                        sequence.Append("E");
                        x += n;
                        n += 1;
                    }
                    else
                    {
                        sequence.Append("WE");
                        x += 1;
                        n += 2;
                    }
                }
                else if (X < x)
                {
                    if (X < x - n)
                    {
                        sequence.Append("W");
                        x -= n;
                        n += 1;
                    }
                    else
                    {
                        sequence.Append("EW");
                        x -= 1;
                        n += 2;
                    }
                }
                else if (Y > y)
                {
                    if (Y > y + n)
                    {
                        sequence.Append("N");
                        y += n;
                        n += 1;
                    }
                    else
                    {
                        sequence.Append("SN");
                        y += 1;
                        n += 2;
                    }
                }
                else if (Y < y)
                {
                    if (Y < y - n)
                    {
                        sequence.Append("S");
                        y -= n;
                        n += 1;
                    }
                    else
                    {
                        sequence.Append("NS");
                        y -= 1;
                        n += 2;
                    }
                }
            }

            Debug.Assert(n <= 500);
            Debug.Assert(n == sequence.Length + 1);
            Debug.Assert(x == X);
            Debug.Assert(y == Y);

            return sequence;
        }
    }
}
