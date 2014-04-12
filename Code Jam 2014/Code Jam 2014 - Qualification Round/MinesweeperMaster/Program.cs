// Copyright © Benoit Blanchon 2014
// All Rights Reserved
//#define TEST_MAP_FILL

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;


namespace MinesweeperMaster
{
    class Program
    {
        static void Main(string[] args)
        {
#if TEST_MAP_FILL
            for (int i = 0; i < 100; i++)
            {
                var map = new Map(10, 10);
                map.PlaceMines(i);
                Console.WriteLine();
                Console.WriteLine(map.Render());
            }

            return;
#endif

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
                        if (chrono.ElapsedMilliseconds > 1000)
                            Console.WriteLine(" -> {0}", chrono.Elapsed);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }
    }
}
