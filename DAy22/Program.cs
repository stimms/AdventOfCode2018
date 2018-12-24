using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Day22
{
    class Program
    {
        static void Main(string[] args)
        {
            Part1();

            Console.WriteLine("done.");
            Console.ReadLine();
        }
        static void Part1()
        {
            var depth = 510;
            var map = BuildMap(depth, 10, 10);
            Assert.Equal(0, map[0, 0].geologic);
            Assert.Equal(510, map[0, 0].erosion);
            Assert.Equal(0, map[0, 0].risk);

            Assert.Equal(16807, map[1, 0].geologic);
            Assert.Equal(17317, map[1, 0].erosion);
            Assert.Equal(1, map[1, 0].risk);

            Assert.Equal(48271, map[0, 1].geologic);
            Assert.Equal(8415, map[0, 1].erosion);
            Assert.Equal(0, map[0, 1].risk);

            Assert.Equal(145722555, map[1, 1].geologic);
            Assert.Equal(1805, map[1, 1].erosion);
            Assert.Equal(2, map[1, 1].risk);

            Assert.Equal(0, map[10, 10].geologic);
            Assert.Equal(510, map[10, 10].erosion);
            Assert.Equal(0, map[10, 10].risk);

            Assert.Equal(114, GetRisk(map));

            map = BuildMap(8112, 13, 743);
            Console.WriteLine(GetRisk(map));
        }

        private static int GetRisk((int geologic, int erosion, int risk)[,] map)
        {
            var sum = 0;
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    sum += map[i, j].risk;
            return sum;
        }

        private static (int geologic, int erosion, int risk)[,] BuildMap(int depth, int maxX, int maxY)
        {
            var regions = new(int geologic, int erosion, int risk)[maxX + 1, maxY + 1];
            for (int i = 0; i < regions.GetLength(0); i++)
                for (int j = 0; j < regions.GetLength(1); j++)
                {
                    int geologic = 0;
                    if (i == 0 && j == 0 || (i == maxX && j == maxY))
                    {
                        geologic = 0;

                    }
                    else if (i == 0)
                    {
                        geologic = j * 48271;
                    }
                    else if (j == 0)
                    {
                        geologic = i * 16807;
                    }
                    else
                    {
                        geologic = regions[i - 1, j].erosion * regions[i, j - 1].erosion;
                    }

                    var erosion = (geologic + depth) % 20183;
                    regions[i, j] = (geologic, erosion, erosion % 3);
                }
            return regions;
        }

    }
}