using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day15
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var map = new ISquare[lines.First().Length, lines.Count()];

            for (int y = 0; y < lines.Count(); y++)
            {
                var line = lines[y];
                for (int x = 0; x < line.Length; x++)
                {
                    map[x, y] = GetSquare(line[x]);
                }
            }
            Print2DArray(map);

            while (true)
            {
                var movedList = new List<(int, int)>();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (map[x, y] is Unit && !movedList.Any(l => l.Item1 == x && l.Item2 == y))
                        {
                            var current = map[x, y] as Unit;
                            var moved = false;

                            var neighbors = (new List<ISquare>() { map[x, y - 1], map[x - 1, y], map[x + 1, y], map[x, y + 1] }).OfType<Unit>();
                            if (neighbors.Where(z => z.UnitType != current.UnitType).Any())
                            {
                                ExecuteCombat(map, x, y, neighbors);
                            }
                            else
                            {

                                var visited = new List<(int, int)>();
                                var toVisit = new List<(int x, int y, List<(int, int)> steps)>();
                                toVisit.AddRange(GetToVisit(map, x, y, new List<(int, int)>()));

                                for (int i = 0; i < toVisit.Count; i++)
                                {
                                    var checkingSquare = map[toVisit[i].x, toVisit[i].y];

                                    visited.Add((toVisit[i].x, toVisit[i].y));
                                    if (checkingSquare is Unit && ((Unit)checkingSquare).UnitType != current.UnitType && toVisit[i].steps.Count > 0)
                                    {
                                        (int, int) newSquare = toVisit[i].steps.First();
                                        map[newSquare.Item1, newSquare.Item2] = current;
                                        map[x, y] = new Space();
                                        movedList.Add((newSquare.Item1, newSquare.Item2));
                                        break;
                                    }
                                    else if (!(checkingSquare is Wall))
                                    {
                                        toVisit.AddRange(GetToVisit(map, toVisit[i].x, toVisit[i].y, toVisit[i].steps).Where(s => !visited.Contains((s.x, s.y))));
                                    }
                                }

                            }

                            
                          
                        }
                    }
                }
                Print2DArray(map);
            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void ExecuteCombat(ISquare[,] map, int x, int y, IEnumerable<Unit> neighbors)
        {
            neighbors.OrderByDescending(u => u.HP).First().HP -= 3;
            if (neighbors.OrderByDescending(u => u.HP).First().HP <= 0)
            {
                if (map[x, y - 1] is Unit && ((Unit)map[x, y - 1]).HP < 0)
                {
                    map[x, y - 1] = new Space();
                }
                if (map[x - 1, y] is Unit && ((Unit)map[x - 1, y]).HP < 0)
                {
                    map[x - 1, y] = new Space();
                }
                if (map[x + 1, y] is Unit && ((Unit)map[x + 1, y]).HP < 0)
                {
                    map[x + 1, y] = new Space();
                }
                if (map[x, y + 1] is Unit && ((Unit)map[x, y + 1]).HP < 0)
                {
                    map[x, y + 1] = new Space();
                }
            }
        }

        static IEnumerable<(int x, int y, List<(int, int)> steps)> GetToVisit(ISquare[,] square, int x, int y, List<(int, int)> steps)
        {
            var upStep = steps.Where(t => 1 == 1).ToList();
            upStep.Add((x, y - 1));
            var leftStep = steps.Where(t => 1 == 1).ToList();
            leftStep.Add((x - 1, y));
            var rightStep = steps.Where(t => 1 == 1).ToList();
            rightStep.Add((x + 1, y));
            var downStep = steps.Where(t => 1 == 1).ToList();
            downStep.Add((x, y + 1));
            return new List<(int x, int y, List<(int, int)> steps)>
            {
                (x, y-1, upStep),
                (x -1 , y, leftStep),
                (x+1, y, rightStep),
                (x, y+1, downStep),
            }.Where(t => t.x >= 0 && t.y >= 0 && t.x < square.GetLength(0) && y < square.GetLength(1));
        }
        static ISquare GetSquare(char value)
        {
            if (value == '#')
                return new Wall();
            if (value == 'G')
                return new Unit { UnitType = UnitType.Goblin };
            if (value == 'E')
                return new Unit { UnitType = UnitType.Elf };
            return new Space();
        }

        public static void Print2DArray(ISquare[,] toPrint)
        {
            for (int i = 0; i < toPrint.GetLength(0); i++)
            {
                var scores = new List<int>();
                for (int j = 0; j < toPrint.GetLength(1); j++)
                {
                    Console.Write(toPrint[j, i]);
                    if (toPrint[j, i] is Unit)
                    {
                        scores.Add(((Unit)toPrint[j, i]).HP);
                    }
                }
                Console.Write("\t" + String.Join(",", scores));
                Console.WriteLine();
            }
        }
    }
    public interface ISquare
    {

    }
    public class Space : ISquare
    {
        public override string ToString()
        {
            return ".";
        }
    }
    public class Wall : ISquare
    {
        public override string ToString()
        {
            return "#";
        }
    }
    public class Unit : ISquare
    {
        public UnitType UnitType { get; set; }
        public int HP { get; set; } = 200;
        public override string ToString()
        {
            if (UnitType == UnitType.Elf)
                return "E";
            return "G";
        }
    }
    public enum UnitType
    {
        Elf,
        Goblin
    }
}