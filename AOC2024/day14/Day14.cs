using System.Text.RegularExpressions;

namespace AOC2024.day14;

public partial class Day14
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day14/input.txt");
        var w = 101;
        var h = 103;
        var t = 100;

        var robots = input.Select(Robot.Parse).ToList();
        var quadrants = robots
            .GroupBy(it => it.Quadrant(t, w, h))
            .Where(it => it.Key != null)
            .Select(it => it.Count());
        var score = quadrants.Aggregate(1L, (acc, cur) => acc * cur);
        Console.WriteLine(score);


        // Hint from reddit because this question SUCKS!
        
        for (var t1 = 0; t1 < 100000; t1++)
        {
            var positions = robots.Select(it => it.PositionAt(t1, w, h)).ToHashSet();
            foreach (var (x, y) in positions)
            {
                var cluster = new HashSet<(int x, int y)>();
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        cluster.Add((x + dx, y + dy));
                    }
                }

                if (positions.Intersect(cluster).Count() == 8)
                {
                    Console.WriteLine(t1);
                    Display(positions, w, h);
                }
            }
        }
    }

    private static void Display(HashSet<(int x, int y)> positions, int w, int h)
    {
        for (int y = 0; y < h; y++)
        {
            for (int x = 0; x < w; x++)
            {
                if (positions.Contains((x, y)))
                {
                    Console.Write("#");
                }
                else Console.Write(" ");
            }
            Console.WriteLine();
        }   
    }
    private partial record Robot(int X0, int Y0, int Vx, int Vy)
    {
        public (int X, int Y) PositionAt(int t, int width, int height) => (
            Rem(X0 + (Vx * t), width),
            Rem(Y0 + (Vy * t), height)
        );

        public int? Quadrant(int t, int width, int height)
        {
            var xMid = (width - 1) / 2;
            var yMid = (height - 1) / 2;

            var (x, y) = PositionAt(t, width, height);

            if (x == xMid || y == yMid)
            {
                return null;
            }

            if (x < xMid)
            {
                return y < yMid ? -1 : 1;
            }
            else
            {
                return y < yMid ? -2 : 2;
            }
        }


        public static Robot Parse(string line)
        {
            var match = MyRegex().Match(line);
            return new Robot(
                int.Parse(match.Groups[1].Value),
                int.Parse(match.Groups[2].Value),
                int.Parse(match.Groups[3].Value),
                int.Parse(match.Groups[4].Value)
            );
        }

        [GeneratedRegex(@"^p=(-?\d+),(-?\d+) v=(-?\d+),(-?\d+)")]
        private static partial Regex MyRegex();
    }

    private static int Rem(int x, int divisor) => (x % divisor + divisor) % divisor;
}