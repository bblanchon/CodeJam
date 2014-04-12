﻿using System.Collections.Generic;
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

            ShowCell(bestCellRow, bestCellCol);
        }

        void ShowCell(int row, int col)
        {
            var cell = cells[row, col];

            if (cell.IsVisible)return;
            
            cell.IsVisible = true;
            visibleCount++;

            if (cell.AdjacentMines == 0)
            {
                ShowNeighbors(row, col);
            }
        }

        void ShowNeighbors(int row, int col)
        {
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

        public bool CanWinInOneClick()
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

        public void PlaceMines(int mineCount)
        {
            AddMinesInBox(mineCount, 0, rows, 0, cols);
            
            Debug.Assert(this.mineCount == mineCount);
        }

        void AddMinesInBox(int mineCount, int boxRow, int boxRows, int boxCol, int boxCols)
        {
            Debug.Assert(mineCount >= 0);
            Debug.Assert(mineCount < boxRows * boxCols);

            if (mineCount == 0) return;

            var smallHalf = mineCount / 2;
            var bigHalf = mineCount - mineCount / 2;

            if (boxRows < boxCols)
            {
                if (mineCount >= boxRows)
                {
                    for (var i = 0; i < boxRows; i++)
                        SetAsMine(boxRow + i, boxCol);

                    AddMinesInBox(mineCount - boxRows, boxRow, boxRows, boxCol + 1, boxCols - 1);
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
                if (mineCount >= boxCols)
                {
                    for (var i = 0; i < boxCols; i++)
                        SetAsMine(boxRow, boxCol + i);

                    AddMinesInBox(mineCount - boxCols, boxRow + 1, boxRows - 1, boxCol, boxCols);
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