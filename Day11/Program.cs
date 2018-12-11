using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Day11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = 9306;
            const int GRID_SIZE = 300;
            var cells = new int[GRID_SIZE, GRID_SIZE];
            for (int x = 0; x < GRID_SIZE; x++)
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    int rackId = x + 1 + 10;
                    int yCoordinate = y + 1;
                    int firstPart = ((rackId * yCoordinate + input) * rackId);
                    cells[x, y] = (((firstPart - (firstPart % 100)) / 100) % 10) - 5;
                    if (x == 216 && y == 195)
                    {
                        Console.WriteLine();
                    }
                }
            var value = (0, 0, 0, 0);
            for (int s = 1; s <= 300; s++)
                for (int x = 0; x < GRID_SIZE - s; x++)
                    for (int y = 0; y < GRID_SIZE - s; y++)
                    {
                        int sum = 0;
                        for (int t = x; t < x + s; t++)
                            for (int u = y; u < y + s; u++)
                            {
                                sum += cells[t, u];
                            }
                        if (sum > value.Item4)
                        {
                            value = (x + 1, y + 1, s, sum);
                        }


                    }
            Console.WriteLine(value);

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

}