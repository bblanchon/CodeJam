// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace FallingDiamond
{
    class TestCase
    {
        // TODO: add members

        public object Solve()
        {

        }

        Dictionary<Grid, double> AddOne(Dictionary<Grid, double> state)
        {
            var newState = new Dictionary<Grid, double>();

            foreach (var kvp in state)
            {
                var grid = kvp.Key;
                var top = new Position(0, 0);
                while (grid.OccupiedPositions.Contains(top))
                {
                    top = top.MoveUp();
                }

                var left = top.MoveLeft();
                var right = top.MoveRight();

                if (grid.OccupiedPositions.Contains(left) && grid.OccupiedPositions.Contains(right))
                {
                    var newPoint = top;

                    var newGrid = new Grid(grid.OccupiedPositions);
                    newGrid.OccupiedPositions.Add(newPoint);

                    if (newState.ContainsKey(newGrid))
                        newState[newGrid] += kvp.Value;
                    else
                        newState[newGrid] = kvp.Value;
                }
                else if (grid.OccupiedPositions.Contains(left) )
                {
                    var newPoint = right;

                    while (!grid.OccupiedPositions.Contains(newPoint))
                        newPoint = newPoint.MoveRight();

                    var newGrid = new Grid(grid.OccupiedPositions);
                    newGrid.OccupiedPositions.Add(newPoint);

                    if (newState.ContainsKey(newGrid))
                        newState[newGrid] += kvp.Value;
                    else
                        newState[newGrid] = kvp.Value;
                }
                else if (grid.OccupiedPositions.Contains(right))
                {
                    var newPoint = left;

                    while (!grid.OccupiedPositions.Contains(newPoint))
                        newPoint = newPoint.MoveLeft();

                    var newGrid = new Grid(grid.OccupiedPositions);
                    newGrid.OccupiedPositions.Add(newPoint);

                    if (newState.ContainsKey(newGrid))
                        newState[newGrid] += kvp.Value;
                    else
                        newState[newGrid] = kvp.Value;
                }
                else
                {
                    {
                        var newPoint = right;

                        while (!grid.OccupiedPositions.Contains(newPoint))
                            newPoint = newPoint.MoveRight();

                        var newGrid = new Grid(grid.OccupiedPositions);
                        newGrid.OccupiedPositions.Add(newPoint);

                        if (newState.ContainsKey(newGrid))
                            newState[newGrid] += kvp.Value* 0.5;
                        else
                            newState[newGrid] = kvp.Value *0.5;
                    }

                    {
                        var newPoint = left;

                        while (!grid.OccupiedPositions.Contains(newPoint))
                            newPoint = newPoint.MoveLeft();

                        var newGrid = new Grid(grid.OccupiedPositions);
                        newGrid.OccupiedPositions.Add(newPoint);

                        if (newState.ContainsKey(newGrid))
                            newState[newGrid] += kvp.Value*0.5;
                        else
                            newState[newGrid] = kvp.Value*0.5;
                    }
                }
            }
        }
    }


    class Grid : IEquatable<Grid>
    {
        public IList<Position> OccupiedPositions { get; private set; }

        public Grid(IEnumerable<Position> occupiedPositions)
        {
            OccupiedPositions = new List<Position>(occupiedPositions);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Grid);
        }

        public bool Equals(Grid other)
        {
            if (other == null) return false;

            return OccupiedPositions.SequenceEqual(other.OccupiedPositions);
        }

        public override int GetHashCode()
        {
            return OccupiedPositions.Select(x => x.GetHashCode()).Aggregate((x, y) => x ^ y);
        }
    }

    struct Position
    {
        public Position(int x, int y)
        {
            Debug.Assert((x + y) % 2 == 0);

            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }

        public override int GetHashCode()
        {
            return Y * 30000 + X;
        }

        public Position MoveUp()
        {
            return new Position(X, Y + 1);
        }

        public Position MoveLeft()
        {
            return new Position(X-1, Y - 1);
        }

        public Position MoveRight()
        {
            return new Position(X + 1, Y -1);
        }
    }
}
