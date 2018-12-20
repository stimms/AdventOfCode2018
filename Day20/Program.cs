using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Day20
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
            var line = File.ReadAllLines("input1.txt").First();

            line = "^ENWWW$";
            var node = GetNodes(line);
            Assert.Equal(5, node.Single().MaxChildLength);

            line = "^(EE|N)$";
            node = GetNodes(line);
            Assert.Equal(2, node.Single().MaxChildLength);

            line = "^ENWWW(NEEE|SSE(EE|N))$";
            node = GetNodes(line);
            Assert.Equal(10, node.Single().MaxChildLength);

            line = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            node = GetNodes(line);
            Assert.Equal(18, node.Single().MaxChildLength);

            line = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|S)NNN$";
            node = GetNodes(line);
            Assert.Equal(22, node.Single().MaxChildLength);

            line = "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS))))$";
            node = GetNodes(line);
            Assert.Equal(31, node.Single().MaxChildLength);

            line = "^WSSEESWWWNW(S|NENNEEEENN(ESSSSW(NWSW|SSEN)|WSWWN(E|WWS(E|SS)))|)$";
            node = GetNodes(line);
            Assert.Equal(11, node.Single().MaxChildLength);

            line = "^N(E|WWSSSESWWNN(NN|WWSESSSWSWSSEEENN(WSWENE|)))$";
            node = GetNodes(line);
            Assert.Equal(29, node.Single().MaxChildLength);

            line = File.ReadAllLines("input1.txt").First();
            node = GetNodes(line);
            Console.WriteLine(node.Single().MaxChildLength);
        }
        static List<Node> GetNodes(string line)
        {
            int index = 0;
            int counter = 0;
            var nodes = new List<Node>();
            var node = new Node();
            while (true)
            {
                if (line[index] == '(')
                {
                    index++;
                    var set = new List<Node>();
                    set.AddRange(GetNodes(line.Substring(index)));
                    node.ChildSets.Add(set);
                    var skipSectionLength = 1;
                    var openCounter = 1;
                    while (openCounter > 0)
                    {
                        if (line[index + skipSectionLength] == '(')
                        {
                            openCounter++;
                        }
                        if (line[index + skipSectionLength] == ')')
                        {
                            openCounter--;
                        }
                        skipSectionLength++;
                    }
                    index += skipSectionLength;
                }
                if (line[index] == '|')
                {
                    node.Length = counter;
                    nodes.Add(node);
                    node = new Node();
                    index++;
                    counter = 0;
                }
                else if (line[index] == '^')
                {
                    index++;
                }
                else if (line[index] == ')' | line[index] == '$')
                {
                    node.Length = counter;
                    nodes.Add(node);
                    return nodes;
                }
                else
                {
                    counter++;
                    index++;
                }


            }
        }




        private static void Part2()
        {
            var line = File.ReadAllLines("input1.txt").First();
            line = "N";
            Assert.Equal(0, GetRoomsIn(line));

            line = "N(E|W)";
            Assert.Equal(3, GetRoomsIn(line, 1));

            line = "^ENNWSWW(NEWS|)SSSEEN(WNSE|)EE(SWEN|)NNN$";
            Assert.Equal(20, GetRoomsIn(line, 5));

            line = File.ReadAllLines("input1.txt").First();
            Console.WriteLine(GetRoomsIn(line));
        }
        static int GetRoomsIn(string line, int limit = 1000)
        {
            var grid = new int[8000, 8000];
            for (int i = 0; i < grid.GetLength(0); i++)
                for (int j = 0; j < grid.GetLength(0); j++)
                    grid[i, j] = 0;

            var decisionPoints = new Stack<(int, int, int)>();
            int x = 4000;
            int y = 4000;
            int doorCounter = 0;
            int farRooms = 0;
            grid[x, y] = 1;
            for (int i = 0; i < line.Length; i++)
            {
                var letter = line[i];
                if (letter == '(')
                {
                    decisionPoints.Push((x, y, doorCounter));

                }
                if (letter == '|')
                {
                    (x, y, doorCounter) = decisionPoints.Peek();
                }
                if (letter == ')')
                {
                    (x, y, doorCounter) = decisionPoints.Pop();
                }
                else
                {
                    if (letter == 'N')
                        x -= 1;
                    if (letter == 'S')
                        x += 1;
                    if (letter == 'E')
                        y += 1;
                    if (letter == 'W')
                        y -= 1;
                    if (grid[x, y] == 0)
                        doorCounter++;
                    if (grid[x, y] == 0 && doorCounter >= limit)
                        farRooms++;
                    grid[x, y] = 1;
                }
            }

           
            return farRooms;
        }
    }
    public class Node
    {
        private int _maxChildLength = -1;
        public int MaxChildLength
        {
            get
            {
                if (_maxChildLength != -1)
                    return _maxChildLength;

                if (ChildSets.Count == 0)
                    return this.Length;
                else
                {
                    var childLengths = 0;
                    foreach (var set in ChildSets)
                    {
                        if (set.Any(x => x.MaxChildLength == 0))
                            childLengths += 0;
                        else
                            childLengths += (set.Max(x => x.MaxChildLength));
                    }
                    _maxChildLength = childLengths + Length;
                    return childLengths + Length;
                }

            }
        }
        public int Length { get; set; }
        public List<List<Node>> ChildSets { get; set; } = new List<List<Node>>();
        public Node()
        {

        }
    }
}