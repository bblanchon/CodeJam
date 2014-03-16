// Copyright © Benoit Blanchon 2013
// All Rights Reserved

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ManageYourEnergy
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
