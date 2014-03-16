using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Globalization;

namespace Lawnmower
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
                        outputFile.Add(testCase.Solve());
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }
    }
}
