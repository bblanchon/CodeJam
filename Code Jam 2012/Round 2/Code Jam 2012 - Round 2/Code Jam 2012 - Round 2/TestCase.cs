using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SwingingWild
{
    class State : IEquatable<State>
    {
        public Vine Vine { get; private set; }
        public int Position { get; private set; }

        public State(int position, Vine vine)
        {
            Position = position;
            Vine = vine;
        }

        public override int GetHashCode()
        {
            return Vine.Position.GetHashCode() ^ Position.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return obj is State && Equals((State)obj);
        }

        public bool Equals(State other)
        {
            return Vine.Position == other.Vine.Position && Position == other.Position;
        }
    }

    class TestCase
    {
        private int D;
        private Vine[] vines;

        public TestCase(int D, Vine[] vines)
        {
            this.D = D;
            this.vines = vines;
        }

        public object Solve()
        {
            var scannedStates = new HashSet<State>();
            return CanWinFrom(scannedStates, new State(0, vines[0])) ? "YES" : "NO";
        }

        /*
        private bool CanWinFrom(int position, Vine vine)
        {
            Debug.Assert(position >= 0);
            Debug.Assert(position < D);

            ToolBox.Log("X={0}, vine={1},{2}", position, vine.Position, vine.Length);

            var radius = vine.Position - position;

            Debug.Assert(radius > 0);
            Debug.Assert(radius <= vine.Length);

            if (vine.Position + radius >= D) return true;

            foreach (var nextVine in vines)
            {
                if (nextVine.Position <= vine.Position) continue;
                if (nextVine.Position > vine.Position + radius) continue;

                Debug.Assert(nextVine.Position > vine.Position);

                var nextPosition = Math.Max(vine.Position, nextVine.Position - nextVine.Length);

                Debug.Assert(nextPosition > position);

                if (CanWinFrom(nextPosition, nextVine)) return true;
            }

            return false;
        }
*/
        /*
        private bool CanWinFrom(int position, Vine vine)
        {
            ToolBox.Log("X={0}, vine={1},{2}", position, vine.Position, vine.Length);

            var radius = vine.Position - position;

            Debug.Assert(radius > 0);
            Debug.Assert(radius <= vine.Length);

            if (vine.Position + radius >= D) return true;

            var reachableVines = vines.Where(x => x.Position > vine.Position && x.Position <= vine.Position + radius);

            if (!reachableVines.Any()) return false;

            var nextVine = reachableVines.OrderByDescending(x=>GetReachableDistanceAfterMoving(vine,x)).First();

            Debug.Assert(nextVine.Position > vine.Position);

            var nextPosition = GetPositionAfterMoving(vine, nextVine);

            Debug.Assert(nextPosition > position);

            return CanWinFrom(nextPosition, nextVine);
        }

        int GetPositionAfterMoving(Vine from, Vine to)
        {
            return Math.Max(from.Position, to.Position - to.Length);
        }

        int GetReachableDistanceAfterMoving(Vine from, Vine to)
        {
            var radius = to.Position - GetPositionAfterMoving(from, to);
            return to.Position + radius;
        }
         * */          

        private bool CanWinFrom(HashSet<State> scannedStates, State state)
        {
            if (scannedStates.Contains(state)) return false;
            scannedStates.Add(state);

            var radius = state.Vine.Position - state.Position;

            Debug.Assert(radius > 0);
            Debug.Assert(radius <= state.Vine.Length);

            if (state.Vine.Position + radius >= D) return true;

            foreach (var nextVine in vines)
            {
                if (nextVine.Position <= state.Vine.Position) continue;
                if (nextVine.Position > state.Vine.Position + radius) continue;

                Debug.Assert(nextVine.Position > state.Vine.Position);

                var nextPosition = Math.Max(state.Vine.Position, nextVine.Position - nextVine.Length);

                Debug.Assert(nextPosition > state.Position);

                if (CanWinFrom(scannedStates, new State(nextPosition, nextVine))) return true;
            }

            return false;
        }
    }
}
