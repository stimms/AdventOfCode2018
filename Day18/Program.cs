using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day18
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
            var lines = File.ReadAllLines("input1.txt");
            var areas = new char[lines.First().Length, lines.Count()];
            for (int y = 0; y < lines.Count(); y++)
                for (int x = 0; x < lines.First().Length; x++)
                    areas[x, y] = lines[x][y];

            Print2DArray(areas);
            for (int i = 0; i < 1_000_000_000; i++)
            {
                var workingCopy = areas.Clone() as char[,];
                for (int y = 0; y < lines.Count(); y++)
                    for (int x = 0; x < lines.First().Length; x++)
                    {
                        workingCopy[x, y] = GetResultingValue(areas, x, y);
                    }
                areas = workingCopy.Clone() as char[,];
                //Print2DArray(areas);
                if (i % 10_000 ==0)
                    Console.Write(".");
            }
            Console.WriteLine(GetValue(areas));
        }

        private static int GetValue(char[,] areas)
        {
            int lumber = 0;
            int mill = 0;
            for (int i = 0; i < areas.GetLength(0); i++)
            {
                for (int j = 0; j < areas.GetLength(1); j++)
                {
                    if (areas[i, j] == '|')
                        lumber++;
                    if (areas[i, j] == '#')
                        mill++;
                }
            }
            return lumber * mill;
        }

        public static void Print2DArray(char[,] toPrint, int startx = 0, int starty = 0)
        {
            Console.WriteLine();
            for (int i = startx; i < toPrint.GetLength(0); i++)
            {
                for (int j = starty; j < toPrint.GetLength(1); j++)
                {
                    Console.Write(toPrint[i, j]);
                }
                Console.WriteLine();
            }
        }

        private static char GetResultingValue(char[,] areas, int x, int y)
        {
            if (areas[x, y] == '.' && GetNeighbors(areas, x, y).trees >= 3)
                return '|';
            else if (areas[x, y] == '.')
                return '.';

            if (areas[x, y] == '|' && GetNeighbors(areas, x, y).lumberyard >= 3)
                return '#';
            else if (areas[x, y] == '|')
                return '|';

            if (areas[x, y] == '#' && GetNeighbors(areas, x, y).lumberyard >= 1 && GetNeighbors(areas, x, y).trees >= 1)
                return '#';
            else if (areas[x, y] == '#')
                return '.';

            throw new Exception("Unable to figure");
        }
        private static (int open, int trees, int lumberyard) GetCell(char c, (int open, int trees, int lumberyard) initial)
        {
            if (c == '.')
                return (initial.open + 1, initial.trees, initial.lumberyard);
            if (c == '|')
                return (initial.open, initial.trees + 1, initial.lumberyard);
            return (initial.open, initial.trees, initial.lumberyard + 1);
        }
        private static (int open, int trees, int lumberyard) GetNeighbors(char[,] areas, int x, int y)
        {
            var value = (0, 0, 0);
            if (x >= 1)
            {
                value = GetCell(areas[x - 1, y], value);
                if (y >= 1)
                    value = GetCell(areas[x - 1, y - 1], value);
                if (y + 1 < areas.GetLength(1))
                    value = GetCell(areas[x - 1, y + 1], value);
            }
            if (y >= 1)
                value = GetCell(areas[x, y - 1], value);
            if (y + 1 < areas.GetLength(1))
                value = GetCell(areas[x, y + 1], value);
            if (x + 1 < areas.GetLength(0))
            {
                value = GetCell(areas[x + 1, y], value);
                if (y >= 1)
                    value = GetCell(areas[x + 1, y - 1], value);
                if (y + 1 < areas.GetLength(1))
                    value = GetCell(areas[x + 1, y + 1], value);
            }
            return value;
        }



        private static void Part2()
        {


        }
    }

}