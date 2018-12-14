﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();
            Part2();

            Console.WriteLine("done.");
            Console.ReadLine();
        }
        private static void Part1()
        {
            var input = 598701;
            var scores = new List<int> { 3, 7 };
            var elves = new List<int> { 0, 1 };
            for (int i = 0; (scores.Count - 2) / 2 < input + 10; i++)
            {
                var newScore = scores[elves[0]] + scores[elves[1]];
                if (newScore >= 10)
                    scores.Add(1);
                scores.Add(newScore % 10);


                elves[0] = (elves[0] + (1 + scores[elves[0]])) % scores.Count;
                elves[1] = (elves[1] + (1 + scores[elves[1]])) % scores.Count;
                if (elves[0] == elves[1])
                    elves[1]++;

            }
            var next = string.Join(' ', scores.Skip(input).Take(10)).Replace(" ", "");
            Console.WriteLine(next);
        }
        private static void Part2()
        {
            var input = "598701";
            int lastDigit = Int32.Parse(input.Last().ToString());

            var scores = new List<int> { 3, 7 };
            var elves = new List<int> { 0, 1 };
            while (true)
            {
                var newScore = scores[elves[0]] + scores[elves[1]];
                if (newScore >= 10)
                    scores.Add(1);
                scores.Add(newScore % 10);


                elves[0] = (elves[0] + (1 + scores[elves[0]])) % scores.Count;
                elves[1] = (elves[1] + (1 + scores[elves[1]])) % scores.Count;
                if (elves[0] == elves[1])
                    elves[1]++;

                if (newScore % 10 <= lastDigit)
                {
                    var all = string.Join(' ', scores.Skip(scores.Count() - (input.Count() + 1))).Replace(" ", "");
                    if (all.Contains(input))
                    {
                        Console.WriteLine(scores.LongCount() - input.Count() - (all.IndexOf(input) == 0 ? 1 : 0));
                        break;
                    }
                }
            }
        }
    }
}