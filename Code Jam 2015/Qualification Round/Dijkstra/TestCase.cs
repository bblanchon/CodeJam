// Copyright © Benoit Blanchon 2015
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
            var secondCuts = EnumerateCutPositionForK().ToArray();

            foreach (var firstCut in EnumerateCutPositionForI())
            {
                foreach (var secondCut in secondCuts)
                {
                    if (MultiplayRange(firstCut, secondCut) == Quaternion.J)
                        return "YES";
                }  
            }

            return "NO";
        }

        private IEnumerable<long> EnumerateCutPositionForI()
        {
            return EnumerateCutPosition(Quaternion.I, 0, +1);
        }

        private IEnumerable<long> EnumerateCutPositionForK()
        {
            return EnumerateCutPosition(Quaternion.K, TotalLength-1, -1);
        }

        private IEnumerable<long> EnumerateCutPosition(Quaternion expected, long startIndex, int increment)
        {
            var index = startIndex;
            var result = Quaternion.One;

            while (index<TotalLength && index >= 0)
            {
                result = Multiply(result, GetQuaternionAt(index));
                index += increment;

                if (result == expected)
                    yield return index;
            }
        }

        private Quaternion MultiplayRange(long startIndex, long stopIndex)
        {
            var result = Quaternion.One;

            for (var i = startIndex; i < stopIndex; i++)
            {
                result = Multiply(result, GetQuaternionAt(i));
            }

            return result;
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
