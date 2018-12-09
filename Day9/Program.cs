using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day9
{
    class Program
    {
        static void Main(string[] args)
        {
            var line = File.ReadAllLines("input.txt").First();
            var numbers = line.Split(' ').Select(x => Int32.Parse(x));
            int numPlayers = Int32.Parse(line.Split(' ')[0]);
            int points = Int32.Parse(line.Split(' ')[6]);
            

            var playerScore = new int[numPlayers];
            var circle = new List<int>();
            circle.Insert(0, 0);
            circle.Insert(1, 1);
            int round = 2;
            int current = 1;
            while (round < points)
            {
                if (round % 23 == 0)
                {
                    int toRemove =  loopRemainder((current - 7), circle.Count);
                    playerScore[round % numPlayers] += round + circle[toRemove];
                    circle.RemoveAt(toRemove);
                    current = toRemove % circle.Count;
                }
                else
                {
                    var location = (current + 2) % (circle.Count);
                    circle.Insert(location, round);
                    current = location;
                    
                }
                round++;
            }


            Console.WriteLine(playerScore.Max());
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        static int loopRemainder(int index, int loopSize)
        {
            var remainder = index % loopSize;
            if (remainder >= 0)
                return remainder;
            return loopSize + remainder;
        }
    }
}
