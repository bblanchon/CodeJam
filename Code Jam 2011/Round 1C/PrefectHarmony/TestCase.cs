using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PrefectHarmony
{
    class TestCase
    {
        public int[] Notes;
        public int L;
        public int H;

        public int N
        {
            get { return Notes.Length; }
        }

        public override string ToString()
        {
            return string.Format("N = {0}, L = {1}, H = {2}", N, L, H);
        }

        public object Solve()
        {
            Array.Sort(Notes);
            long currentLcm = 1;

            for (var index = -1; index < N; index++)
            {
                if( index>=0 ) currentLcm = LCM(currentLcm, Notes[index]);
                
                if( currentLcm > H ) break;

                //Debug.Assert(IsMultiOfAllPrevious(currentLcm, index));

                if( index+1 < N )
                {
                    var nextLcm = LCM(currentLcm, Notes[index+1]);

                    for( var multi=1 ; ; multi++ )
                    {
                        var proposition = currentLcm * multi;

                        if( proposition < L ) continue;
                        if( proposition > H ) break;
                        if (proposition > nextLcm) break;

                        if( IsDividerOfAllFollowings(proposition, index+1) )
                            return proposition;
                    }
                }
                else 
                {
                    if (IsInRange(currentLcm)) return currentLcm;
                }
            }

            return "NO";
        }

        bool IsInRange(long value)
        {
            return value >= L && value <= H;
        }

        bool IsDividerOfAllFollowings(long value, int index)
        {
            for (var i = index; i < N; i++)
                if (Notes[i] % value != 0)
                    return false;
            return true;
        }

        bool IsMultiOfAllPrevious(long value, int index)
        {
            for (var i = 0; i <= index; i++)
                if (value % Notes[i] != 0)
                    return false;
            return true;
        }

        // From http://www.blackwasp.co.uk/EuclidsAlgorithm.aspx
        static long GCD(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            return Math.Max(a, b);
        }

        static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
    }
}
