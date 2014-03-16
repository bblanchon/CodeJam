// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Consonants
{
    class TestCase
    {
        readonly string name;
        readonly int l;

        public TestCase(string name, int l)
        {
            this.name = name;
            this.l = l;
        }

        public object Solve()
        {
            int nvalue = 0;

            for (int i = 0; i <= name.Length - l; i++)
            {
                for (int n = l; n <= name.Length - i; n++)
                {
                    if (ContainsConsecutiveCosonants(i, n))
                    {
                        nvalue += 1 + name.Length - i - n;
                        break;
                    }
                }
            }

            return nvalue;
        }

        private bool ContainsConsecutiveCosonants(int i, int n)
        {
            int consecutives = 0;

            for( int idx=0 ; idx<n ; idx++)
            {
                var c = name[i + idx];

                if (IsConsonant(c))
                    consecutives++;
                else
                    consecutives = 0;

                if (consecutives >= l) return true;
            }

            return false;
        }

        private bool IsConsonant(char c)
        {
            return c != 'a' && c != 'e' && c != 'i' && c != 'o' && c != 'u';
        }
    }
}
