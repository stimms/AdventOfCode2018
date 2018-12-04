using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            var claims = new List<Claim>();
            foreach (var line in lines)
            {
                var claim = new Claim();
                claim.id = Int32.Parse(line.Split('@')[0].Substring(1));

                //Console.WriteLine();
                claim.startX = Int32.Parse(line.Split(' ')[2].Split(',')[0]);
                claim.startY = Int32.Parse(line.Split(' ')[2].Split(',')[1].Replace(":", ""));
                claim.width = Int32.Parse(line.Split(' ')[3].Split('x')[0]);
                claim.height = Int32.Parse(line.Split(' ')[3].Split('x')[1]);
                claims.Add(claim);
            }
            var junk = new List<int>();
            var visited = new Dictionary<string, List<int>>();
            foreach (var claim in claims)
            {
                bool alreadySeen = false;
                for (int i = claim.startX; i < claim.startX + claim.width; i++)
                    for (int j = claim.startY; j < claim.startY + claim.height; j++)
                    {
                        List<int> item;
                        if(visited.ContainsKey($"{i},{j}")){
                        item = visited[$"{i},{j}"];
                        }
                        else
                        {
                            //item = new Point { name = , ids = new List<int> { claim.id } };
                            item = new List<int>();
                            visited.Add($"{i},{j}", item);
                        }
                        item.Add(claim.id);
                    }
            }
            // var grps = visited.GroupBy(l => l.name).Select(group => new
            // {
            //     Coords = group.Key,
            //     Count = group.Count()
            // });
            // Console.WriteLine(grps.Where(x => x.Count > 1).Count());
            //Console.WriteLine(claims.Select(x => x.id).Where(x => !junk.Contains(x)).FirstOrDefault());
            Console.WriteLine(claims.Select(x=>x.id).Where(y=> !visited.Where(x => x.Value.Count() > 1).SelectMany(x=>x.Value).Distinct().Contains(y)).Count());
            Console.WriteLine(claims.Select(x=>x.id).Where(y=> !visited.Where(x => x.Value.Count() > 1).SelectMany(x=>x.Value).Distinct().Contains(y)).FirstOrDefault());
            Console.WriteLine("done.");
            Console.ReadLine();
        }
    }

    class Point
    {
        public string name { get; set; }
        public List<int> ids { get; set; }
    }
    class Claim
    {
        public int id { get; set; }
        public int startX { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int startY { get; set; }
    }
}
