using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rotate
{
    class Table
    {
        readonly char[,] array;

        public Table(int width)
        {
            array = new char[width, width];
        }

        public char this[int row, int col]
        {
            set { array[row, col] = value; }
            get { return array[row, col]; }
        }

        public int Width
        {
            get { return array.GetLength(0); }
        }

        public Table ApplyGravityOnTheRight()
        {
            var t = new Table(Width);

            for (int row = 0; row < Width; row++)
            {
                int dstCol = Width-1;
                for (var srcCol = Width-1; srcCol >= 0; srcCol--)
                {
                    if (array[row, srcCol] != '.')
                        t.array[row, dstCol--] = array[row, srcCol];
                }
                while (dstCol >= 0)
                    t.array[row, dstCol--] = '.';
            }

            return t;
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            for (int row = 0; row < Width; row++)
            {
                for (int col = 0; col < Width; col++)
                {
                    s.Append(this[row, col]);
                }
                s.AppendLine();
            }
            return s.ToString();
        }
    }
}
