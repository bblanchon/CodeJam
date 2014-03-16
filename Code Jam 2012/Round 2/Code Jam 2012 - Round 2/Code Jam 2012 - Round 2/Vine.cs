using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwingingWild
{
    struct Vine
    {
        public Vine(int position, int length)
            : this()
        {
            Position = position;
            Length = length;
        }

        public int Position { get; private set; }
        public int Length { get; private set; }
    }
}
