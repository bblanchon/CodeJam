using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl
{
    class LaneList
    {
        readonly Lane[] lanes;

        public LaneList(Car[] cars)
        {
            lanes = cars.Select(x => x.InitialLane).ToArray();
        }

        public bool TryUpdate(DependencyMatrix dependencies)
        {
            for (int car1 = 0; car1 < lanes.Length; car1++)
            {
                if (dependencies.IsIndependent(car1))
                {
                    lanes[car1] = Lane.Undeterminated;
                    continue;
                }

                foreach (var car2 in dependencies.GetCarsOnSameLane(car1))
                {
                    if (lanes[car2] == Lane.Undeterminated) continue;

                    if (lanes[car1] == Lane.Undeterminated)
                        lanes[car1] = lanes[car2];
                    else if (lanes[car1] != lanes[car2])
                        return false;
                }

                foreach (var car2 in dependencies.GetCarsOnOppositeLane(car1))
                {
                    if (lanes[car2] == Lane.Undeterminated) continue;

                    if (lanes[car1] == Lane.Undeterminated)
                        lanes[car1] = lanes[car2].Opposite();
                    else if (lanes[car1] == lanes[car2])
                        return false;
                }
            }

            return true;
        }
    }
}
