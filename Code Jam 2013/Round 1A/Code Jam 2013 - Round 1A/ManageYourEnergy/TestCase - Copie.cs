// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;

namespace ManageYourEnergy
{
    class TestCase
    {
        readonly int E, R;
        readonly int[] values;
        readonly int N;

        Random rand;

        public TestCase(int e, int r, IEnumerable<int> v)
        {
            E = e;
            R = r;
            values = v.ToArray();
            N = values.Length;

            rand = new Random();
        }

        public object Solve()
        {
            var best = FindBestSequence();
            var bestTotalGain = ComputeTotalGain(best);

#if DEBUG
            var chrono = new Stopwatch();

            chrono.Start();

            while (chrono.ElapsedMilliseconds < 900)
            {
                var randomSequence = CreateRandomSequence();
                var randomTotalGain = ComputeTotalGain(randomSequence);

                Debug.Assert(bestTotalGain >= randomTotalGain);
            }
#endif

            return bestTotalGain;
        }

        private int[] CreateRandomSequence()
        {
            var currentEnergy = E;
            var sequence = new int[N];

            for (var i = 0; i < N; i++)
            {
                if (currentEnergy > 0)
                {
                    sequence[i] = rand.Next((int)currentEnergy + 1);
                    currentEnergy -= sequence[i];
                }
                else
                {
                    sequence[i] = 0;
                }

                Debug.Assert(currentEnergy >= 0);
                Debug.Assert(currentEnergy <= E);
                currentEnergy = Math.Min(E, currentEnergy + R);
            }

            return sequence;
        }

        private long ComputeTotalGain(int[] sequence)
        {
            Debug.Assert(sequence.Length == values.Length);

            var currentEnergy = E;
            var totalGain = 0L;

            for (var i = 0; i < N; i++)
            {
                Debug.Assert(sequence[i] >= 0);
                Debug.Assert(sequence[i] <= currentEnergy);

                totalGain += (long)sequence[i] * (long)values[i];

                currentEnergy -= sequence[i];
                Debug.Assert(currentEnergy >= 0);
                Debug.Assert(currentEnergy <= E);

                currentEnergy = Math.Min(E, currentEnergy + R);
            }

            return totalGain;
        }

        private int[] FindBestSequence()
        {
            var states = new LinkedList<ActivityState>(values.Select(
                x => new ActivityState(x, E, 0)));

            while (true)
            {
                var biggestUnset = states
                    .Where(x => x.AllocatedEnergy < x.AvailableEnergy)
                    .OrderByDescending(x => x.Value)
                    .FirstOrDefault();

                if (biggestUnset == null) break;

                // take the max we can
                biggestUnset.AllocatedEnergy = biggestUnset.AvailableEnergy;

                // propagate the change
                var node = states.Find(biggestUnset);
                PropagateConstrainBackward(node);
                PropagateConstrainForward(node);
            }

            Debug.Assert(states.All(x=>x.AllocatedEnergy == x.AvailableEnergy));

            return states.Select(x => x.AllocatedEnergy).ToArray();
        }

        private void PropagateConstrainForward(LinkedListNode<ActivityState> node)
        {
            Debug.Assert(node != null);

            if (node.Next == null) return;

            var current = node.Value;
            var next = node.Next.Value;

            Debug.Assert(current != null);
            Debug.Assert(next != null);
            Debug.Assert(current.AllocatedEnergy >= 0);
            Debug.Assert(current.AvailableEnergy <= E);
            Debug.Assert(current.AllocatedEnergy <= current.AvailableEnergy);

            var available = Math.Min(current.AvailableEnergy - current.AllocatedEnergy + R, E);

            Debug.Assert(available <= next.AvailableEnergy);
            Debug.Assert(next.AllocatedEnergy <= available);

            if (next.AvailableEnergy > available)
            {
                next.AvailableEnergy = available;
                PropagateConstrainForward(node.Next);
            }
        }

        private void PropagateConstrainBackward(LinkedListNode<ActivityState> node)
        {
            Debug.Assert(node != null);

            if (node.Previous == null) return;

            var current = node.Value;
            var previous = node.Previous.Value;

            Debug.Assert(current != null);
            Debug.Assert(previous != null);
            Debug.Assert(current.AllocatedEnergy >= 0);
            Debug.Assert(current.AvailableEnergy <= E);
            Debug.Assert(current.AllocatedEnergy <= current.AvailableEnergy);

            Debug.Assert(previous.AllocatedEnergy == 0);
            var available = Math.Min(previous.AvailableEnergy - current.AllocatedEnergy + R, E);

            Debug.Assert(available <= previous.AvailableEnergy);
            Debug.Assert(previous.AllocatedEnergy <= available);

            if (previous.AvailableEnergy > available)
            {
                previous.AvailableEnergy = available;
                PropagateConstrainBackward(node.Previous);
            }
        }

        class ActivityState
        {
            public ActivityState(int value, int available, int allocated)
            {
                Value = value;
                AvailableEnergy = available;
                AllocatedEnergy = allocated;
            }

            public int Value { get; private set; }
            public int AvailableEnergy { get; set; }
            public int AllocatedEnergy { get; set; }
        }
    }
}
