using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Lawnmower
{
    class TestCase
    {
        readonly int N;
        readonly int M;
        readonly int[,] grid;

        public TestCase(int[,] grid)
        {
            this.grid = grid;
            N = grid.GetLength(0);
            M = grid.GetLength(1);
        }

        public object Solve()
        {
            for (int row = 0; row < N; row++)
            {
                for (int col = 0; col < M; col++)
                {
                    var h = grid[row,col];
                    Debug.Assert(h >= 1);
                    Debug.Assert(h <= 100);

                    var canBeDoneByRow = GetRow(row).All(x => x <= h);
                    var canBeDoneByCol = GetColumn(col).All(x => x <= h);

                    if (!canBeDoneByCol && !canBeDoneByRow)
                        return "NO";
                }
            }

            return "YES";
        }

        IEnumerable<int> GetRow(int row)
        {
            return Enumerable.Range(0, M).Select(col => grid[row, col]);
        }

        IEnumerable<int> GetColumn(int col)
        {
            return Enumerable.Range(0, N).Select(row => grid[row, col]);
        }
    }
}
