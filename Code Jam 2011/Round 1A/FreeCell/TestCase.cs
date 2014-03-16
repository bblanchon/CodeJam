using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FreeCell
{
    class TestCase
    {
        public long N, Pd, Pg;

        public object Solve()
        {
            if (Pg == 0 && Pd != 0) return "Broken";
            if (Pg == 100 && Pd != 100) return "Broken";

            for (var i = 1; i <= N; i++)
            {
                if ((i * Pd % 100) == 0)
                    return "Possible";
            }

            return "Broken";
        }

        public override string ToString()
        {
            return string.Format("N={0} Pd={1} Pg={2}", N, Pd, Pg);
        }
    }
}
