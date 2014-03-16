using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CruiseControl
{    
    class TestCase
    {      
        private Car[] cars;

        public TestCase(Car[] cars)
        {
            this.cars = cars;
        }

        public object Solve()
        {
            var dependencies = new DependencyMatrix(cars);
            var lanes = new LaneList(cars);

            var initialListIsOk = lanes.TryUpdate(dependencies);
            Debug.Assert(initialListIsOk);

            foreach (var e in Events.Enumerate(cars))         
            {
                Debug.Assert(e.Time >= 0);         

                if (e.Type == EventType.StartOverlapping)
                {
                    if (!dependencies.TryPutCarsOnOppositeLanes(e.Car1, e.Car2)) 
                        return e.Time;
                }

                if (e.Type == EventType.StopOverlapping)
                {
                    dependencies.RemoveCarsOnOppositeLanes(e.Car1, e.Car2);
                }

                if (!lanes.TryUpdate(dependencies))
                    return e.Time;
            }

            return "Possible";
        }            
    }
}
