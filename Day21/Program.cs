using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Day21
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();

            Console.WriteLine("done.");
            Console.ReadLine();
        }

        private static void Run()
        {
            var lines = File.ReadAllLines("input1.txt");

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
            var registers = ExecuteCode(ops);
        }

        private static int[] ExecuteCode(List<OpCode> ops)
        {
            var registers = new int[] { 1, 0, 0, 0, 0, 0 };
            var instructionPointer = Int32.Parse(File.ReadAllLines("input1.txt").Single(x => x.StartsWith("#")).Split(" ").Last());
            var lines = File.ReadAllLines("input1.txt").Where(x => !x.StartsWith("#")).ToList();
            int i = 0;
            var initialValues = new List<int>();
            while (registers[instructionPointer] < lines.Count() && registers[instructionPointer] >= 0)
            {
                var line = lines[registers[instructionPointer]];
                if (registers[instructionPointer] == 28)
                {
                    if (initialValues.Contains(registers[3]))
                    {
                        Console.WriteLine("Part 1:\t" + initialValues.First());
                        Console.WriteLine("Part 2:\t" + initialValues.Last());
                        break;
                    }
                    initialValues.Add(registers[3]);
                }

                if (registers[instructionPointer] == 18 && registers[2] == 0)
                {
                    registers[2] = registers[5] / 256;
                }
                var instruction = ops.Where(x => x.GetType().Name.ToLower() == line.Split(' ').First().ToLower()).First();
                var values = line.Split(' ').Skip(1).Take(3).Select(x => Int32.Parse(x)).ToArray();
                registers[values[2]] = instruction.Execute(values[0], values[1], registers);
                registers[instructionPointer]++;
                i++;
            }
            return registers;
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