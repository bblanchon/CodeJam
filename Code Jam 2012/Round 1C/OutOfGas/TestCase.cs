using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace OutOfGas
{
    class Position
    {
        public double X;
        public double Time;
        public double Speed;
    }

    class Car
    {
        public List<Position> KnownPositions = new List<Position>();
    }

    class MyCar : Car
    {
        public MyCar()
        {
            KnownPositions.Add(new Position
            {
                Time = 0,
                Speed = 0,
                X = 0
            });
        }

        public Position ComputePositionAtTime(double t, double acceleration)
        {            
            var lastKnown = KnownPositions.Last(x => x.Time <= t);
            var elapsedTime = t - lastKnown.Time;
            return new Position
            { 
                Time = t,
                Speed = lastKnown.Speed + elapsedTime * acceleration,
                X = lastKnown.X + lastKnown.Speed*elapsedTime + 0.5*acceleration*elapsedTime*elapsedTime,
            };
        }

        public double ComputeTimeToReach(double x, double acceleration)
        {
            var lastKnown = KnownPositions.Last(p => p.X <= x);

            var sol = ResolveSecondOrderEq(0.5 * acceleration, lastKnown.Speed, lastKnown.X-x);

            return sol.Single(s => s >= 0);
        }

        private IEnumerable<double> ResolveSecondOrderEq(double a, double b, double c)
        {
            double d = b * b - 4 * a * c;

            if (d == 0)            
                return new[] { -b / 2 * a };

            if (d > 0)
                return new[]
                    {
                        (-b - Math.Sqrt(d)) / 2 / a,
                        (-b + Math.Sqrt(d)) / 2 / a,
                    };

            return new double[0];
        }
    }

    class OtherCar : Car
    {
        public void FillKnownSpeeds()
        {
            Debug.Assert(KnownPositions.Count >= 2);

            for (var i = 0; i < KnownPositions.Count-1; i++)
            {
                var next = KnownPositions[i+1];
                var current = KnownPositions[i];
                current.Speed = (next.X - current.X) / (next.Time - current.Time);                     
            }

            KnownPositions[KnownPositions.Count - 1].Speed = KnownPositions[KnownPositions.Count - 2].Speed;
        }

        public Position ComputePositionAtTime(double t)
        {
            var exactPosition = KnownPositions.SingleOrDefault(x => Math.Abs(x.Time-t)<=double.Epsilon);
            if (exactPosition != null) return exactPosition;

            var previousPosition = KnownPositions.Last(x => x.Time <= t);
            var nextPosition = KnownPositions.First(x => x.Time >= t);

            var speed = (nextPosition.X - previousPosition.X) / (nextPosition.Time - previousPosition.Time);

            return new Position
            {
                Time = t,
                Speed = speed,
                X = previousPosition.X + speed * (t - previousPosition.Time ),
            };
        }

        public double ComputeTimeToReach(double x)
        {
            var previousPosition = KnownPositions.Last(p => p.X <= x);
            var nextPosition = KnownPositions.First(p => p.X >= x);
            var dx = nextPosition.X - previousPosition.X;
            var dt = nextPosition.Time - previousPosition.Time;

            if (dx == 0) return previousPosition.Time;

            return previousPosition.Time + (x-previousPosition.X)*dt/dx;
        }
    }

    class TestCase
    {
        public OtherCar OtherCar = new OtherCar();
        public List<double> Accelerations = new List<double>();
        public double Distance;

        public object Solve()
        {
            OtherCar.FillKnownSpeeds();

            return Environment.NewLine + string.Join(Environment.NewLine, Accelerations.Select(SolveAcceleration));
        }

        public double SolveAcceleration(double acceleration)
        {
            var myCar = new MyCar();

            foreach (var otherCarPos in OtherCar.KnownPositions.Skip(1))
            {
                var positionIfFree = myCar.ComputePositionAtTime(otherCarPos.Time, acceleration);
                if (positionIfFree.X >= otherCarPos.X)
                    myCar.KnownPositions.Add(otherCarPos);
            }

            var myTimeToReach = myCar.ComputeTimeToReach(Distance, acceleration);
            var hisTimeToReach = OtherCar.ComputeTimeToReach(Distance);

            return Math.Max(myTimeToReach, hisTimeToReach);
        }

        public override string ToString()
        {
            return string.Format("N={0} A={1}", OtherCar.KnownPositions.Count, Accelerations.Count); 
        }
    }
}
