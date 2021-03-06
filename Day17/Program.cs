﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day17
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
            var floors = new List<(int x1, int x2, int depth)>();
            var walls = new List<(int y1, int y2, int x)>();
            foreach (var line in lines)
            {
                if (line.StartsWith("x"))
                {
                    //wall
                    var lineBits = line.Split(new string[] { "=", ",", ".." }, StringSplitOptions.RemoveEmptyEntries);
                    walls.Add((Int32.Parse(lineBits[3]), Int32.Parse(lineBits[4]), Int32.Parse(lineBits[1])));
                }
                else
                {
                    //floor
                    var lineBits = line.Split(new string[] { "=", ",", ".." }, StringSplitOptions.RemoveEmptyEntries);
                    floors.Add((Int32.Parse(lineBits[3]), Int32.Parse(lineBits[4]), Int32.Parse(lineBits[1])));
                }
            }
            var minWall = walls.Select(x => x.x).Union(floors.Select(x => x.x2)).Min();
            var cells = new CellContents[walls.Select(x => x.x).Union(floors.Select(x => x.x2)).Max() - minWall + 2, floors.Select(x => x.depth).Union(walls.Select(x => x.y2)).Max() + 1];

            foreach (var wall in walls)
            {
                for (int i = wall.y1; i <= wall.y2; i++)
                    cells[wall.x - minWall + 1, i] = CellContents.Clay;
            }
            foreach (var floor in floors)
            {
                for (int i = floor.x1 - minWall + 1; i <= floor.x2 - minWall; i++)
                    cells[i, floor.depth] = CellContents.Clay;
            }
            var numberFilled = 0;
            cells[500 - walls.Select(x => x.x).Union(floors.Select(x => x.x2)).Min() + 1, 0] = CellContents.FallingWater;

            while (numberFilled != GetNumberFiled(cells))
            {

                numberFilled = GetNumberFiled(cells);
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    for (int y = 0; y < cells.GetLength(1); y++)
                    {
                        if (x == 500)
                        {
                            var b = 1 + 1;
                        }
                        if (y + 1 < cells.GetLength(1) && cells[x, y] == CellContents.FallingWater && cells[x, y + 1] == CellContents.Dirt)
                            cells[x, y + 1] = CellContents.FallingWater;
                        if (y > 0 && cells[x, y] == CellContents.Clay && cells[x, y - 1] == CellContents.FallingWater)
                        {
                            cells[x, y - 1] = CellContents.Water;
                        }

                        if (y + 1 < cells.GetLength(1) && cells[x, y] == CellContents.FallingWater && (cells[x, y + 1] == CellContents.Water))
                        {
                            var fillLeftCounter = x - 1;
                            while (fillLeftCounter > 0 && y + 1 < cells.GetLength(1) && (cells[fillLeftCounter, y] == CellContents.Dirt && (cells[fillLeftCounter, y + 1] == CellContents.Water || cells[fillLeftCounter, y + 1] == CellContents.Clay)))
                            {
                                cells[fillLeftCounter, y] = CellContents.FallingWater;
                                fillLeftCounter--;
                            }
                            if (fillLeftCounter >= 0 && y + 1 < cells.GetLength(1) && cells[fillLeftCounter, y + 1] == CellContents.Dirt)
                            {
                                cells[fillLeftCounter, y] = CellContents.FallingWater;
                                cells[fillLeftCounter, y + 1] = CellContents.FallingWater;
                            }

                            var fillRightCounter = x + 1;
                            while (fillRightCounter < cells.GetLength(0) && y + 1 < cells.GetLength(1) && (cells[fillRightCounter, y] == CellContents.Dirt && (cells[fillRightCounter, y + 1] == CellContents.Water || cells[fillRightCounter, y + 1] == CellContents.Clay)))
                            {
                                cells[fillRightCounter, y] = CellContents.FallingWater;
                                fillRightCounter++;
                            }
                            if (fillRightCounter < cells.GetLength(0) && y + 1 < cells.GetLength(1) && cells[fillRightCounter, y + 1] == CellContents.Dirt)
                            {
                                cells[fillRightCounter, y] = CellContents.FallingWater;
                                cells[fillRightCounter, y + 1] = CellContents.FallingWater;
                            }
                        }

                    }
                }


                bool shouldConvert = false;
                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    var toConvert = new List<(int x, int y)> { };
                    for (int x = 0; x < cells.GetLength(0); x++)

                    {
                        if (cells[x, y] == CellContents.Dirt)
                            toConvert.Clear();

                        if (cells[x, y] == CellContents.FallingWater)
                        {

                            if (x > 0 && y + 1 < cells.GetLength(1) && (cells[x - 1, y] == CellContents.Clay || toConvert.Count > 0) && (cells[x, y + 1] == CellContents.Water))
                            {
                                toConvert.Add((x, y));
                                if (cells[x + 1, y] == CellContents.Clay)
                                    shouldConvert = true;
                            }
                            if (shouldConvert)
                            {
                                foreach (var convert in toConvert)
                                    cells[convert.x, convert.y] = CellContents.Water;
                                shouldConvert = false;
                                toConvert.Clear();
                            }
                        }
                    }
                }


                for (int y = 0; y < cells.GetLength(1); y++)
                {
                    for (int x = 0; x < cells.GetLength(0); x++)
                    {
                        if (cells[x, y] == CellContents.Water)
                        {
                            var fillLeftCounter = x - 1;
                            while (fillLeftCounter > 0 && y + 1 < cells.GetLength(1) && (cells[fillLeftCounter, y] == CellContents.Dirt && cells[fillLeftCounter, y + 1] == CellContents.Clay))
                            {
                                cells[fillLeftCounter, y] = CellContents.Water;
                                fillLeftCounter--;
                            }
                            if (fillLeftCounter >= 0 && y + 1 < cells.GetLength(1) && cells[fillLeftCounter, y + 1] == CellContents.Dirt)
                            {
                                cells[fillLeftCounter, y + 1] = CellContents.FallingWater;
                            }

                            var fillRightCounter = x + 1;
                            while (fillRightCounter < cells.GetLength(0) && y + 1 < cells.GetLength(1) && (cells[fillRightCounter, y] == CellContents.Dirt && cells[fillRightCounter, y + 1] == CellContents.Clay))
                            {
                                cells[fillRightCounter, y] = CellContents.Water;
                                fillRightCounter++;
                            }
                            if (fillRightCounter < cells.GetLength(0) && y + 1 < cells.GetLength(1) && cells[fillRightCounter, y + 1] == CellContents.Dirt)
                            {
                                cells[fillRightCounter, y + 1] = CellContents.FallingWater;
                            }
                        }
                    }
                }
                //Print2DArray(cells, walls.Select(x => x.x).Union(floors.Select(x => x.x2)).Min(), 0);
            }
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 1; x < cells.GetLength(0) - 1; x++)
                {
                    if (cells[x - 1, y] == CellContents.FallingWater && cells[x, y] == CellContents.Water && cells[x + 1, y] == CellContents.FallingWater)
                    {
                        cells[x, y] = CellContents.FallingWater;
                    }
                }
            }
            File2DArray(cells, 0, 0);
            Console.WriteLine(GetNumberFiled(cells, 0, floors.Select(x => x.depth).Union(walls.Select(x => x.y1)).Min()));
            Console.WriteLine(GetNumberFiledStanding(cells, 0, floors.Select(x => x.depth).Union(walls.Select(x => x.y1)).Min()));
        }


        public static void File2DArray(CellContents[,] toPrint, int startx = 0, int starty = 0)
        {
            var lines = new List<string>();
            for (int i = starty; i < toPrint.GetLength(1); i++)
            {
                var sb = new StringBuilder();
                for (int j = startx; j < toPrint.GetLength(0); j++)
                {
                    switch (toPrint[j, i])
                    {
                        case CellContents.Clay:
                            sb.Append("#");
                            break;
                        case CellContents.Dirt:
                            sb.Append(".");
                            break;
                        case CellContents.Water:
                            sb.Append("~");
                            break;
                        case CellContents.FallingWater:
                            sb.Append("|");
                            break;
                    }
                }
                lines.Add(sb.ToString());
            }
            File.WriteAllLines("output.txt", lines);
        }
        public static void Print2DArray(CellContents[,] toPrint, int startx = 0, int starty = 0)
        {
            for (int i = starty; i < toPrint.GetLength(1); i++)
            {
                for (int j = startx; j < toPrint.GetLength(0); j++)
                {
                    switch (toPrint[j, i])
                    {
                        case CellContents.Clay:
                            Console.Write("#");
                            break;
                        case CellContents.Dirt:
                            Console.Write(".");
                            break;
                        case CellContents.Water:
                            Console.Write("~");
                            break;
                        case CellContents.FallingWater:
                            Console.Write("|");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        private static int GetNumberFiledStanding(CellContents[,] cells, int startX = 0, int startY = 0)
        {
            var counter = 0;
            for (int i = startX; i < cells.GetLength(0); i++)
                for (int j = startY; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == CellContents.Water)
                    {
                        counter++;
                    }
                }
            return counter;
        }

        private static int GetNumberFiled(CellContents[,] cells, int startX = 0, int startY = 0)
        {
            var counter = 0;
            for (int i = startX; i < cells.GetLength(0); i++)
                for (int j = startY; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == CellContents.Water || cells[i, j] == CellContents.FallingWater)
                    {
                        counter++;
                    }
                }
            return counter;
        }

        private static void Part2()
        {


        }
    }
    public enum CellContents
    {
        Dirt,
        Clay,
        Water,
        FallingWater
    }
}