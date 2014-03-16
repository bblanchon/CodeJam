using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RPI
{
    class TestCase
    {
        public string[] Schedule;

        public int N { get { return Schedule.Length; } }

        public override string ToString()
        {
            return "N = " + N;
        }

        public object Solve()
        {
            var results = new List<double>(N);

            for (var team = 0; team < N; team++)
            {
                var rpi = 0.25 * WP(team) + 0.50 * OWP(team) + 0.25*OOWP(team);
                results.Add(rpi);
            }

            return "\n" + string.Join("\n", results);
        }

        public double WP(int team)
        {
            return WP(Schedule[team]);
        }

        public double WP (string row)
        {
            double win = row.Count(x => x == '1');
            double fail = row.Count(x => x == '0');

            return win / ( win + fail );
        }

        public double OWP (int team)
        {
            double sum = 0;
            int count = 0;

            for( var opponent=0 ; opponent<N ; opponent++ )
            {
                if( Schedule[opponent][team] == '.' ) continue;

                sum += WP(Schedule[opponent].Remove(team,1));
                count++;
            }
            
            return sum / count;
        }

        public double OOWP(int team)
        {
            double sum = 0;
            int count = 0;

            for( var opponent=0 ; opponent<N ; opponent++ )
            {
                if (Schedule[opponent][team] == '.') continue;

                sum += OWP(opponent);
                count++;
            }

            return sum / count;
        }
    }
}
