using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Treasure
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            foreach (var inputFileName in Directory.EnumerateFiles(".", "*.in"))
            {
                var outputFileName = Path.ChangeExtension(inputFileName, ".out");

                Console.WriteLine("Solving {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);

                using (var outputFile = new OutputFile(outputFileName))
                {
                    foreach (var testCase in inputFile.TestCases)
                    {
                        var chrono = new Stopwatch();
                        chrono.Start();
                        outputFile.Add(testCase.Solve());
                        chrono.Stop();
                        if( chrono.ElapsedMilliseconds > 500 )
                            Console.WriteLine("({0})", chrono.Elapsed);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }
    }
}
