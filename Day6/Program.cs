using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var points = new List<(int i, int x, int y)>();
            var index = 0;
            foreach (var line in lines)
            {
                index++;
                var (x, y) = line.Split(',');
                points.Add((index, Int32.Parse(x), Int32.Parse(y)));
            }
            var grid = new int[points.Max(x => x.x) + 1, points.Max(y => y.y) + 1];
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var closest = points.Select(x => (x, GetMHDistance((i, j), (x.x, x.y)))).OrderBy(x => x.Item2);
                    if (closest.First().Item2 != closest.Skip(1).First().Item2)
                        grid[i, j] = closest.First().x.i;

                }
            //Console.WriteLine();
            //for (int i = 0; i < grid.GetLength(0); i++)
            //{
            //    for (int j = 0; j < grid.GetLength(1); j++)
            //    {
            //        Console.Write(grid[i, j]);
            //    }
            //    Console.WriteLine();
            //}
            var toignore = new List<int>();
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                toignore.Add(grid[i, 0]);
                toignore.Add(grid[i, grid.GetLength(1) - 1]);
            }
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                toignore.Add(grid[0, i]);
                toignore.Add(grid[grid.GetLength(0) - 1, i]);
            }
            toignore = toignore.Distinct().ToList();
            var sizes = new Dictionary<int, int>();
            for (int i = 0; i < lines.Length + 1; i++)
                sizes[i] = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    sizes[grid[i, j]]++;
                }
            }
            Console.WriteLine(sizes.Where(x => !toignore.Contains(x.Key)).OrderByDescending(x => x.Value).First());

            int within = 0;
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    var closest = points.Select(x => (x, GetMHDistance((i, j), (x.x, x.y)))).OrderBy(x => x.Item2);

                    grid[i, j] = closest.Sum(x => x.Item2);
                    if (grid[i, j] < 10000)
                    {
                        within++;
                    }

                }
            Console.WriteLine(within);

            Console.WriteLine("done.");
            Console.ReadLine();
        }

        public static int GetMHDistance((int, int) one, (int, int) two)
        {
            return Math.Abs(one.Item1 - two.Item1) + Math.Abs(two.Item2 - one.Item2);
        }
    }
}
