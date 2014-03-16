using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Store_Credit
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach( var inputFileName in Directory.EnumerateFiles(".", "*.in") )
            {
                Console.WriteLine("Reading {0}...", inputFileName);
                var inputFile = new InputFile(inputFileName);
                Console.WriteLine("Reading {0}... done", inputFileName);

                var outputFile = new OutputFile(inputFile.TestCases.Count);

                foreach (var testCase in inputFile.TestCases)
                {
                    Console.WriteLine("TestCase: Credit={0}, Items.Count={1}",
                        testCase.Credit, string.Join(" ", testCase.Items.Count));

                    var found = false;

                    for (var index1 = 0; index1 < testCase.Items.Count - 1 && !found; index1++)
                    {
                        var item1 = testCase.Items[index1];

                        if (item1> testCase.Credit) continue;

                        for (var index2 = index1 + 1; index2 < testCase.Items.Count && !found; index2++)
                        {
                            var item2 = testCase.Items[index2];

                            if (item1 + item2 == testCase.Credit)
                            {
                                outputFile.Add(index1, index2);
                                found = true;

                                Console.WriteLine("Result: {0} + {1} = {2}",
                                    item1, item2, testCase.Credit);
                            }
                        }
                    }

                    if (!found)
                        Console.WriteLine("Solution not found !");
                }

                var outputFileName = Path.ChangeExtension(inputFileName, ".out");

                Console.WriteLine("Writing {0}...", outputFileName);
                outputFile.Save(outputFileName);
                Console.WriteLine("Writing {0}... done", outputFileName);
            }
        }
    }
}
