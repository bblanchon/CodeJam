using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach( var inputFileName in System.IO.Directory.EnumerateFiles(".", "*.in") )
            {
                var outputFileName = Path.ChangeExtension(inputFileName, ".out");

                Console.WriteLine("Solving {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);
                
                using( var outputFile = new OutputFile(outputFileName))
                {
                    foreach (var testCase in inputFile.TestCases)
                    {
                        Console.WriteLine("TestCase: {0}", testCase);

                        var mkdirCount = 0;
                        var root = new Directory();

                        foreach (var directory in testCase.ExistingDirectories)
                        {
                            Debug.Assert(!string.IsNullOrEmpty(directory));
                            Debug.Assert(directory[0] == '/');
                            root.Mkdir(directory.Substring(1));
                        }
                       
                        foreach (var directory in testCase.DirectoriesToCreate)
                        {
                            Debug.Assert(!string.IsNullOrEmpty(directory));
                            Debug.Assert(directory[0] == '/');

                            mkdirCount += root.Mkdir(directory.Substring(1));
                        }

                        var testResult = new TestResult(mkdirCount);

                        Console.WriteLine("TestResult: {0}", testResult);

                        outputFile.Add(testResult);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }

        static int CountCommonChars(string a, string b)
        {
            return a.Zip(b, (ai, bi) => ai == bi).TakeWhile(m => m).Count();
        }
    }
}
