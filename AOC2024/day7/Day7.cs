using System.Collections;
using System.Collections.Specialized;

namespace AOC2024.day7;

public class Day7
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day7/input.txt");
        
        var eqns = input.Select(Eqn.Parse);

        var total = eqns.Where(it => it.Test()).Sum(it => it.TestValue);
        Console.WriteLine(total);
    }

    private record Eqn(long TestValue, List<int> Values)
    {
        public static Eqn Parse(string input)
        {
            var parts = input.Split(':');
            var values = parts[1].Split(' ').Where(it=>!string.IsNullOrWhiteSpace(it)).Select(int.Parse).ToList();
            return new Eqn(long.Parse(parts[0]), values);
        }

        public bool Test()
        {
            var intervals = Values.Count - 1;
            for (int operatorPattern = 0; operatorPattern < Math.Pow(2, intervals); operatorPattern++)
            {
                var operatorQueue = new BitArray([operatorPattern]);
                var valueQueue = new Queue<int>(Values);
                
                long acc = valueQueue.Dequeue();
                var j = 0;
                while (valueQueue.Count != 0)
                {
                    if(operatorQueue[j]) acc += valueQueue.Dequeue();
                    else acc *= valueQueue.Dequeue();
                    j++;
                }

                if (acc == TestValue) return true;
                
            }
            return false;
        }
        
        
    }
    
    
}