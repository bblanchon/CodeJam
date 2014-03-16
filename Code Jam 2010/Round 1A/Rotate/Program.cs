using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.IO;

namespace Rotate
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

                        //Console.WriteLine("testCase.Table:\n {0}", testCase.Table);

                        var tableWithGravity = testCase.Table.ApplyGravityOnTheRight();

                        //Console.WriteLine("tableWithGravity:\n {0}", tableWithGravity);
                        
                        bool rWins = IsWinner(tableWithGravity, 'R', testCase.K);
                        bool bWins = IsWinner(tableWithGravity, 'B', testCase.K);

                        var testResult =
                            (bWins && rWins) ? "Both" :
                            bWins ? "Blue" :
                            rWins ? "Red" :
                            "Neither";

                        Console.WriteLine("TestResult: {0}", testResult);

                        outputFile.Add(testResult);
                    }
                }
                Console.WriteLine("Solving {0}... done", inputFileName);
            }

            Console.WriteLine("-- PRESS ENTER TO CLOSE --");
            Console.ReadLine();
        }

        static bool IsWinner(Table table, char player, int K)
        {
            for (var row = 0; row < table.Width; row++)
            {
                for (var col = 0; col < table.Width; col++)
                {
                    if (IsWinnerAtPoint(table, player, K, col, row, 0, 1)) return true;
                    if (IsWinnerAtPoint(table, player, K, col, row, 1, 0)) return true;
                    if (IsWinnerAtPoint(table, player, K, col, row, 1, 1)) return true;
                    if (IsWinnerAtPoint(table, player, K, col, row, -1, 1)) return true; 
                }
            }
            return false;
        }

        static bool IsWinnerAtPoint(Table table, char player, int K, int col, int row, int colInc, int rowInc)
        {
            if (table[col, row] == player)
            {
                if (K == 1) return true;

                col += colInc;
                if (col < 0 || col >= table.Width) return false;

                row += rowInc;
                if (row < 0 || row >= table.Width) return false;
                
                return IsWinnerAtPoint(table, player, K - 1, col, row, colInc, rowInc);
            }
            else return false;
        }
    }
}
