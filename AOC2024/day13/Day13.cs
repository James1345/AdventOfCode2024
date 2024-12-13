using System.Text.RegularExpressions;

namespace AOC2024.day13;

public class Day13
{

    public static void Invoke()
    {
        var input = File.ReadAllLines("day13/input.txt");
        var conditions = Conditions.Parse(input);
        Console.WriteLine(conditions.Sum(it => it.Cost));
    }
    
    private record Button(int X, int Y, int Cost);

    private record Conditions(Button A, Button B, long X, long Y)
    {
        public static List<Conditions> Parse(string[] input)
        {
            var conditions = new List<Conditions>(input.Length / 4);
            for (var i = 0; i < input.Length; i += 4)
            {
                var a = Regex.Match(input[i], @"Button A: X\+(\d+), Y\+(\d+)");
                var b = Regex.Match(input[i + 1], @"Button B: X\+(\d+), Y\+(\d+)");
                var prize = Regex.Match(input[i + 2], @"Prize: X=(\d+), Y=(\d+)");

                conditions.Add(new Conditions(
                    new Button(int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value), 3),
                    new Button(int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value), 1),
                    int.Parse(prize.Groups[1].Value),
                    int.Parse(prize.Groups[2].Value)
                ));
            }
            return conditions;
        }

        public long Cost => Solve()?.Cost ?? 0;

        public Solution? Solve()
        {
            var beta = (double)(Y * A.X - X * A.Y) / (B.Y*A.X - B.X * A.Y);
            
            if(Math.Abs(Math.Floor(beta) - Math.Ceiling(beta)) > 0.001) return null;

            var alpha = (X - beta * B.X) / A.X;
            
            if(Math.Abs(Math.Floor(alpha) - Math.Ceiling(alpha)) > 0.001) return null;
            
            return new Solution(this, (long)alpha, (long)beta);
        }
    };

    private record Solution(Conditions Condition, long Alpha, long Beta)
    {
        public long Cost => Alpha * Condition.A.Cost + Beta * Condition.B.Cost;
    }
}