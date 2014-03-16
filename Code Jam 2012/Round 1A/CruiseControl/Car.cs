using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl
{
    class Car
    {
        public Car(Lane lane, int speed, int pos)
        {
            InitialLane = lane;
            InitialPosition = pos;
            Speed = speed;
        }

        public Lane InitialLane { get; private set; }
        public int Speed { get; private set; }
        public int InitialPosition { get; private set; }
    }
}
