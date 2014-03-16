using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach( var inputFileName in Directory.EnumerateFiles(".", "*.in") )
            {
                var outputFileName = Path.ChangeExtension(inputFileName, ".out");

                Console.WriteLine("Solving {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);
                
                using( var outputFile = new OutputFile(outputFileName))
                {
                    foreach (var testCase in inputFile.TestCases)
                    {
                        Console.WriteLine("TestCase: {0}", testCase);

                        var pattern = testCase.Pattern;
                        pattern = pattern.Replace('(','[');
                        pattern = pattern.Replace(')',']');

                        var regex = new Regex(pattern, RegexOptions.Compiled);

                        var matchCount = inputFile.Dictionnary.Count(regex.IsMatch);

                        var testResult = new TestResult(matchCount);

                        Console.WriteLine("TestResult: {0}", testResult);

                        outputFile.Add(testResult);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }

        static long Factorial(long n)
        {
            if( n <= 1 ) return 1;
            else return n * Factorial(n - 1);
        }
    }
}
