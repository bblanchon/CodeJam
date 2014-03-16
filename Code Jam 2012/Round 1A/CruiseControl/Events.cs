using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl
{
    enum EventType
    {
        // caution: order matters
        StopOverlapping = 0,
        StartOverlapping = 1
    }

    struct Event
    {
        public Event(EventType type, Fraction time, int car1, int car2)
            : this()
        {
            Type = type;
            Time = time;
            Car1 = car1;
            Car2 = car2;
        }

        public EventType Type { get; private set; }
        public Fraction Time { get; private set; }
        public int Car1 { get; private set; }
        public int Car2 { get; private set; }
    }

    static class Events
    {
        public static IEnumerable<Event> Enumerate(IReadOnlyList<Car> cars)
        {
            return EnumerateUnordered(cars).OrderBy(x => x.Type).OrderBy(x => x.Time);
        }
        
        static IEnumerable<Event> EnumerateUnordered(IReadOnlyList<Car> cars)
        {
            for (int i = 0; i < cars.Count; i++)
            {
                for (int j = i + 1; j < cars.Count; j++)
                {
                    // when the two cars starts overlapping
                    {
                        var startTime = GetOverlappingStartTime(cars[i], cars[j]);

                        if (startTime.IsFinite && startTime >= 0)
                        {
                            yield return new Event(EventType.StartOverlapping, startTime, i, j);
                        }
                    }

                    // when the two cars stops overlapping
                    {
                        var stopTime = GetOverlappingStop(cars[i], cars[j]);

                        if (stopTime.IsFinite && stopTime > 0)
                        {
                            yield return new Event(EventType.StopOverlapping, stopTime, i, j);
                        }
                    }
                }
            }
        }        

        static Fraction GetTouchingTime(Car car1, Car car2, int offset)
        {
            if (car1.Speed < car2.Speed)
                ToolBox.Swap(ref car1, ref car2);

            return new Fraction(car1.InitialPosition - car2.InitialPosition + offset, car2.Speed - car1.Speed);
        }

        static Fraction GetOverlappingStartTime(Car car1, Car car2)
        {            
            return GetTouchingTime(car1, car2, +5);
        }

        static Fraction GetOverlappingStop(Car car1, Car car2)
        {
            return GetTouchingTime(car1, car2, -5);
        }
    }
}
