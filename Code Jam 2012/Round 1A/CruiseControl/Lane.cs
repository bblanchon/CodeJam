using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl
{
    enum Lane
    {
        Undeterminated,
        Left,
        Right
    }

    static class LaneExtensions
    {
        public static Lane Opposite(this Lane lane)
        {
            return 
                lane == Lane.Right ? Lane.Left : 
                lane == Lane.Left ? Lane.Right :
                Lane.Undeterminated;
        }
    }
}
