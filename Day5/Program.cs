using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day5
{
    class Program
    {
        private const string alphabet = "abcdefghijklmnopqrstuvwxyz";

        private static string React(string line)
        {
            var length = line.Length;
            do
            {
                length = line.Length;
                foreach (var c in alphabet)
                {
                    line = line.Replace($"{c}{c.ToString().ToUpper()}", "").Replace($"{c.ToString().ToUpper()}{c}", "");
                }
               
            }
            while (length != line.Length);
            return line;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            Console.WriteLine(React(lines.First()).Length);

            var lengths = new List<(string, int)>();
            foreach (var c in alphabet)
            {
                var currentReplacement = c.ToString();
                var line = lines.First();
                
                line = line.Replace(currentReplacement, "");
                line = line.Replace(currentReplacement.ToUpper(), "");
                line = React(line);
                lengths.Add((c.ToString(), line.Length));
                Console.Write(".");

            }
            Console.WriteLine(lengths.Min(x => x.Item2));

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }
}
