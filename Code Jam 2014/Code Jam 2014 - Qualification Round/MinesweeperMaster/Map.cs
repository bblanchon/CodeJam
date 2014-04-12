using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MinesweeperMaster
{
    class Cell
    {
        public bool IsMine { get; set; }
        public int AdjacentMines { get; set; }
    }

    class Map
    {
        readonly Cell[,] cells;
        readonly int rows;
        readonly int cols;
        readonly int perimeter;
        int mineCount;

        public Map(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;

            perimeter = rows*2 + cols*2 - 4;

            cells = new Cell[rows, cols];

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    cells[row, col] = new Cell();
                }
            }
        }

        public string Render()
        {
            var sb = new StringBuilder();
            var mustClickZero = true;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var cell = cells[row, col];

                    if (cell.IsMine)
                    {
                        sb.Append('*');
                    }
                    else if (cell.AdjacentMines == 0 && mustClickZero)
                    {
                        sb.Append('c');
                        mustClickZero = false;
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public bool CanWinInOnClick()
        {
            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var cell = cells[row, col];

                    if (cell.IsMine) continue;
                    if (cell.AdjacentMines == 0) continue;

                    if (!IsTouchingAZero(row, col)) 
                        return false;
                }
            }

            return true;
        }

        bool IsTouchingAZero(int row, int col)
        {
            return
                GetNeighbors(row, col)
                    .Where(x => !x.IsMine)
                    .Any(x => x.AdjacentMines == 0);
        }

        public void AddMineAtFavorablePlace()
        {
            if (mineCount < perimeter)
                AddMineInPerimeter();
            else
                AddMineAtHottestCell();
        }

        void AddMineInPerimeter()
        {
            var indexInTopBorder = mineCount;
            var indexInRightBorder = indexInTopBorder - cols + 1;
            var indexInBottomBorder = indexInRightBorder - rows + 1;
            var indexInLeftBorder = indexInBottomBorder - cols + 1;
            
            if (indexInTopBorder < cols)
                SetAsMine(0, indexInTopBorder);
            else if (indexInRightBorder < rows)
                SetAsMine(indexInRightBorder, cols-1);
            else if (indexInBottomBorder < cols)
                SetAsMine(rows-1, cols-1-indexInBottomBorder);
            else if (indexInLeftBorder < rows)
                SetAsMine(rows-1-indexInLeftBorder, 0);
            else
                Debug.Fail("Bug in AddMineInPerimeter()");
        }

        void AddMineAtHottestCell()
        {
            var hottestCellRow = 0;
            var hottestCellCol = 0;
            var hottestCellValue = 0;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var cell = cells[row, col];
                    
                    if (cell.IsMine) continue;
                    if (cell.AdjacentMines <= hottestCellValue) continue;

                    hottestCellRow = row;
                    hottestCellCol = col;
                    hottestCellValue = cell.AdjacentMines;
                }
            }

            SetAsMine(hottestCellRow, hottestCellCol);
        }

        public void SetAsMine(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);

            cells[row, col].IsMine = true;

            foreach (var neighbor in GetNeighbors(row, col))
                neighbor.AdjacentMines++;

            mineCount++;
        }

        IEnumerable<Cell> GetNeighbors(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);

            var rowIncs = GetNeightborOnOneAxis(row, rows);
            var colIncs = GetNeightborOnOneAxis(col, cols).ToArray();

            return 
                from neighborRow in rowIncs 
                from neighborCol in colIncs
                where neighborRow != row || neighborCol != col
                select cells[neighborRow, neighborCol];
        }

        IEnumerable<int> GetNeightborOnOneAxis(int position, int count)
        {
            if (position > 0) yield return position - 1;
            yield return position;
            if (position < count - 1) yield return position + 1;
        }
    }
}
