using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting");

            var lines = File.ReadAllLines("input.txt");
            int twoCounter = 0;
            int threeCounter = 0;
            foreach(var line in lines){
            var groups = line.ToCharArray().GroupBy(l => l)
                        .Select(group => new { 
                             Metric = group.Key, 
                             Count = group.Count() 
                        });
                if(groups.Any(x => x.Count == 2))
                {
                    twoCounter++;
                }
                if(groups.Any(x => x.Count == 3))
                {
                    threeCounter++;
                }
                 
            }
            Console.WriteLine(twoCounter * threeCounter);
            foreach(var line in lines){
                foreach(var line2 in lines){
                    var diffCounter = 0;
                    for(int i = 0; i< line2.Length; i++){
                        if(line[i]!=line2[i]){
                            diffCounter++;
                        }
                    }
                    if(diffCounter == 1){
                        Console.WriteLine(line);
                        Console.WriteLine(line2);
                    }
                    
                }
                
            }
            Console.WriteLine("done.");
            
            Console.ReadLine();
        }
    }
}
