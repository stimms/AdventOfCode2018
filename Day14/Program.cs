using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = 598701;
            var elfCount = 2;
            var scores = new List<int> { 3, 7 };
            var elves = new List<int> { 0, 1 };
            for (int i = 0;  (scores.Count-2)/2 < input+10; i++)
            {
                var newScore = scores[elves[0]] + scores[elves[1]];
                if (newScore >= 10 )
                    scores.Add(1);
                scores.Add(newScore % 10);
                

                elves[0] = (elves[0] + (1 + scores[elves[0]])) % scores.Count;
                elves[1] = (elves[1] + (1 + scores[elves[1]])) % scores.Count;
                if (elves[0] == elves[1])
                    elves[1]++;

            }
            var next = string.Join(' ', scores.Skip(input).Take(10)).Replace(" ", "");
            next+= string.Join(' ', scores.Take(10-next.Length)).Replace(" ", "");
            Console.WriteLine(next);
            Console.WriteLine("done.");
            Console.ReadLine();
        }

    }
}