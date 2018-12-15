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

            var round = 0;
            var incomplete = true;
            while (incomplete)
            {


                var movedList = new List<(int, int)>();
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        var updatedx = x;
                        var updatedy = y;
                        if (map[x, y] is Unit && !movedList.Any(l => l.Item1 == x && l.Item2 == y))
                        {
                            var current = map[x, y] as Unit;
                            var moved = false;

                            var neighbors = (new List<ISquare>() { map[x, y - 1], map[x - 1, y], map[x + 1, y], map[x, y + 1] }).OfType<Unit>().Where(z => z.UnitType != current.UnitType);

                            if (!neighbors.Any())
                            {

                                var visited = new List<(int, int)>();
                                var toVisit = new List<(int x, int y, List<(int, int)> steps)>();
                                List<(int, int)> baddies = GetAllUnitsOfType(map, current.UnitType == UnitType.Elf ? UnitType.Goblin : UnitType.Elf);
                                foreach (var baddy in baddies)
                                {
                                    toVisit.AddRange(GetToVisit(map, baddy.Item1, baddy.Item2, new List<(int, int)>()));
                                }
                                var scores = new int[map.GetLength(0), map.GetLength(1)];
                                for (int o = 0; o < scores.GetLength(0); o++)
                                    for (int p = 0; p < scores.GetLength(1); p++)
                                        scores[o, p] = map.GetLength(0) + map.GetLength(1);
                                for (int i = 0; i < toVisit.Count; i++)
                                {
                                    var checkingSquare = map[toVisit[i].x, toVisit[i].y];
                                    if (checkingSquare is Space)
                                    {

                                        visited.Add((toVisit[i].x, toVisit[i].y));

                                        scores[toVisit[i].x, toVisit[i].y] = Math.Min(toVisit[i].steps.Count, scores[toVisit[i].x, toVisit[i].y]);




                                        toVisit.AddRange(GetToVisit(map, toVisit[i].x, toVisit[i].y, toVisit[i].steps).Where(s => !toVisit.Any(t => t.x == s.x && t.y == s.y) && !visited.Contains((s.x, s.y))));
                                    }
                                }
                                var candidates = new List<int> { scores[x, y - 1], scores[x - 1, y], scores[x + 1, y], scores[x, y + 1] };
                                if (candidates.Any(c => c < map.GetLength(0) + map.GetLength(1)))
                                {
                                    var moveTo = candidates.IndexOf(candidates.Min());
                                    if (moveTo == 0)
                                    {
                                        map[x, y - 1] = current;
                                        movedList.Add((x, y - 1));
                                        updatedy = y - 1;
                                    }
                                    else if (moveTo == 1)
                                    {
                                        map[x - 1, y] = current;
                                        movedList.Add((x - 1, y));
                                        updatedx = x - 1;
                                    }
                                    else if (moveTo == 2)
                                    {
                                        map[x + 1, y] = current;
                                        movedList.Add((x + 1, y));
                                        updatedx = x + 1;
                                    }
                                    else if (moveTo == 3)
                                    {
                                        map[x, y + 1] = current;
                                        movedList.Add((x, y + 1));
                                        updatedy = y + 1;
                                    }
                                    map[x, y] = new Space();
                                }
                                Console.WriteLine($"({x},{y}) -> ({updatedx},{updatedy})");
                                //scores.Print2DArray();


                                //Print2DArray(map);
                            }
                            neighbors = (new List<ISquare>() { map[updatedx, updatedy - 1], map[updatedx - 1, updatedy], map[updatedx + 1, updatedy], map[updatedx, updatedy + 1] }).OfType<Unit>().Where(z => z.UnitType != current.UnitType);
                            if (neighbors.Any())
                            {
                                ExecuteCombat(map, updatedx, updatedy, neighbors);
                            }



                        }
                    }
                }

                //Console.WriteLine(AllHPs(map));
                //Console.WriteLine("Round: " + round);
                //Print2DArray(map);
                if (AreNoOpponents(map))
                {
                    Console.WriteLine(round * AllHPs(map));
                    incomplete = false;
                    Print2DArray(map);
                }
                round++;

            }
            Console.WriteLine("done.");
            Console.ReadLine();
        }
        static List<(int, int)> GetAllUnitsOfType(ISquare[,] map, UnitType type)
        {
            var all = new List<(int, int)>();
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] is Unit && ((Unit)map[x, y]).UnitType == type)
                    {
                        all.Add((x, y));
                    }
                }
            }
            return all;
        }
        static bool AreNoOpponents(ISquare[,] map)
        {
            var elves = 0;
            var goblins = 0;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] is Unit)
                    {
                        if (((Unit)map[x, y]).UnitType == UnitType.Elf)
                            elves++;
                        else
                            goblins++;
                    }
                }
            }
            return elves == 0 || goblins == 0;
        }
        static int AllHPs(ISquare[,] map)
        {
            var total = 0;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (map[x, y] is Unit)
                    {
                        total += ((Unit)map[x, y]).HP;
                    }
                }
            }
            return total;
        }

        private static void ExecuteCombat(ISquare[,] map, int x, int y, IEnumerable<Unit> neighbors)
        {
            neighbors.OrderBy(u => u.HP).First().HP -= 3;

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
            for (int i = 0; i < toPrint.GetLength(1); i++)
            {
                var scores = new List<int>();
                for (int j = 0; j < toPrint.GetLength(0); j++)
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