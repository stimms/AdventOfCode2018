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
            

            var playerScore = new long[numPlayers];
            var circle = new LinkedList<int>();
            circle.AddFirst(0);
            circle.AddLast(1);
            int round = 2;
            var current = circle.Find(1);
            while (round < points)
            {
                if (round % 23 == 0)
                {
                    var toRemove =  get7back(current, circle);
                    playerScore[round % numPlayers] += round + toRemove.Value;
                    
                    current = toRemove.Next;
                    circle.Remove(toRemove);
                }
                else
                {
                    current = circle.AddAfter(current.Next ?? circle.First, round);
                }
                round++;
            }


            Console.WriteLine(playerScore.Max());
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        static LinkedListNode<int> get7back(LinkedListNode<int> current, LinkedList<int> list)
        {
            int counter = 0;
            while(counter < 7)
            {
                current = current.Previous;
                if(current == null)
                {
                    current = list.Last;
                }
                counter++;
            }
            return current;
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
