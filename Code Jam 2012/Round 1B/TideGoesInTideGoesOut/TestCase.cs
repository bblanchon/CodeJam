using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TideGoesInTideGoesOut
{
    class TestCase
    {
        private int H;
        private int N;
        private int M;
        readonly Cell[,] map;

        public TestCase(int H, int N, int M, int[][] ceilingMap, int[][] floorMap)
        {
            this.H = H;
            this.N = N;
            this.M = M;

            map = new Cell[N,M];          
            
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    map[i, j] = new Cell(ceilingMap[i][j], floorMap[i][j]);
                }
            }

            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < M; j++)
                {
                    var here = map[i, j];

                    var adjacents = new[]
                        {
                            // north
                            i > 0 ? map[i - 1, j] : null,
                            // south
                            i < N - 1 ? map[i + 1, j] : null,
                            // west
                            j > 0 ? map[i, j - 1] : null,
                            // east
                            j < M - 1 ? map[i, j + 1] : null
                        };

                    foreach (var dest in adjacents)
                    {
                        if (dest != null && IsMovingPossible(here, dest))
                            here.AdjacentCells.Add(dest);
                    }
                }
            }

            map[0, 0].TimeToGetThere = 0;
        }

        public static bool IsMovingPossible(Cell from, Cell to)
        {
            if (to.Ceiling - to.Floor < 50) return false;
            if (to.Ceiling - from.Floor < 50) return false;
            if (from.Ceiling - to.Floor < 50) return false;
            return true;
        }

        public double GetTimeToMove(Cell from, Cell to)
        {
            var h = H - 10 * from.TimeToGetThere;

            var timeToWait = to.Ceiling - h < 50 ? (50 + h - to.Ceiling) / 10 : 0;
            Debug.Assert(timeToWait >= 0);

            if( timeToWait == 0 && from.TimeToGetThere == 0)
                return 0;

            h = H - 10 * (from.TimeToGetThere + timeToWait);

            var timeToMove = h - from.Floor < 20 ? 10 : 1;

            return timeToWait + timeToMove;
        }

        public object Solve()
        {
            var remaingCells = map.Cast<Cell>().ToList();

            while (remaingCells.Count > 0)
            {
                remaingCells.Sort();

                var currentCell = remaingCells[remaingCells.Count - 1];
                remaingCells.RemoveAt(remaingCells.Count - 1);

                if (currentCell == map[N - 1, M - 1]) break;

                Debug.Assert(!double.IsInfinity(currentCell.TimeToGetThere));

                foreach (var adjCell in currentCell.AdjacentCells)
                {                    
                    var delay = GetTimeToMove(currentCell, adjCell);

                    if (adjCell.TimeToGetThere > currentCell.TimeToGetThere + delay)
                    {
                        Debug.Assert(remaingCells.Contains(adjCell));
                        adjCell.TimeToGetThere = currentCell.TimeToGetThere + delay;
                    }
                }
            }

            return map[N - 1, M - 1].TimeToGetThere;
        }
    }
}
