using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Day12
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input.txt");
            var state = new HashSet<long>();

            int charCounter = 0;
            foreach (var c in lines.First())
            {
                if (c == '#')
                    state.Add(charCounter);
                charCounter++;
            }

            var rules = new List<(bool, bool, bool, bool, bool, bool)>();
            foreach (var line in lines.Skip(2))
            {
                rules.Add((line[0] == '#', line[1] == '#', line[2] == '#', line[3] == '#', line[4] == '#', line[9] == '#'));
            }
            var seenStates = new List<(long, List<long>, HashSet<long>)>();
            var normalizationFactor = state.Min();
            var normalizedState = state.Select(x => x - normalizationFactor).ToList();
            seenStates.Add((normalizationFactor, normalizedState, state));
            var jumped = false;
            for (long i = 0; i < 50_000_000_000; i++)
            {
                var newState = new HashSet<long>();

                for (long j = state.Min() - 2; j < state.Max() + 2; j++)
                {

                    (bool, bool, bool, bool, bool, bool) matchingRule = rules.SingleOrDefault(x => x.Item1 == state.Contains(j - 2) &&
                            x.Item2 == state.Contains(j - 1) &&
                            x.Item3 == state.Contains(j) &&
                            x.Item4 == state.Contains(j + 1) &&
                            x.Item5 == state.Contains(j + 2));
                    if (matchingRule.Item6)
                        newState.Add(j);
                }

                state = newState;
                normalizationFactor = state.Min();
                normalizedState = state.Select(x => x - normalizationFactor).ToList();

                var seen = false;
                if (!jumped)
                {
                    foreach (var seenState in seenStates)
                    {
                        if (seenState.Item2.Zip(normalizedState, (a, b) => a - b).All(x => x == 0))
                        {
                            Console.WriteLine("Loop detected at " + i + " and normalization factor " + normalizationFactor);
                            seen = true;
                            long blockSize = i - seenState.Item1;
                            var blockCount = ((50_000_000_000 - i) / blockSize);
                            var destItteration = i + (blockCount * (blockSize));

                            state = newState.Select(x => (x + (blockCount * blockSize))).ToHashSet();
                            i = destItteration;
                            Console.WriteLine();
                            jumped = true;
                        }
                    }
                    if (!seen)
                    {
                        seenStates.Add((normalizationFactor, normalizedState, newState));
                    }
                }

            }
            long sum = 0;
            for (int i = 0; i < state.Count; i++)
            {
                sum += state.ToList()[i];
            }
            Console.WriteLine(sum);
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

}