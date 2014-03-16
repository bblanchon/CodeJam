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
            int previous = 0;

            for (int i =  name.Length - l; i >=0; i--)
            {
                int increment;

                if( IsConsonant(name[i]))
                {
                    int consecutives = 0;
                    increment = 0;

                    for (int j = i; j < name.Length; j++)
                    {
                        var c = name[j];

                        if (IsConsonant(c))
                            consecutives++;
                        else
                            consecutives = 0;

                        if (consecutives >= l)
                        {
                            increment = name.Length - j;
                            break;
                        }
                    }
                }
                else
                {
                    increment = previous;
                }

                nvalue += increment;
                previous = increment;
            }

            return nvalue;
        }

        private bool IsConsonant(char c)
        {
            return c != 'a' && c != 'e' && c != 'i' && c != 'o' && c != 'u';
        }
    }
}
