using System;
using System.Diagnostics;

namespace SwingingWild
{
    static class ToolBox
    {
        [Conditional("TRACE")]
        public static void Log(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }
    }
}
