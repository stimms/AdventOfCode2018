using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 300;
            Console.WindowHeight = 30;
            var lines = File.ReadAllLines("input.txt");
            List<Point> points = GetPoints(lines);
            int counter = 0;

            while (true)
            {
                counter++;
                UpdatePointLocations(points);
                if (ThereAreABunchOfVerticalLines(points))
                {
                    Print(points, counter);
                }
            }
        }

        private static bool ThereAreABunchOfVerticalLines(List<Point> points)
        {
            return points.GroupBy(p => p.X, p => p.X, (key, g) => g).Where(x => x.Count() > 7).Count() > 3;
        }

        private static void UpdatePointLocations(List<Point> points)
        {
            foreach (var p in points)
            {
                p.X = p.X + p.XVelocity;
                p.Y = p.Y + p.YVelocity;
            }
        }

        private static void Print(List<Point> points, int counter)
        {
            var stringBuilder = new StringBuilder();
            Console.Clear();
            var minX = points.Min(x => x.X);
            var minY = points.Min(y => y.Y);
            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < 256; i++)
                {
                    if (points.Any(p => p.X == i + minX && p.Y == j + minY))
                    {
                        stringBuilder.Append("#");
                    }
                    else
                    {
                        stringBuilder.Append(".");
                    }

                }
                stringBuilder.Append("\n");
            }
            Console.WriteLine(stringBuilder.ToString());
            Console.WriteLine(counter);
            Thread.Sleep(500);
        }

        private static List<Point> GetPoints(string[] lines)
        {
            var points = new List<Point>();
            foreach (var line in lines)
            {
                var (x, y) = line.Split('<').Skip(1).First().Split(',', '>');
                var (velX, velY) = line.Split('<').Skip(2).First().Split(',', '>');
                points.Add(new Point
                {
                    X = Int32.Parse(x),
                    XVelocity = Int32.Parse(velX),
                    Y = Int32.Parse(y),
                    YVelocity = Int32.Parse(velY)
                });
            }

            return points;
        }
    }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int XVelocity { get; set; }
        public int YVelocity { get; set; }
    }
}
