using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CruiseControl
{    
    class TestCase
    {
        public class CarState
        {
            readonly HashSet<CarState> carsOnOppositeLane = new HashSet<CarState>();
            readonly HashSet<CarState> carsOnSameLane = new HashSet<CarState>();

            public CarState(Lane initialLane)
            {
                CurrentLane = initialLane;
            }

            public Lane? CurrentLane { get; set; }

            public bool TryAddDependency(CarState other)
            {   
                if (carsOnOppositeLane.Count >= 2) return false;

                if (carsOnSameLane.Contains(other)) return false;

                if (other.CurrentLane.HasValue && !TrySetLane(other.CurrentLane.Value.Opposite()))
                    return false;

                carsOnOppositeLane.Add(other);
                foreach (var car in other.carsOnSameLane)
                {
                    if (car == this) return false;
                    car.carsOnOppositeLane.Add(car);
                    carsOnOppositeLane.Add(this);
                }

                foreach (var car in other.carsOnOppositeLane)
                {
                    if (car == this) continue;
                    carsOnSameLane.Add(car);
                    car.carsOnSameLane.Add(this);
                }

                return true;
            }

            public void RemoveDependency(CarState other)
            {
                carsOnOppositeLane.Remove(other);
                CleanUp();
            }

            private bool TrySetLane(Lane newLane)
            {
                if (CurrentLane.HasValue && CurrentLane.Value != newLane) return false;
                if (CurrentLane.HasValue && CurrentLane.Value == newLane) return true;

                CurrentLane = newLane;

                foreach (var dep in carsOnOppositeLane)
                {
                    if (!dep.TrySetLane(newLane.Opposite()))
                        return false;
                }

                return true;
            }

            public void CleanUp()
            {
                if (carsOnOppositeLane.Count == 0)
                {
                    CurrentLane = null;
                    foreach (var car in carsOnSameLane)
                        car.carsOnSameLane.Remove(this);
                    carsOnSameLane.Clear();
                }
            }
        }

        enum EventType
        {
            Leave,
            Enter         
        }

        struct Event
        {
            public Event(EventType type, Fuzzy time, int car1, int car2)
                : this()
            {
                Type = type;
                Time = time;
                Car1 = car1;
                Car2 = car2;
            }

            public EventType Type { get; private set; }
            public Fuzzy Time { get; private set; }
            public int Car1 { get; private set; }
            public int Car2 { get; private set; }
        }

        private Car[] cars;

        public TestCase(Car[] cars)
        {
            this.cars = cars;
        }

        public object Solve()
        {
            var states = GetInitialCarStates();

            var events = EnumerateEvents().OrderBy(x => x.Type).OrderBy(x => x.Time).ToArray();

            foreach (var e in events)
            {
#if TRACE
                for (var i = 0; i < states.Count; i++)
                {
                    Log(" car {0}, {1}", i, states[i].CurrentLane.HasValue ? states[i].CurrentLane.ToString() : "Undeterminated");
                }
                Log("{3}\t{0} {1} & {2}", e.Type, e.Car1, e.Car2, e.Time);
#endif

                var car1 = states[e.Car1];
                var car2 = states[e.Car2];

                if (e.Type == EventType.Enter)
                {
                    if (!TryMakeDependency(car1, car2))
                        return e.Time;
                }
                else
                {
                    BreakDependency(car1, car2);
                }
            }

            return "Possible";
        }

        bool TryMakeDependency(CarState car1, CarState car2)
        {
            return car1.TryAddDependency(car2) && car2.TryAddDependency(car1);
        }

        void BreakDependency(CarState car1, CarState car2)
        {
            car1.RemoveDependency(car2);
            car2.RemoveDependency(car1);
        }

        IReadOnlyList<CarState> GetInitialCarStates()
        {
            var states = new CarState[cars.Length];

            // create all
            for (int i = 0; i < cars.Length; i++)
                states[i] = new CarState(cars[i].InitialLane); 

            // make dependencies
            for (int i = 0; i < cars.Length; i++)
            {              
                for (int j = i+1; j < cars.Length; j++)
                {
                    if (AreOverlapping(cars[i].InitialPosition, cars[j].InitialPosition))
                    {
                        var ok = TryMakeDependency(states[i], states[j]);
                        Debug.Assert(ok);
                    }
                }
            }

            // ignore lane when no dependency
            for (int i = 0; i < cars.Length; i++)
                states[i].CleanUp();

            return states;
        }

        IEnumerable<Event> EnumerateEvents()
        {
            for (int i = 0; i < cars.Length; i++)
            {
                for( int j=i+1 ; j<cars.Length ; j++)
                {
                    // when the two cars starts overlapping
                    {
                        var enterTime = GetEnterTime(cars[i], cars[j]);

                        if (enterTime > 0)
                            yield return new Event(EventType.Enter, enterTime, i, j);
                    }

                    // when the two cars stops overlapping
                    {
                        var leaveTime = GetLeaveTime(cars[i], cars[j]);

                        if (leaveTime > 0)
                            yield return new Event(EventType.Leave, leaveTime, i, j);
                    }
                }
            }
        }

        void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        Fuzzy GetEnterTime(Car car1, Car car2)
        {
            if (car1.Speed == car2.Speed) return Fuzzy.NaN;
            if (car1.InitialPosition > car2.InitialPosition) Swap(ref car1, ref car2);            
            return (car1.InitialPosition - car2.InitialPosition + 5.0) / (car2.Speed - car1.Speed);
        }

        Fuzzy GetLeaveTime(Car car1, Car car2)
        {
            if (car1.Speed == car2.Speed) return Fuzzy.NaN;
            if (car1.InitialPosition > car2.InitialPosition) Swap(ref car1, ref car2);
            return (car1.InitialPosition - car2.InitialPosition - 5.0) / (car2.Speed - car1.Speed);
        }

        bool AreOverlapping(Fuzzy position1, Fuzzy position2)
        {
            return
                position1 >= position2 && position1 <= position2 + 5
                ||
                position1 + 5 >= position2 && position1 <= position2;
        }

        [Conditional("TRACE")]
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
