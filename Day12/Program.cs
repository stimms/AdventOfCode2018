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
            var state = new List<bool>();
            for (int i = 0; i < 20; i++)
                state.Add(false);
            foreach (var c in lines.First())
            {
                if (c == '.')
                    state.Add(false);
                else
                    state.Add(true);
            }
            for (int i = 0; i < 20; i++)
                state.Add(false);
            var rules = new List<(bool, bool, bool, bool, bool, bool)>();
            foreach (var line in lines.Skip(2))
            {
                rules.Add((line[0] == '#', line[1] == '#', line[2] == '#', line[3] == '#', line[4] == '#', line[9] == '#'));
            }
            Console.WriteLine(String.Join(' ', state.Skip(17).Take(35).Select(x => x ? "#" : ".")).Replace(" ", ""));
            for (int i = 0; i < 20; i++)
            {
                var newState = new List<bool> ();
                for (int j = 0; j < state.Count; j++)
                    newState.Add(false);
                for (int j = 2; j < state.Count - 2; j++)
                {

                    (bool, bool, bool, bool, bool, bool) matchingRule = rules.SingleOrDefault(x => x.Item1 == state[j - 2] &&
                            x.Item2 == state[j - 1] &&
                            x.Item3 == state[j] &&
                            x.Item4 == state[j + 1] &&
                            x.Item5 == state[j + 2]);
                    newState[j] = rules.Any(x => x.Item1 == state[j - 2] &&
                            x.Item2 == state[j - 1] &&
                            x.Item3 == state[j] &&
                            x.Item4 == state[j + 1] &&
                            x.Item5 == state[j + 2]) ? matchingRule.Item6 : false;
                }
                Console.WriteLine(String.Join(' ', state.Skip(17).Take(35).Select(x => x ? "#" : ".")).Replace(" ", ""));
                state = newState;
            }
            Console.WriteLine(state.Select((b, i) => b ? i - 20 : 0).Sum());
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

}