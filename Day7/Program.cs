using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day7

{
    class Program
    {
        private const int WORKERS = 5;

        static int CalculateTime(string cal)
        {
            return 60 + (cal.ToCharArray().First() - 'A' + 1);
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");
            Part1();
            Part2();
            Console.WriteLine("done.");
            Console.ReadLine();
        }
        static void Part2()
        {
            var lines = File.ReadAllLines("input.txt");
            var inputs = new List<(string prereq, string actual)>();
            foreach (var line in lines)
            {
                var a = line.Split(' ')[1];
                var b = line.Split(' ')[7];
                inputs.Add((a, b));
            }
            var actuals = inputs.Select(x => x.actual).Distinct().OrderBy(x => x).ToList();
            var prereqs = inputs.Select(x => x.prereq).Distinct().OrderBy(x => x).ToList();
            var all = inputs.Select(x => x.actual).Union(inputs.Select(x => x.prereq)).Distinct().OrderBy(x => x).ToList();
            var order = new List<string>();
            var time = 0;
            var inprogress = new List<(string unit, int timeRemaining)>();
            for (int i = 0; i < WORKERS; i++)
                inprogress.Add((null, 0));
            while (order.Count() != all.Count())
            {

                inprogress = inprogress.Select(x => (x.unit, x.timeRemaining - 1)).ToList();
                order.AddRange(inprogress.Where(x => x.timeRemaining == 0).Select(x => x.unit).OrderBy(x => x));
                inprogress = inprogress.Where(x => x.Item2 > 0).ToList();

                inputs = inputs.Where(x => !order.Contains(x.prereq)).ToList();
                actuals = inputs.Select(x => x.actual).Where(x => !inprogress.Select(y => y.unit).Contains(x) && !order.Contains(x)).Distinct().OrderBy(x => x).ToList();
                if (!actuals.Any())
                    prereqs = all.Where(x => !inprogress.Select(y => y.unit).Contains(x) && !order.Contains(x)).OrderBy(x => x).ToList();
                else
                    prereqs = inputs.Select(x => x.prereq).Distinct().OrderBy(x => x).ToList();
                inprogress.AddRange(prereqs.Where(x => !actuals.Contains(x) && !inprogress.Select(y => y.unit).Contains(x)).Take(WORKERS - inprogress.Count).Select(x => (x, CalculateTime(x))));
                time++;
            }
            Console.WriteLine(time - 1);

        }
        static void Part1() {
            

            var lines = File.ReadAllLines("input.txt");
            var inputs = new List<(string prereq, string actual)>();
            foreach (var line in lines)
            {
                var a = line.Split(' ')[1];
                var b = line.Split(' ')[7];
                inputs.Add((a, b));
            }
            var actuals = inputs.Select(x => x.actual).Distinct().OrderBy(x => x).ToList();
            var prereqs = inputs.Select(x => x.prereq).Distinct().OrderBy(x => x).ToList();
            var order = new List<string>();
            while (actuals.Any())
            {
                order.Add(prereqs.Where(x => !actuals.Contains(x)).First());
                if (!inputs.Where(x => !order.Contains(x.prereq)).Any())
                {
                    order.AddRange(inputs.Select(x => x.actual).OrderBy(x => x));
                }
                inputs = inputs.Where(x => !order.Contains(x.prereq)).ToList();
                actuals = inputs.Select(x => x.actual).Distinct().OrderBy(x => x).ToList();
                prereqs = inputs.Select(x => x.prereq).Distinct().OrderBy(x => x).ToList();
            }
            foreach (var x in order)
            {
                Console.Write(x);
            }
            Console.WriteLine();
        }
    }
}
