using System.Text.RegularExpressions;

namespace AOC2024.day3;

public abstract partial class Day3
{
    public static void Invoke()
    {
        var input = File.ReadAllText("day3/input.txt");
        
        var matchCollection = MulRegex().Matches(input);

        var total = matchCollection.Sum(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value));
        
        Console.WriteLine(total);
    }

    [GeneratedRegex(@"mul\((\d+),(\d+)\)")]
    private static partial Regex MulRegex();
}