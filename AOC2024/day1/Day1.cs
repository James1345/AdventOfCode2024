namespace AOC2024.day1;

public abstract class Day1
{
    public static void Invoke()
    {
        var (left, right) = ReadInput();

        Console.WriteLine(Distance(left, right));
        
        Console.WriteLine(Similarity(left, right));
    }

    private static (List<int> left, List<int> right) ReadInput()
    {
        var input = File.ReadAllLines("day1/input.txt");
        var left = new List<int>();
        var right = new List<int>();
        foreach (var line in input)
        {
            var lr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(lr[0]));
            right.Add(int.Parse(lr[1]));
        }
        left.Sort();
        right.Sort();
        return (left, right);
    }

    private static int Distance(List<int> left, List<int> right) =>
        left.Zip(right)
            .Sum(x => Math.Abs(x.First - x.Second));
    
    private static int Similarity(List<int> left, List<int> right)
    {
        Dictionary<int, int> rightFrequency = new();
        foreach (var it in right)
        {
            rightFrequency[it] = rightFrequency.GetValueOrDefault(it) + 1;
        }
        
        return left.Sum(it => it * rightFrequency.GetValueOrDefault(it));
    }
}