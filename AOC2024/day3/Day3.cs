using System.Text.RegularExpressions;

namespace AOC2024.day3;

public abstract partial class Day3
{
    public static void Invoke()
    {
        var input = File.ReadAllText("day3/input.txt");
        
        var total = ExecuteMuls(input);

        Console.WriteLine(total);
        

        var chunks = input.Split("don't()").ToList();
        
        List<string> enabledChunks = [chunks.First()];
        chunks.RemoveAt(0);

        foreach (var chunk in chunks.ToList())
        {
            var subChunks = chunk.Split("do()").ToList();
            subChunks.RemoveAt(0);
            enabledChunks.AddRange(subChunks);
        }
        
        Console.WriteLine(enabledChunks.Sum(ExecuteMuls));

    }

    private static int ExecuteMuls(string input)
    {
        var matchCollection = MulRegex().Matches(input);

        var total = matchCollection.Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        return total;
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();
}