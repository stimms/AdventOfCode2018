using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day8

{
    class Program
    {
        static void Main(string[] args)
        {
            var line = File.ReadAllLines("input.txt").First();
            var numbers = line.Split(' ').Select(x => Int32.Parse(x));
            Console.WriteLine(recurse(numbers.ToArray()));
            Console.WriteLine(recursePart2(numbers.ToArray()));
            Console.ReadLine();
        }

        static (int index, int sum) recursePart2(int[] numbers)
        {
            if (!numbers.Any())
                return (0, 0);
            if (numbers[0] == 0)
            {
                var sum = 0;
                for (int i = 2; i < 2 + numbers[1]; i++)
                {
                    sum += numbers[i];
                }
                return (2 + numbers[1], sum);
            }
            else
            {
                int index = 0;
                var childrenSum = new int[numbers[0]];
                for (int i = 0; i < numbers[0]; i++)
                {
                    var (ind, s) = recursePart2(numbers.SubArray(2 + index, numbers.Length - (2 + index) - numbers[1]));
                    index += ind;
                    childrenSum[i] = s;
                }
                var sum = 0;
                for (int i = index + 2; i < index + 2 + numbers[1]; i++)
                {
                    if (childrenSum.Length > numbers[i]-1)
                        sum += childrenSum[numbers[i]-1];
                }
                return (index + 2 + numbers[1], sum);
            }
        }

        static (int index, int sum) recurse(int[] numbers)
        {
            if (!numbers.Any())
                return (0, 0);
            if (numbers[0] == 0)
            {
                var sum = 0;
                for (int i = 2; i < 2 + numbers[1]; i++)
                {
                    sum += numbers[i];
                }
                return (2 + numbers[1], sum);
            }
            else
            {
                int index = 0;
                int sum = 0;
                for (int i = 0; i < numbers[0]; i++)
                {
                    var (ind, s) = recurse(numbers.SubArray(2 + index, numbers.Length - (2 + index) - numbers[1]));
                    index += ind;
                    sum += s;
                }

                for (int i = index + 2; i < index + 2 + numbers[1]; i++)
                {
                    sum += numbers[i];
                }
                return (index + 2 + numbers[1], sum);
            }
        }
    }
}
