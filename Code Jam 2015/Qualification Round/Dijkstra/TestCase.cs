// Copyright © Benoit Blanchon 2015
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Dijkstra
{
    enum Quaternion
    {
        One,
        I,
        J,
        K,
        MinusOne,
        MinusI,
        MinusJ,
        MinusK
    }

    class TestCase
    {
        public string Sequence;
        public long Repetitions;

        public object Solve()
        {
            foreach (var firstCut in EnumerateCutPosition(Quaternion.I, 0))
            {
                foreach (var secondCut in EnumerateCutPosition(Quaternion.J, firstCut))
                {
                    foreach (var end in EnumerateCutPosition(Quaternion.K, secondCut))
                    {
                        if (end == TotalLength)
                            return "YES";
                    }
                }  
            }

            return "NO";
        }

        private IEnumerable<int> EnumerateCutPosition(Quaternion expected, int startIndex)
        {
            var index = startIndex;
            var result = Quaternion.One;

            while (index<TotalLength)
            {
                result = Multiply(result, GetQuaternionAt(index));

                if (result == expected)
                    yield return index + 1;

                index++;
            }
        }

        private long TotalLength
        {
            get { return Repetitions*Sequence.Length; }
        }

        private char GetCharAt(long i)
        {
            Debug.Assert(i < TotalLength);
            return Sequence[(int)(i % Sequence.Length)];
        }

        private Quaternion GetQuaternionAt(long i)
        {
            switch (GetCharAt(i))
            {
                case 'i':
                    return Quaternion.I;
                case 'j':
                    return Quaternion.J;
                case 'k':
                    return Quaternion.K;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static Quaternion Multiply(Quaternion left, Quaternion right)
        {
            return MulitplicationTable[left][right];
        }

        private static readonly Dictionary<Quaternion, Dictionary<Quaternion, Quaternion>> MulitplicationTable = new Dictionary<Quaternion, Dictionary<Quaternion, Quaternion>>()
        {
            {
                Quaternion.One,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.One},
                    {Quaternion.I, Quaternion.I},
                    {Quaternion.J, Quaternion.J},
                    {Quaternion.K, Quaternion.K},
                    {Quaternion.MinusOne, Quaternion.MinusOne},
                    {Quaternion.MinusI, Quaternion.MinusI},
                    {Quaternion.MinusJ, Quaternion.MinusJ},
                    {Quaternion.MinusK, Quaternion.MinusK}
                }
            },
            {
                Quaternion.I,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.I},
                    {Quaternion.I, Quaternion.MinusOne},
                    {Quaternion.J, Quaternion.K},
                    {Quaternion.K, Quaternion.MinusJ},
                    {Quaternion.MinusOne, Quaternion.MinusI},
                    {Quaternion.MinusI, Quaternion.One},
                    {Quaternion.MinusJ, Quaternion.MinusK},
                    {Quaternion.MinusK, Quaternion.J}
                }
            },
            {
                Quaternion.J,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.J},
                    {Quaternion.I, Quaternion.MinusK},
                    {Quaternion.J, Quaternion.MinusOne},
                    {Quaternion.K, Quaternion.I},
                    {Quaternion.MinusOne, Quaternion.MinusJ},
                    {Quaternion.MinusI, Quaternion.K},
                    {Quaternion.MinusJ, Quaternion.One},
                    {Quaternion.MinusK, Quaternion.MinusI}
                }
            },
            {
                Quaternion.K,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.K},
                    {Quaternion.I, Quaternion.J},
                    {Quaternion.J, Quaternion.MinusI},
                    {Quaternion.K, Quaternion.MinusOne},
                    {Quaternion.MinusOne, Quaternion.MinusK},
                    {Quaternion.MinusI, Quaternion.MinusJ},
                    {Quaternion.MinusJ, Quaternion.I},
                    {Quaternion.MinusK, Quaternion.One}
                }
            },
            {
                Quaternion.MinusOne,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.MinusOne},
                    {Quaternion.I, Quaternion.MinusI},
                    {Quaternion.J, Quaternion.MinusJ},
                    {Quaternion.K, Quaternion.MinusK},
                    {Quaternion.MinusOne, Quaternion.One},
                    {Quaternion.MinusI, Quaternion.I},
                    {Quaternion.MinusJ, Quaternion.J},
                    {Quaternion.MinusK, Quaternion.K}
                }
            },
            {
                Quaternion.MinusI,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.MinusI},
                    {Quaternion.I, Quaternion.One},
                    {Quaternion.J, Quaternion.MinusK},
                    {Quaternion.K, Quaternion.J},
                    {Quaternion.MinusOne, Quaternion.I},
                    {Quaternion.MinusI, Quaternion.MinusOne},
                    {Quaternion.MinusJ, Quaternion.K},
                    {Quaternion.MinusK, Quaternion.MinusJ}
                }
            },
            {
                Quaternion.MinusJ,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.MinusJ},
                    {Quaternion.I, Quaternion.K},
                    {Quaternion.J, Quaternion.One},
                    {Quaternion.K, Quaternion.MinusI},
                    {Quaternion.MinusOne, Quaternion.J},
                    {Quaternion.MinusI, Quaternion.MinusK},
                    {Quaternion.MinusJ, Quaternion.MinusOne},
                    {Quaternion.MinusK, Quaternion.I}
                }
            },
            {
                Quaternion.MinusK,
                new Dictionary<Quaternion, Quaternion>
                {
                    {Quaternion.One, Quaternion.MinusK},
                    {Quaternion.I, Quaternion.MinusJ},
                    {Quaternion.J, Quaternion.I},
                    {Quaternion.K, Quaternion.One},
                    {Quaternion.MinusOne, Quaternion.K},
                    {Quaternion.MinusI, Quaternion.J},
                    {Quaternion.MinusJ, Quaternion.MinusI},
                    {Quaternion.MinusK, Quaternion.MinusOne}
                }
            }
        };
    }
}
