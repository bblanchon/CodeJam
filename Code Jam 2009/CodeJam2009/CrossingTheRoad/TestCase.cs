using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CrossingTheRoad
{
    class TestCase
    {
        public int N;
        public int M;
        public int[,] S;
        public int[,] W;
        public int[,] T;
        // TODO: add members

        public override string ToString()
        {
            return string.Format("N = {0}, M = {1}", N, M);
        }

        public object Solve()
        {
            var positions = new Position[2*N, 2*M];

            for (int i = 0; i < 2 * N; i++)
            {
                for( int j=0 ; j< 2*M; j++ )
                {
                    positions[i, j] = new Position();
                }
            }

            positions[0, 0].BestTime = 0;
            positions[0, 0].HasRecentlyChanged = true;

            var links = new List<Link> ();

            for (int n=0; n < N; n++)
            {
                for (int m = 0; m < M; m++)
                {
                    var eSLight = new TrafficLight { 
                        OnPeriod = S[n,m],
                        OffPeriod = W[n,m],
                        StartTime = T[n, m]
                    };

                    eSLight.End1 = positions[2 * n, 2 * m];
                    eSLight.End2 = positions[2 * n, 2 * m + 1];

                    var wSLight = (TrafficLight)eSLight.Clone();
                    wSLight.End1 = positions[2 * n + 1 , 2 * m];
                    wSLight.End2 = positions[2 * n + 1, 2 * m + 1];

                    var sWLight =  new TrafficLight { 
                        OnPeriod = W[n, m],
                        OffPeriod = S[n, m],
                        StartTime = T[n, m] + S[n, m]
                    };
                    sWLight.End1 = positions[2 * n, 2 * m];
                    sWLight.End2 = positions[2 * n + 1, 2 * m];

                    var nWLight = (TrafficLight)sWLight.Clone();
                    nWLight.End1 = positions[2 * n, 2 * m + 1];
                    nWLight.End2 = positions[2 * n + 1, 2 * m + 1];

                    links.Add(eSLight);
                    links.Add(sWLight);
                    links.Add(wSLight);
                    links.Add(nWLight);
                }
            }

            for (int n = 0; n < N-1; n++)
            {
                for (int m = 0; m < M-1; m++)
                {
                    var sBlock = new Block();
                    sBlock.End1 = positions[2 * n + 1, 2 * m + 1];
                    sBlock.End2 = positions[2 * n + 1, 2 * m + 2];

                    var wBlock = new Block();
                    sBlock.End1 = positions[2 * n + 1, 2 * m + 1];
                    sBlock.End2 = positions[2 * n + 2, 2 * m + 1];

                    links.Add(sBlock);
                    links.Add(wBlock);
                }
            }

            foreach (var link in links)
            {
                link.End1.Neighboors.Add(Tuple.Create(link, link.End2));
                link.End2.Neighboors.Add(Tuple.Create(link, link.End1));
            }

            bool changedSomething = true;

            while (changedSomething)
            {
                changedSomething = false;

                foreach (var pos in positions)
                {
                    if (pos.Process())
                        changedSomething = true;
                }
            }
            
            return positions[2 * N - 1, 2 * M - 1].BestTime;
        }

        abstract class Link
        {
            public Position End1;
            public Position End2;

            public abstract int GetTimeToCross (int t0);
        }

        class Block : Link
        {
            public override int GetTimeToCross(int t0)
            {
                return 2;
            }
        }

        class TrafficLight : Link
        {
            public int OnPeriod, OffPeriod, StartTime;

            public override int GetTimeToCross(int t0)
            {
                if (IsOn(t0) && IsOn(t0 + 1))                
                    return 1;
                else
                    return GetTimeToNextOn(t0) + 1;
            }

            int TotalPeriod
            {
                get { return OnPeriod + OffPeriod; }
            }

            int GetTimeToNextOn(int t0)
            {
                return TotalPeriod - ((t0 - StartTime) % TotalPeriod);
            }

            bool IsOn(int t0)
            {
                return (t0 - StartTime) % TotalPeriod < OnPeriod;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }

        class Position
        {
            public int BestTime = int.MaxValue;
            public bool HasRecentlyChanged;
            public List<Tuple<Link, Position>> Neighboors = new List<Tuple<Link,Position>>();

            public bool Process()
            {
                Debug.Assert(Neighboors.Count >= 2);
                Debug.Assert(Neighboors.Count <= 4);

                if (!HasRecentlyChanged) return false;

                bool changedSomething = false;

                foreach (var neighboor in Neighboors)
                {
                    int ttr = BestTime + neighboor.Item1.GetTimeToCross(BestTime);
                    if (ttr < neighboor.Item2.BestTime)
                    {
                        neighboor.Item2.HasRecentlyChanged = true;
                        neighboor.Item2.BestTime = ttr;
                        changedSomething = true;
                    }
                }

                HasRecentlyChanged = false;

                return changedSomething;
            }
        }
    }
}
