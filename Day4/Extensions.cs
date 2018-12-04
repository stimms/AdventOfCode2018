using System.Collections.Generic;
using System.Linq;
public static class Extensions
{
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