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

            if (N == 1)
                Debug.Assert(bestTotalGain == (long)E * (long)values[0]);

#if DEBUG
            var chrono = new Stopwatch();

            chrono.Start();

            while (chrono.ElapsedMilliseconds < 500)
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
                x => new ActivityState(x, E)));

            while (true)
            {
                var current = states
                    .Where(x => x.AllocatedEnergy == null)
                    .OrderByDescending(x => x.Value)
                    .FirstOrDefault();

                if (current == null) break;

                Debug.Assert(current.EnergyBefore >= 0);
                Debug.Assert(current.EnergyBefore <= E);
                Debug.Assert(current.EnergyAfter >= 0);
                Debug.Assert(current.EnergyAfter <= E);
                Debug.Assert(current.EnergyAfter <= current.EnergyBefore);

                // take the max we can
                current.AllocatedEnergy = current.EnergyBefore - current.EnergyAfter;
               
                // propagate the change
                var node = states.Find(current);
                PropagateForward(node, current.EnergyAfter + R);
                PropagateBackward(node, current.EnergyBefore - R);
            }

            return states.Select(x => x.AllocatedEnergy.Value).ToArray();
        }

        private void PropagateForward(LinkedListNode<ActivityState> node, int availableEnergy)
        {
            if (availableEnergy >= E) return;

            var current = node.Value;

            if (node.Next != null)
            {
                var next = node.Next.Value;

                if (availableEnergy < next.EnergyBefore)
                {
                    Debug.Assert(next.AllocatedEnergy == null);
                    next.EnergyBefore = availableEnergy;
                    Debug.Assert(next.EnergyBefore >= next.EnergyAfter);                    
                }

                PropagateForward(node.Next, availableEnergy + R);
            }
        }

        private void PropagateBackward(LinkedListNode<ActivityState> node, int requiredEnergy)
        {
            if (requiredEnergy <= 0) return;

            var current = node.Value;

            if (node.Previous != null)
            {
                var previous = node.Previous.Value;

                if (requiredEnergy > previous.EnergyAfter)
                {
                    Debug.Assert(previous.AllocatedEnergy == null);
                    previous.EnergyAfter = requiredEnergy;
                    Debug.Assert(previous.EnergyBefore >= previous.EnergyAfter);                    
                }

                PropagateBackward(node.Previous, requiredEnergy - R);
            }
        }       

        class ActivityState
        {
            public ActivityState(int value, int e)
            {
                Value = value;
                EnergyBefore = e;
                EnergyAfter = 0;
            }

            public int Value { get; private set; }
            public int? AllocatedEnergy { get; set; }
            public int EnergyBefore { get; set; }
            public int EnergyAfter { get; set; }
        }
    }
}
