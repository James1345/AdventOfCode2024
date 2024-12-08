namespace AOC2024.day8;

public abstract class Day8
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day8/input.txt");

        var size = new Point2D(input[0].Length, input.Length);

        var antennae = ParseMap(input);

        CalculateAntinodeCount(antennae, size);
        CalculateResonantAntinodeCount(antennae, size);
    }

    private static Dictionary<char, List<Point2D>> ParseMap(string[] input)
    {
        var antennae = new Dictionary<char, List<Point2D>>();

        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                if (input[y][x] != '.')
                {
                    if (!antennae.ContainsKey(input[y][x]))
                    {
                        antennae.Add(input[y][x], new List<Point2D>());
                    }

                    antennae[input[y][x]].Add(new Point2D(x, y));
                }
            }
        }

        return antennae;
    }

    private static void CalculateAntinodeCount(Dictionary<char, List<Point2D>> antennae, Point2D size)
    {
        var antinodes = new HashSet<Point2D>();

        foreach (var (frequency, emitters) in antennae)
        {
            foreach (var a in emitters)
            {
                foreach (var b in emitters.Where(b => a != b))
                {
                    antinodes.UnionWith(Point2D.Antinodes(a, b)
                        .Where(it => it is { X: >= 0, Y: >= 0 } && it.X < size.X && it.Y < size.Y));
                }
            }
        }

        Console.WriteLine(antinodes.Count);
    }

    private static void CalculateResonantAntinodeCount(Dictionary<char, List<Point2D>> antennae, Point2D size)
    {
        var antinodes = new HashSet<Point2D>();

        foreach (var (frequency, emitters) in antennae)
        {
            foreach (var a in emitters)
            {
                foreach (var b in emitters.Where(b => a != b))
                {
                    antinodes.UnionWith(Point2D.ResonantAntinodes(a, b, size)
                        .Where(it => it is { X: >= 0, Y: >= 0 } && it.X < size.X && it.Y < size.Y));
                }
            }
        }

        Console.WriteLine(antinodes.Count);
    }

    private record Point2D(int X, int Y)
    {
        public static List<Point2D> Antinodes(Point2D a, Point2D b)
        {
            var vector = new Point2D(b.X - a.X, b.Y - a.Y);
            return
            [
                new Point2D(a.X - vector.X, a.Y - vector.Y),
                new Point2D(b.X + vector.X, b.Y + vector.Y)
            ];
        }

        public static HashSet<Point2D> ResonantAntinodes(Point2D a, Point2D b, Point2D size)
        {
            var antinodes = new HashSet<Point2D>();
            var vector = new Point2D(b.X - a.X, b.Y - a.Y);
            var absoluteBounds = Math.Min(Math.Abs(size.X / vector.X), Math.Abs(size.Y / vector.Y));
            for (int i = -absoluteBounds; i < absoluteBounds; i++)
            {
                antinodes.Add(
                    new Point2D(a.X - vector.X*i, a.Y - vector.Y*i));
                antinodes.Add(
                    new Point2D(b.X + vector.X*i, b.Y + vector.Y*i));
            }
            return antinodes;
        }
    }
}