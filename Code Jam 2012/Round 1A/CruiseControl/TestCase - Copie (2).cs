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
        enum Dependency
        {
            None,
            SameLane,
            OppositeLane
        }

        class DependencyMatrix
        {
            readonly int size;
            readonly Dependency[,] matrix;

            public DependencyMatrix(int carCount)
            {
                size = carCount;
                matrix = new Dependency[size, size];
                for (var i = 0; i < size; i++)
                    matrix[i, i] = Dependency.SameLane;
            }

            public bool TryAddOvertake(int car1, int car2)
            {
                Debug.Assert(car1 != car2);

                if (GetDependency(car1, car2) == Dependency.SameLane)
                    return false;

                Debug.Assert(GetDependency(car1, car2) == Dependency.None);

                for (int i = 0; i < size; i++)
                {
                    switch (GetDependency(car1, i))
                    {
                        case Dependency.OppositeLane:
                            if (GetDependency(i, car2) == Dependency.OppositeLane) return false;
                            SetDependency(i, car2, Dependency.SameLane);
                            break;

                        case Dependency.SameLane:                    
                            if (GetDependency(car2, i) == Dependency.SameLane) return false;
                            SetDependency(car2, i, Dependency.OppositeLane);
                            break;
                    }

                    switch (GetDependency(i, car2))
                    {
                        case Dependency.OppositeLane:
                            if (GetDependency(car1, i) == Dependency.OppositeLane) return false;
                            SetDependency(car1, i, Dependency.SameLane);
                            break;

                        case Dependency.SameLane:
                            if (GetDependency(car1, i) == Dependency.SameLane) return false;
                            SetDependency(car1, i, Dependency.OppositeLane);
                            break;
                    }
                }

                return true;
            }

            public void RemoveOvertake(int car1, int car2)
            {
                Debug.Assert(GetDependency(car1, car2) == Dependency.OppositeLane);
                SetDependency(car1, car2, Dependency.None);

                if (IsIndependent(car1)) ResetDependencies(car1);
                if (IsIndependent(car2)) ResetDependencies(car2);
            }

            public bool IsIndependent(int car1)
            {
                return !Enumerable.Range(0, size).Any(car2 => GetDependency(car1, car2) == Dependency.OppositeLane);
            }

            public IEnumerable<int> GetCarsOnSameLane(int car1)
            {
                return Enumerable.Range(0, size).Where(car2 => GetDependency(car1, car2) == Dependency.SameLane);
            }

            public IEnumerable<int> GetCarsOnOppositeLane(int car1)
            {
                return Enumerable.Range(0, size).Where(car2 => GetDependency(car1, car2) == Dependency.OppositeLane);
            }

            private void ResetDependencies(int car)
            {
                for (int i = 0; i < size; i++)
                    SetDependency(car, i, car!=i ? Dependency.None : Dependency.SameLane);
            }

            private Dependency GetDependency(int car1, int car2)
            {
                return matrix[car1, car2];
            }

            private void SetDependency(int car1, int car2, Dependency value)
            {
                Debug.Assert(car1 != car2 || value == Dependency.SameLane);
                matrix[car1, car2] = value;
                matrix[car2, car1] = value;
            }

            public override string ToString()
            {
                var symbols = new Dictionary<Dependency,char>
                {
                    { Dependency.None, ' ' },
                    { Dependency.SameLane, 'S' },
                    { Dependency.OppositeLane, 'O'}
                };

                var text = new StringBuilder();

                text.AppendFormat(" |");
                for (var col = 0; col < size; col++)
                    text.AppendFormat("{0}|", col);
                text.AppendLine();
                text.Append('-', size * 2 + 2);
                text.AppendLine();

                for (var row = 0; row < size; row++)
                {
                    text.AppendFormat("{0}|", row);

                    for (var col = 0; col < size; col++)
                    {                        
                        text.Append(symbols[GetDependency(row, col)]);
                        text.Append('|');
                    }

                    text.AppendLine();
                    text.Append('-', size*2+2);
                    text.AppendLine();
                }

                return text.ToString();
            }
        }


        private Car[] cars;

        public TestCase(Car[] cars)
        {
            this.cars = cars;
        }

        public object Solve()
        {
            var matrix = new DependencyMatrix(cars.Length);
            var lanes = cars.Select(x => x.InitialLane).ToArray();

            GetInitialCarDependencies(matrix);
            var ok = TryUpdateLaneList(matrix, lanes);
            Debug.Assert(ok);

            var events = EnumerateEvents().OrderBy(x => x.Type).OrderBy(x => x.Time);

            foreach (var e in events)
            {
#if TRACE
                Log("Dependencies:");
                Log("{0}", matrix);
                Log("Lanes:");
                for (var car = 0; car < lanes.Length; car++)
                    Log("{0} = {1}", car, lanes[car]);

                Log("At {3}, event {0} with {1} and {2}", e.Type, e.Car1, e.Car2, e.Time); 
#endif                             
                                

                if (e.Type == EventType.Enter)
                {
                    if (!matrix.TryAddOvertake(e.Car1, e.Car2))
                        return e.Time;
                }
                else
                {
                    matrix.RemoveOvertake(e.Car1, e.Car2);
                }

                if (!TryUpdateLaneList(matrix, lanes))
                    return e.Time;
            }

            return "Possible";
        }

        bool TryUpdateLaneList(DependencyMatrix dependencies, Lane[] lanes)
        {
            for (int car1 = 0; car1 < cars.Length; car1++)
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

        void GetInitialCarDependencies(DependencyMatrix matrix)
        {
            // make dependencies
            for (int i = 0; i < cars.Length; i++)
            {              
                for (int j = i+1; j < cars.Length; j++)
                {
                    if (AreOverlapping(cars[i].InitialPosition, cars[j].InitialPosition))
                    {
                        var ok = matrix.TryAddOvertake(i, j);
                        Debug.Assert(ok);
                    }
                }
            }
        }

        bool AreOverlapping(int position1, int position2)
        {
            if (position1 > position2) ToolBox.Swap(ref position1, ref position2);
            return position1 + 5 > position2;
        }

        /*
        bool AreOverlapping(Fuzzy position1, Fuzzy position2)
        {
            return
                position1 > position2 && position1 < position2 + 5
                ||
                position1 + 5 > position2 && position1 < position2;
        }

        bool AreTouching(Fuzzy position1, Fuzzy position2)
        {
            return
                position1 >= position2 && position1 <= position2 + 5
                ||
                position1 + 5 >= position2 && position1 <= position2;
        }*/

        #region Events

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

        IEnumerable<Event> EnumerateEvents()
        {
            for (int i = 0; i < cars.Length; i++)
            {
                for (int j = i + 1; j < cars.Length; j++)
                {
                    // when the two cars starts overlapping
                    {
                        var enterTime = GetEnterTime(cars[i], cars[j]);

                        if (enterTime.IsFinite && enterTime >= 0)
                        {
                            yield return new Event(EventType.Enter, enterTime, i, j);
                        }
                    }

                    // when the two cars stops overlapping
                    {
                        var leaveTime = GetLeaveTime(cars[i], cars[j]);

                        if (leaveTime.IsFinite && leaveTime >= 0)
                        {
                            yield return new Event(EventType.Leave, leaveTime, i, j);
                        }
                    }
                }
            }
        }


        Fuzzy GetTouchingTime(Car car1, Car car2, double offset)
        {
            if (car1.Speed < car2.Speed)
                ToolBox.Swap(ref car1, ref car2);

            var time = (car1.InitialPosition - car2.InitialPosition + offset) / (car2.Speed - car1.Speed);

            if (time == 0) return Fuzzy.Epsilon;

            return time;
        }

        Fuzzy GetEnterTime(Car car1, Car car2)
        {            
            return GetTouchingTime(car1, car2, +5);
        }

        Fuzzy GetLeaveTime(Car car1, Car car2)
        {
            return GetTouchingTime(car1, car2, -5);
        }

        #endregion

        #region Plot file

        public void WritePlotFile(TextWriter file)
        {
            file.WriteLine("set style fill transparent solid 0.5 noborder");
            for( int i=0 ; i<cars.Length ; i++ )
            {
                if( i==0 ) file.Write("plot ");
                else file.Write(", ");
                file.Write("'+' using 1:($1*{0}+{1}):($1*{0}+{1}+5) with filledcurves closed", cars[i].Speed, cars[i].InitialPosition);
            }
        }

        #endregion

        [Conditional("TRACE")]
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
