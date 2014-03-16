using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace BotTrust
{
    class Button
    {
        public char Robot;
        public int Position;

        public override string ToString()
        {
            return string.Format("{0} {1}", Robot, Position);
        }
    }

    class TestCase
    {
        public List<Button> Sequence;

        public override string ToString()
        {
            return string.Join(", ", Sequence);
        }

        int completed;
        Dictionary<char, int> robotPositions;

        public object Solve()
        {
            robotPositions = new Dictionary<char, int>
            {
                { 'O', 1 },
                { 'B', 1 },
            };
            completed = 0;

            int time = 0;

            while (completed < Sequence.Count)
            {
                var o = MakeOneStep('O');
                var b = MakeOneStep('B');
                if (o || b)
                {
                    Debug.Assert(o != b);                    
                    completed++;
                }
                time++;
            }

            return time;
        }

        public bool MakeOneStep(char robot)
        {
            var robotPosition = robotPositions[robot];
            var currentButton = Sequence[completed];
            var log = false;

            Debug.Assert(robotPosition >= 1);
            Debug.Assert(robotPosition <= 100);

            if (log) Console.Write("Robot {0}: ", robot);

            if (currentButton.Robot == robot)
            {
                if (currentButton.Position == robotPosition)
                {
                    if (log) Console.WriteLine("PUSH");
                    return true;
                }
                else if (currentButton.Position > robotPosition)
                {
                    if (log) Console.WriteLine("Move +1");
                    robotPositions[robot] = robotPosition + 1;
                    return false;
                }
                else
                {
                    if (log) Console.WriteLine("Move -1");
                    robotPositions[robot] = robotPosition - 1;
                    return false;
                }
            }
            else
            {
                var nextButton = Sequence.Skip(completed).FirstOrDefault(x => x.Robot == robot);

                if (nextButton == null)
                {
                    if (log) Console.WriteLine("No more button for me");
                    return false;
                }
                else if (nextButton.Position > robotPosition)
                {
                    if (log) Console.WriteLine("Move +1");
                    robotPositions[robot] = robotPosition + 1;
                    return false;
                }
                else if (nextButton.Position < robotPosition)
                {
                    if (log) Console.WriteLine("Move -1");
                    robotPositions[robot] = robotPosition - 1;
                    return false;
                }
                else
                {
                    if (log) Console.WriteLine("Wait");
                    return false;
                }
            }
        }
    }
}
