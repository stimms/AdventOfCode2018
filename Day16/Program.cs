using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Day16
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
            var ops = new List<OpCode>
            {
                new Addr(),
                new Addi(),
                new Mulr(),
                new Muli(),
                new Banr(),
                new Bani(),
                new Borr(),
                new Bori(),
                new Setr(),
                new Seti(),
                new Gtir(),
                new Gtri(),
                new Gtrr(),
                new Eqir(),
                new Eqri(),
                new Eqrr()
            };
            Assert.Equal(16, ops.Select(x => x.GetType().Name).Distinct().Count());
            int counter = 0;
            var inputs = GetInputs();

            var possibleOpMapping = new List<List<OpCode>>();
            for (int i = 0; i < ops.Count; i++)
                possibleOpMapping.Add(ops.Select(x => x).ToList());

            foreach (var input in inputs)
            {
                var same = 0;
                foreach (var code in ops)
                {
                    var output = new int[4];
                    Array.Copy(input.Item1, output, 4);
                    output[input.Item2[3]] = code.Execute(input.Item2[1], input.Item2[2], input.Item1);
                    var out1 = output.Zip(input.Item3, (a, b) => a == b);

                    if (out1.All(x => x))
                        same++;

                    if (!out1.All(x => x))
                    {
                        possibleOpMapping[input.Item2[0]] = possibleOpMapping[input.Item2[0]].Where(x => x.GetType() != code.GetType()).ToList();
                    }
                }
                if (same >= 3)
                    counter++;
            }
            Console.WriteLine("Part 1: " + counter);

            OpCode[] finalMappings = GetOpCodeMapping(ref possibleOpMapping);
            int[] registers = ExecuteCode(finalMappings);
            Console.WriteLine("Part 2: " + registers[0]);

        }

        private static int[] ExecuteCode(OpCode[] finalMappings)
        {
            var registers = new int[] { 0, 0, 0, 0 };
            foreach (var line in File.ReadAllLines("input2.txt"))
            {
                var values = line.Split(' ').Select(x => Int32.Parse(x)).ToArray();
                registers[values[3]] = finalMappings[values[0]].Execute(values[1], values[2], registers);
            }

            return registers;
        }

        private static OpCode[] GetOpCodeMapping(ref List<List<OpCode>> possibleOpMapping)
        {
            var finalMappings = new OpCode[16];
            while (finalMappings.Where(x => x == null).Any())
            {
                var knownOp = possibleOpMapping.Where(x => x.Count > 0).OrderBy(x => x.Count).First();
                var opCode = possibleOpMapping.IndexOf(knownOp);
                finalMappings[opCode] = knownOp.Single();
                possibleOpMapping = possibleOpMapping.Select(y => y.Where(x => x.GetType() != knownOp.Single().GetType()).ToList()).ToList();
            }

            return finalMappings;
        }

        private static List<(int[], int[], int[])> GetInputs()
        {
            var lines = File.ReadAllLines("input1.txt");
            var result = new List<(int[], int[], int[])>();
            for (int i = 0; i < lines.Length; i += 4)
            {
                var input = lines[i].Split('[', ']', ',');
                var arguments = lines[i + 1].Split(' ');
                var ouput = lines[i + 2].Split('[', ']', ',');
                result.Add((new int[] { Int32.Parse(input[1]), Int32.Parse(input[2]), Int32.Parse(input[3]), Int32.Parse(input[4]) },
                    new int[] { Int32.Parse(arguments[0]), Int32.Parse(arguments[1]), Int32.Parse(arguments[2]), Int32.Parse(arguments[3]) },
                    new int[] { Int32.Parse(ouput[1]), Int32.Parse(ouput[2]), Int32.Parse(ouput[3]), Int32.Parse(ouput[4]) }));
            }
            return result;
        }
        private static void Part2()
        {

        }
    }

    public interface OpCode
    {

        int Execute(int a, int b, int[] registers);
    }

    public class Addr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] + registers[b];
        }
    }

    public class Addi : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] + b;
        }
    }

    public class Mulr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] * registers[b];
        }
    }

    public class Muli : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] * b;
        }
    }

    public class Banr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] & registers[b];
        }
    }

    public class Bani : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] & b;
        }
    }

    public class Borr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] | registers[b];
        }
    }

    public class Bori : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] | b;
        }
    }

    public class Setr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a];
        }
    }

    public class Seti : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return a;
        }
    }

    public class Gtir : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return a > registers[b] ? 1 : 0;
        }
    }

    public class Gtri : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] > b ? 1 : 0;
        }
    }

    public class Gtrr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] > registers[b] ? 1 : 0;
        }
    }

    public class Eqir : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return a == registers[b] ? 1 : 0;
        }
    }

    public class Eqri : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] == b ? 1 : 0;
        }
    }

    public class Eqrr : OpCode
    {
        public int Execute(int a, int b, int[] registers)
        {
            return registers[a] == registers[b] ? 1 : 0;
        }
    }
}