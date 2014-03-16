using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;
using System.Security.Cryptography;

namespace KillerWord
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var inputFileName in System.IO.Directory.EnumerateFiles(".", "*.in"))
            {
                var outputFileNamePattern = Path.ChangeExtension(inputFileName, ".{0}.out");
                string outputFileName = null;
                var hashes = new List<string>();

                Console.WriteLine("Solving {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);

                for (var i = 1; i < 100; i++)
                {
                    outputFileName = string.Format(outputFileNamePattern, i);
                    if (!File.Exists(outputFileName)) break;                  
                    hashes.Add(BitConverter.ToString(MD5.Create().ComputeHash(File.ReadAllBytes(outputFileName))));
                }

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
                Console.WriteLine("Result written in {0}", outputFileName);

                var myHash = BitConverter.ToString(MD5.Create().ComputeHash(File.ReadAllBytes(outputFileName)));
                if (hashes.Contains(myHash))
                    Console.WriteLine("CAUTION: already had this output in a previous run");
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }
    }
}
