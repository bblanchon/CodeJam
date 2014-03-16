using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TicTacToeTomek
{
    class TestCase
    {
        readonly string[] grid;

        public TestCase(string[] grid)
        {
            this.grid = grid;

            Debug.Assert(grid.Length == 4);
            Debug.Assert(grid.All(row => row.Length == 4));
        }

        public object Solve()
        {
            bool oWon = false, xWon = false;

            for (var i = 0; i < 4; i++)
            {
                oWon |= WinsOn('O', GetRow(i));
                oWon |= WinsOn('O', GetColumn(i));
                xWon |= WinsOn('X', GetRow(i));
                xWon |= WinsOn('X', GetColumn(i));
            }

            oWon |= WinsOn('O', GetDiag1());
            oWon |= WinsOn('O', GetDiag2());
            xWon |= WinsOn('X', GetDiag1());
            xWon |= WinsOn('X', GetDiag2()); 

            Debug.Assert(!(oWon && xWon));

            if (xWon) return "X won";
            if (oWon) return "O won";

            var emptyCell = grid.Any(row => row.Contains('.'));

            return emptyCell ? "Game has not completed" : "Draw";
        }

        private bool WinsOn(char player, IEnumerable<char> cells)
        {
            Debug.Assert(cells.Count()==4);
            return cells.Count(x => x == player || x == 'T') >= 4;
        }

        private IEnumerable<char> GetRow(int row)
        {
            return Enumerable.Range(0, 4).Select(col => grid[row][col]);
        }

        private IEnumerable<char> GetColumn(int col)
        {
            return Enumerable.Range(0, 4).Select(row => grid[row][col]);
        }

        private IEnumerable<char> GetDiag1()
        {
            return Enumerable.Range(0, 4).Select(i => grid[i][i]);
        }

        private IEnumerable<char> GetDiag2()
        {
            return Enumerable.Range(0, 4).Select(i => grid[3-i][i]);
        }
    }
}
