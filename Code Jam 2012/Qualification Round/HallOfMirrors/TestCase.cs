using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HallOfMirrors
{
    class TestCase
    {
        const double step = 0.5;
        readonly char[,] grid;
        readonly int maxDistance;
        readonly Position startPosition;

        public TestCase(char[,] grid, int maxDistance)
        {
            this.grid = grid;
            this.maxDistance = maxDistance;
            this.startPosition = FindStartPosition(grid);
        }

        [Conditional("TRACE")]
        static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public object Solve()
        {
            var potentialDirections = EnumeratePotentialDirections().ToArray();
            Log("Number potential direction = {0}", potentialDirections.Length);
            return potentialDirections.AsParallel().Count(SolveDirection);

        }

        public bool SolveDirection(Direction direction)
        {            
            var distance = new Fuzzy(0.0);
            var position = startPosition;

            Log("----");

            while (true)
            {
                Log("(X ; Y) = ({0} ; {1})", position.X, position.Y);
                Log("(dX ; dY) = ({0} ; {1})", direction.Dx, direction.Dy);

                distance += position.MoveAlong(direction, step, out position);

                if (distance > maxDistance)
                {
                    Log("Max distance reached");
                    return false;
                }

                if (position.Equals(startPosition))
                {
                    Log("Start position reached !!!");
                    return true;
                }                               
                                
                bool reflectionOnX, reflectionOnY;
                var xOnGrid = Fuzzy.Floor(position.X);
                var yOnGrid = Fuzzy.Floor(position.Y);

                Log("(X ; Y) = ({0} ; {1})", position.X, position.Y);

                var isOnHorizontalGrid = position.Y == yOnGrid;
                var isOnVerticalGrid = position.X == xOnGrid;

                Log("isOnHorizontalGrid = {0}", isOnHorizontalGrid);
                Log("isOnVerticalGrid = {0}", isOnVerticalGrid);

                if (isOnHorizontalGrid && isOnVerticalGrid)
                {
                    var mirrorOnUpperLeft = IsMirrorAt(position.X - step, position.Y - step);
                    var mirrorOnUpperRight = IsMirrorAt(position.X + step, position.Y - step);
                    var mirrorOnLowerLeft = IsMirrorAt(position.X - step, position.Y + step);
                    var mirrorOnLowerRight = IsMirrorAt(position.X + step, position.Y + step);

                    // we can't possibly be in the middle of 4 mirrors
                    Debug.Assert(!(mirrorOnUpperLeft&&mirrorOnUpperRight&&mirrorOnLowerLeft&&mirrorOnLowerRight));

                    reflectionOnX =
                        mirrorOnUpperLeft && mirrorOnLowerLeft ||
                        mirrorOnUpperRight && mirrorOnLowerRight;

                    reflectionOnY =
                        mirrorOnUpperLeft && mirrorOnUpperRight ||
                        mirrorOnLowerLeft && mirrorOnLowerRight;
                }
                else if (isOnHorizontalGrid)
                {
                    var mirrorAbove = IsMirrorAt(position.X, position.Y - step);
                    var mirrorBelow = IsMirrorAt(position.X, position.Y + step);

                    reflectionOnY = mirrorAbove || mirrorBelow;
                    reflectionOnX = false;
                }
                else if (isOnVerticalGrid)
                {
                    var mirrorOnLeft = IsMirrorAt(position.X - step, position.Y);
                    var mirrorOnRight = IsMirrorAt(position.X + step, position.Y);

                    reflectionOnX = mirrorOnLeft || mirrorOnRight;
                    reflectionOnY = false;
                }
                else if (IsMirrorAt(position.X, position.Y))
                {
                    Log("ray is destroyed");
                    return false;
                }
                else
                {
                    reflectionOnY = false;
                    reflectionOnX = false;
                }

                if (position.X <= 1 || position.X >= grid.GetLength(0) - 1) Debug.Assert(reflectionOnX);
                if (position.Y <= 1 || position.Y >= grid.GetLength(1) - 1) Debug.Assert(reflectionOnY);
                if (reflectionOnX) Log("reflection on Y");
                if (reflectionOnY) Log("reflection on Y");

                direction = new Direction(
                    direction.Dx * (reflectionOnX ? -1 : 1),
                    direction.Dy * (reflectionOnY ? -1 : 1));
            }
        }

        public bool IsOnGrid(Fuzzy x)
        {
            return x - Fuzzy.Round(x) == 0;
        }

        private bool IsMirrorAt(Fuzzy x, Fuzzy y)
        {
            return grid[Fuzzy.Floor(x), Fuzzy.Floor(y)] == '#';
        }

        private static Position FindStartPosition(char[,] grid)
        {            
            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (grid[x, y] == 'X')
                        return new Position(x + 0.5, y + 0.5);
                }
            }

            throw new InvalidOperationException();
        }

        private IEnumerable<Direction> EnumeratePotentialDirections()
        {
            var D2 = maxDistance*maxDistance;

            for (var x = -maxDistance; x <= maxDistance; x++)
            {
                for (var y = -maxDistance; y <= maxDistance; y++)
                {
                    if (x == 0 && y == 0) continue;
                    if (x * x + y * y > D2) continue;
                    if (!ToolBox.AreCoprime(x, y)) continue;

                    yield return new Direction(x, y);
                }
            }
        }
    }

    class Position
    {
        readonly Fuzzy x;
        readonly Fuzzy y;

        public Position(Fuzzy x, Fuzzy y)
        {
            this.x = x;
            this.y = y;
        }

        public Fuzzy MoveAlong(Direction direction, double step, out Position newPosition)
        {
            // simple case #1 : dx == 0
            if ( direction.Dx == 0 )
            {
                newPosition = new Position(x, y + step * Fuzzy.Sign(direction.Dy));
                return step;
            }

            // simple case #2 : dy == 0
            if ( direction.Dy == 0)
            {
                newPosition = new Position(x + step * Fuzzy.Sign(direction.Dx), y);
                return step;
            }

            var newXOnGrid = GetNextPositionOnGrid(x, direction.Dx, step);
            var newYOnGrid = GetNextPositionOnGrid(y, direction.Dy, step);

            var xMotion = newXOnGrid - x;
            var yMotion = newYOnGrid - y;

            Debug.Assert(Fuzzy.Abs(xMotion) > 0);
            Debug.Assert(Fuzzy.Abs(xMotion) <= 0.5);
            Debug.Assert(Fuzzy.Abs(yMotion) > 0);
            Debug.Assert(Fuzzy.Abs(yMotion) <= 0.5);

            // shorter motion on X axis
            if (xMotion / direction.Dx < yMotion / direction.Dy)
            {
                yMotion = xMotion * direction.Dy / direction.Dx;
                Debug.Assert(Fuzzy.Abs(yMotion) > 0);
                Debug.Assert(Fuzzy.Abs(yMotion) <= 0.5);
            }
            // shorter motion on Y axis
            else if (xMotion / direction.Dx > yMotion / direction.Dy)
            {
                xMotion = yMotion * direction.Dx / direction.Dy;
                Debug.Assert(Fuzzy.Abs(xMotion) > 0);
                Debug.Assert(Fuzzy.Abs(xMotion) <= 0.5);
            }

            Debug.Assert(yMotion / xMotion == direction.Dy / direction.Dx);

            newPosition = new Position(x + xMotion, y + yMotion);

            return Fuzzy.Sqrt(xMotion * xMotion + yMotion * yMotion);
        }

        private Fuzzy GetNextPositionOnGrid(Fuzzy position, Fuzzy direction, double step)
        {
            // positive directon
            if (direction > 0)
                return Fuzzy.Floor(position / step + 1) * step;

            // negative directon
            if (direction < 0)
                return Fuzzy.Ceiling(position / step - 1) * step;

            // null direction
            return position;
        }

        public Fuzzy X
        {
            get { return x; }
        }

        public Fuzzy Y
        {
            get { return y; }
        }

        public bool Equals(Position other)
        {
            return x == other.x && y == other.y;
        }
    }

    class Direction
    {
        readonly Fuzzy dx;
        readonly Fuzzy dy;

        public Direction(Fuzzy dx, Fuzzy dy)
        {
            // normalize the vector along the smallest axis

            if (Fuzzy.Abs(dx) > Fuzzy.Abs(dy))
            {
                this.dx = dy != 0 ? dx / Fuzzy.Abs(dy) : Fuzzy.Sign(dx);
                this.dy = Fuzzy.Sign(dy);
            }
            else
            {
                this.dx = Fuzzy.Sign(dx);
                this.dy = dx != 0 ? dy / Fuzzy.Abs(dx) : Fuzzy.Sign(dy);
            }            
        }

        public Fuzzy Dx
        {
            get { return dx; }
        }

        public Fuzzy Dy
        {
            get { return dy; }
        }
    }
}
