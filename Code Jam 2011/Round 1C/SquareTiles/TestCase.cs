using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SquareTiles
{
    class TestCase
    {
        public char[,] Picture;

        public int R
        {
            get
            {
                return Picture.GetLength(0);
            }
        }

        public int C
        {
            get
            {
                return Picture.GetLength(1);
            }
        }

        public override string ToString()
        {
            return string.Format("R = {0}, C = {1}", R, C);
        }

        public object Solve()
        {
            var pic = (char[,])Picture.Clone();

            for (int row = 0; row < R - 1; row++)
            {
                for (int col = 0; col < C - 1; col++)
                {
                    if (pic[row, col] == '#' &&
                        pic[row + 1, col] == '#' &&
                        pic[row, col + 1] == '#' &&
                        pic[row + 1, col + 1] == '#')
                    {
                        pic[row, col] = '/';
                        pic[row + 1, col] = '\\';
                        pic[row, col + 1] = '\\';
                        pic[row + 1, col + 1] = '/';
                    }
                }
            }

            var s = new StringBuilder();

            for (int row = 0; row < R; row++)
            {
                s.AppendLine();

                for (int col = 0; col < C; col++)
                {
                    s.Append(pic[row, col]);
                }
            }

            var result = s.ToString();

            if (result.Contains('#'))
                return "\nImpossible";
            else
                return result;
        }
    }
}
