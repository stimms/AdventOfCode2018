using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            foreach (var line in lines)
            {
                var (a, b) = line.Split(new char[] { ',' });
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}
