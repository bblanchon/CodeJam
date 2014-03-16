using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace SpeakingInTongues
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var inputFileName in System.IO.Directory.EnumerateFiles(".", "*.in"))
            {
                var outputFileName = Path.ChangeExtension(inputFileName, ".out");

                Console.WriteLine("Solving {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);

                using (var outputFile = new OutputFile(outputFileName))
                {
                    foreach (var testCase in inputFile.TestCases)
                    {
                        Console.WriteLine("TestCase: {0}", testCase);

                        var testResult = testCase.Solve();

                        Console.WriteLine("TestResult: {0}", testResult);

                        outputFile.Add(testResult);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }
    }
}
