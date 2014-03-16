using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TideGoesInTideGoesOut
{
    class Cell : IComparable<Cell>
    {
        public Cell(int ceiling, int floor)
        {
            Floor = floor;
            Ceiling = ceiling;
            AdjacentCells = new List<Cell>();
            TimeToGetThere = double.PositiveInfinity;
        }

        public int Ceiling { get; private set; }
        public int Floor { get; private set; }

        public IList<Cell> AdjacentCells { get; private set; }

        public double TimeToGetThere { get; set; }

        public int CompareTo(Cell other)
        {
            // the closest cell should go at the end
            return -TimeToGetThere.CompareTo(other.TimeToGetThere);
        }
    }
}
