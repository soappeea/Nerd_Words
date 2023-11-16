// Author: Sophia Lin
// File Name: Program.cs
// Project Name: MP2
// Creation Date: November 10, 2023
// Modified Date: November 15, 2023
// Description: Detect if a word is a Nerd Word or not
using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

namespace NerdWords
{
    class Program
    {
        //Track the execution time passed
        static Stopwatch stopWatch = new Stopwatch();

        //Cache good and bad list
        static List<String> goods = new List<string>();
        static List<String> bads = new List<string>();
        static void Main(string[] args)
        {
            StreamReader inFile;
            StreamWriter outFile;
            string cheatCode;
            int validCount = 0;

            Console.Write("Enter a file name to test: ");
            inFile = File.OpenText(Console.ReadLine());
            outFile = File.CreateText("Lin_S.txt");

            //Reset and start the timer
            stopWatch.Reset();
            stopWatch.Start();

            //Read in one cheat code at a time to be validated
            while (!inFile.EndOfStream)
            {
                cheatCode = inFile.ReadLine();
                goods = new List<string>();
                bads = new List<string>();

                bool isNerdWord = ValidateNerdWord(cheatCode);

                if (isNerdWord)
                {
                    outFile.WriteLine(cheatCode + ":" + "YES");
                    validCount++;
                }
                else
                {
                    outFile.WriteLine(cheatCode + ":" + "NO");
                }
            }

            //Stop the timer and display results on the screen
            stopWatch.Stop();
            Console.WriteLine(GetTimeOutput(stopWatch));
            Console.WriteLine("Valid Nerd-Words: " + validCount);

            //Close the files
            inFile.Close();
            outFile.Close();
            Console.ReadLine();
        }

        private static bool ValidateNerdWord(string cheatCode)
        {
            bool isGood = false;
            isGood = (cheatCode == "X");

            //Make sure at least 3 chars after single character check
            if (!isGood && cheatCode.Length > 2)
            {
                //Check cache to avoid extra calculation
                if (goods.Contains(cheatCode)) return true;
                else if (bads.Contains(cheatCode)) return false;

                //Validate code word
                if (cheatCode.StartsWith("A") && cheatCode.EndsWith("B"))
                    isGood = ValidateCodeWord(cheatCode.Substring(1, cheatCode.Length - 2));

                //If above not good, valide nerd word as well
                if (!isGood && cheatCode.Contains("Y"))
                {
                    List<int> yIndex = new List<int>();
                    for (int i = 1; i < cheatCode.Length - 1; i++)
                    {
                        if (cheatCode[i].Equals('Y'))
                            yIndex.Add(i);
                    }

                    for (int i = 0; i < yIndex.Count; i++)
                    {

                        string left = cheatCode.Substring(0, yIndex[i]);
                        if (ValidateCodeWord(left))
                        {
                            if (!goods.Contains(left))
                            {
                                goods.Add(left);
                            }

                            string right = cheatCode.Substring(yIndex[i] + 1);

                            isGood = ValidateNerdWord(right);
                            if (isGood && !goods.Contains(right))
                            {
                                goods.Add(right);
                            }
                            else if (!isGood && !bads.Contains(right))
                            {
                                bads.Add(right);
                            }
                        }
                        else if (!bads.Contains(left))
                        {
                            bads.Add(left);
                        }

                        if (isGood)
                            break;
                    }
                }
            }
            return isGood;
        }

        private static bool ValidateCodeWord(string cheatCode)
        {
            if (cheatCode == "X")
            {
                return true;
            }
            else if (cheatCode.Length <= 2)
            {
                return false;
            }
            else if (goods.Contains(cheatCode))
            {
                return true;
            }
            else if (bads.Contains(cheatCode))
            {
                return false;
            }
            else
            {
                if (cheatCode.StartsWith("A") && cheatCode.EndsWith("B"))
                {
                    return ValidateNerdWord(cheatCode.Substring(1, cheatCode.Length - 2));
                }
                else
                {
                    return ValidateNerdWord(cheatCode);
                }
            }
        }

        private static string GetTimeOutput(Stopwatch timer)
        {
            TimeSpan ts = timer.Elapsed;
            return "Time- Days:Hours:Minutes:Seconds.Milliseconds:" + ts.Days + ":" + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds + "." + ts.Milliseconds;
        }
    }
}