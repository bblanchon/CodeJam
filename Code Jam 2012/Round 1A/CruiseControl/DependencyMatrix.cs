using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CruiseControl
{
    class DependencyMatrix
    {
        enum Dependency
        {
            None,
            SameLane,
            OppositeLane
        }

        readonly int size;
        readonly Dependency[,] matrix;

        public DependencyMatrix(IReadOnlyList<Car> cars)
        {
            size = cars.Count;
            matrix = new Dependency[size, size];

            // make initial dependencies
            for (int i = 0; i < cars.Count; i++)
            {
                matrix[i, i] = Dependency.SameLane;

                for (int j = i + 1; j < cars.Count; j++)
                {
                    var x1 = cars[i].InitialPosition;
                    var x2 = cars[j].InitialPosition;
                    var areOverlapping = x1 < x2 ? x1 + 5 > x2 : x2 + 5 > x1;

                    if (areOverlapping)
                    {
                        var ok = TryPutCarsOnOppositeLanes(i, j);
                        Debug.Assert(ok);
                    }
                }
            }
        }

        public bool TryPutCarsOnOppositeLanes(int car1, int car2)
        {
            Debug.Assert(car1 != car2);

            if (GetDependency(car1, car2) == Dependency.SameLane)
                return false;

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

        public void RemoveCarsOnOppositeLanes(int car1, int car2)
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
                SetDependency(car, i, car != i ? Dependency.None : Dependency.SameLane);
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
    }
}
