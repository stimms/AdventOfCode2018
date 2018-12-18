using System;
using System.Collections.Generic;
using System.Linq;

namespace Day18
{
    public static class Extensions
    {
        public static string JoinArrayToString(this IEnumerable<int> elements)
        {
            return elements.Select(x => x.ToString()).Aggregate((i, j) => $"{i}{j}");
        }
        public static LinkedListNode<int> GetNBack(LinkedListNode<int> current, LinkedList<int> list, int n)
        {
            int counter = 0;
            while (counter < n)
            {
                current = current.Previous;
                if (current == null)
                {
                    current = list.Last;
                }
                counter++;
            }
            return current;
        }

        public static void Print2DArray(this int[,] toPrint, int startx = 0, int starty = 0)
        {
            for (int i = startx; i < toPrint.GetLength(0); i++)
            {
                for (int j = starty; j < toPrint.GetLength(1); j++)
                {
                    Console.Write(toPrint[i, j]);
                }
                Console.WriteLine();
            }
        }
        public static List<(int, int)> AllCombinations(this IList<int> one, IList<int> two)
        {
            var combined = new List<(int, int)>();
            foreach (var i in one)
                foreach (var j in two)
                {
                    combined.Add((i, j));
                }
            return combined;
        }
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
        public static void Deconstruct<T>(this IList<T> list, out T first)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out T third, out T fourth, out T fifth)
        {
            first = list.Count > 0 ? list[0] : default(T); // or throw
            second = list.Count > 1 ? list[1] : default(T); // or throw
            third = list.Count > 2 ? list[2] : default(T); // or throw
            fourth = list.Count > 3 ? list[3] : default(T); // or throw
            fifth = list.Count > 4 ? list[4] : default(T); // or throw
        }
    }
}