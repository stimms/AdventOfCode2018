using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day4
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var times = new List<Tuple<DateTime, string>>();
            foreach (var line in lines)
            {
                var (_, date, action) = line.Split(new char[] { '[', ']' });
                times.Add(new Tuple<DateTime, string>(DateTime.Parse(date), action.Trim()));
            }
            var ordered = times.OrderBy(x => x.Item1);
            var asleep = new Dictionary<int, int>();
            var currentGuard = 0;
            var startedSleeping = new DateTime();
            foreach (var item in ordered)
            {
                if (item.Item2.StartsWith("Guard"))
                {
                    currentGuard = Int32.Parse(item.Item2.Split(new char[] { ' ', '#' })[2]);
                }
                if (item.Item2.StartsWith("fall"))
                {
                    startedSleeping = item.Item1;
                }
                if (item.Item2.StartsWith("wake"))
                {
                    if (asleep.ContainsKey(currentGuard))
                    {
                        asleep[currentGuard] += (item.Item1 - startedSleeping).Minutes;
                    }
                    else
                    {
                        asleep.Add(currentGuard, (item.Item1 - startedSleeping).Minutes);
                    }
                }
            }
            int sleepiest = asleep.OrderByDescending(x => x.Value).First().Key;
            var minutesAsleept = new Dictionary<int, int>();
            for (var i = 0; i < 60; i++)
            {
                minutesAsleept.Add(i, 0);
            }
            foreach (var item in ordered)
            {
                if (item.Item2.StartsWith("Guard"))
                {
                    currentGuard = Int32.Parse(item.Item2.Split(new char[] { ' ', '#' })[2]);
                }
                if (item.Item2.StartsWith("fall"))
                {
                    startedSleeping = item.Item1;
                }
                if (item.Item2.StartsWith("wake"))
                {
                    if (currentGuard == sleepiest)
                    {
                        for (var i = startedSleeping.Minute; i < item.Item1.Minute; i++)
                        {
                            minutesAsleept[i]++;
                        }
                    }

                }
            }

            Console.WriteLine($"Guard {sleepiest}");
            Console.WriteLine($"Slept for {minutesAsleept.OrderByDescending(x => x.Value).First().Key} minutes");
            Console.WriteLine(">>>" + sleepiest * minutesAsleept.OrderByDescending(x => x.Value).First().Key);

            //part 2

            var sleepiestMinute = new Dictionary<int, Dictionary<int, int>>();//guard, minute, count
            foreach (var guard in ordered.Where(x=>x.Item2.StartsWith("Guard")).Select(x => Int32.Parse(x.Item2.Split(new char[] { ' ', '#' })[2])).Distinct())
            {
                var minuteDict = new Dictionary<int, int>();
                for (var i = 0; i < 60; i++)
                {
                    minuteDict.Add(i, 0);
                }
                sleepiestMinute.Add(guard, minuteDict);
            }
            foreach (var item in ordered)
            {
                if (item.Item2.StartsWith("Guard"))
                {
                    currentGuard = Int32.Parse(item.Item2.Split(new char[] { ' ', '#' })[2]);
                }
                if (item.Item2.StartsWith("fall"))
                {
                    startedSleeping = item.Item1;
                }
                if (item.Item2.StartsWith("wake"))
                {

                    for (var i = startedSleeping.Minute; i < item.Item1.Minute; i++)
                    {
                        sleepiestMinute[currentGuard][i]++;
                    }


                }
            }
            var sleepiestGuardMinute = sleepiestMinute.Select(x => new Tuple<int, KeyValuePair<int, int>>(x.Key, x.Value.OrderByDescending(y => y.Value).First())).OrderByDescending(x => x.Item2.Value).First();



            Console.WriteLine($"Guard id {sleepiestGuardMinute.Item1}");
            Console.WriteLine($"Slept the most at minute {sleepiestGuardMinute.Item2.Key}({sleepiestGuardMinute.Item2.Value} times)");
            Console.WriteLine(">>>" + sleepiestGuardMinute.Item1 * sleepiestGuardMinute.Item2.Key);

            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

}
