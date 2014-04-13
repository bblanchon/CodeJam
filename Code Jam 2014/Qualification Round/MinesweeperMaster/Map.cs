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
        public bool IsVisible { get; set; }
        public bool IsClicked { get; set; }
    }

    class Map
    {
        readonly Cell[,] cells;
        readonly int rows;
        readonly int cols;
        int visibleCount;
        int mineCount;

        public Map(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;

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

            for (var row = 0; row < rows; row++)
            {
                sb.AppendLine();

                for (var col = 0; col < cols; col++)
                {
                    var cell = cells[row, col];

                    if (cell.IsMine)
                        sb.Append('*');
                    else if (cell.IsClicked)
                        sb.Append('c');
                    else
                        sb.Append('.');
                }
            }

            return sb.ToString();
        }

        public bool IsWon
        {
            get { return mineCount + visibleCount == cells.Length; }
        }

        public void Click()
        {
            var bestCellCol = 0;
            var bestCellRow = 0;
            var bestCellValue = 10;

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var cell = cells[row, col];

                    if (cell.IsMine) continue;

                    if (cell.AdjacentMines >= bestCellValue) continue;

                    bestCellRow = row;
                    bestCellCol = col;
                    bestCellValue = cell.AdjacentMines;
                }
            }

            cells[bestCellRow, bestCellCol].IsClicked = true;
            ShowCell(bestCellRow, bestCellCol);
        }

        void ShowCell(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);

            var cell = cells[row, col];

            if (cell.IsVisible) return;
            
            cell.IsVisible = true;
            visibleCount++;

            if (cell.AdjacentMines == 0)
            {
                ShowNeighbors(row, col);
            }
        }

        void ShowNeighbors(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);
            Debug.Assert(cells[row, col].IsVisible);
            Debug.Assert(cells[row, col].IsMine == false);

            var neighborRows = GetNeightborOnOneAxis(row, rows);
            var neighborCols = GetNeightborOnOneAxis(col, cols).ToArray();

            foreach (var neighborRow in neighborRows)
            {
                foreach (var neighborCol in neighborCols)
                {
                    ShowCell(neighborRow, neighborCol);
                }
            }
        }

        public void PlaceMines(int mines)
        {
            Debug.Assert(mines >= 0);

            PlaceMinesInBox(mines, 0, rows, 0, cols);
            
            Debug.Assert(mineCount == mines);
        }

        void PlaceMinesInBox(int mines, int boxRow, int boxRows, int boxCol, int boxCols)
        {
            Debug.Assert(mines >= 0);
            Debug.Assert(mines < boxRows * boxCols);
            Debug.Assert(boxRow >= 0);
            Debug.Assert(boxRow + boxRows <= rows);
            Debug.Assert(boxCol >= 0);
            Debug.Assert(boxCol + boxCols <= cols);

            if (mines == 0) return;

            var smallHalf = mines / 2;
            var bigHalf = mines - mines / 2;

            if (boxRows < boxCols)
            {
                if (mines >= boxRows)
                {
                    for (var i = 0; i < boxRows; i++)
                        SetAsMine(boxRow + i, boxCol);

                    PlaceMinesInBox(mines - boxRows, boxRow, boxRows, boxCol + 1, boxCols - 1);
                }
                else
                {
                    for (var i=0; i < bigHalf; i++)
                        SetAsMine(boxRow, boxCol + i);

                    for (var i = 0; i < smallHalf; i++)
                        SetAsMine(boxRow + boxRows - 1, boxCol + boxCols - 1 - i);
                }
            }
            else
            {
                if (mines >= boxCols)
                {
                    for (var i = 0; i < boxCols; i++)
                        SetAsMine(boxRow, boxCol + i);

                    PlaceMinesInBox(mines - boxCols, boxRow + 1, boxRows - 1, boxCol, boxCols);
                }
                else
                {
                    for (var i = 0; i < bigHalf; i++)
                        SetAsMine(boxRow + i, boxCol);

                    for (var i = 0; i < smallHalf; i++)
                        SetAsMine(boxRow + boxRows - 1 - i, boxCol + boxCols - 1);
                }
            }
        }

        public void SetAsMine(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);
            Debug.Assert(cells[row, col].IsMine == false);

            cells[row, col].IsMine = true;
            mineCount++;

            foreach (var neighbor in GetNeighbors(row, col))
                neighbor.AdjacentMines++;
        }

        IEnumerable<Cell> GetNeighbors(int row, int col)
        {
            Debug.Assert(row >= 0);
            Debug.Assert(row < rows);
            Debug.Assert(col >= 0);
            Debug.Assert(col < cols);

            var neighborRows = GetNeightborOnOneAxis(row, rows);
            var neighborCols = GetNeightborOnOneAxis(col, cols).ToArray();

            return
                from neighborRow in neighborRows
                from neighborCol in neighborCols
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
