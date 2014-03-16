using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace TestProject
{
    class Wire
    {
        public Wire(int a, int b)
        {
            Debug.Assert(a >= 1);
            Debug.Assert(a <= 10000);
            Debug.Assert(b >= 1);
            Debug.Assert(b <= 10000);

            A = a;
            B = b;
        }

        public readonly int A, B;

        public bool Intersects(Wire other)
        {
            Debug.Assert(other != null);

            return Math.Sign(A - other.A) != Math.Sign(B - other.B);
        }

        public override string ToString()
        {
            return string.Format("[{0}-{1}]", A, B);
        }
    }
}
