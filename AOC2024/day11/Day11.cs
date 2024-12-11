namespace AOC2024.day11;

public class Day11
{
    public static void Invoke()
    {
        IEnumerable<long> stones = File.ReadAllText("day11/input.txt").Split(' ').Select(long.Parse);

        Console.WriteLine(stones.Sum(it => RecursiveStoneUpdate(it, 0)));
    }


    private const int MaxDepth = 75;

    private static Dictionary<(long, int), long> _lazyMap = new();

    private static long RecursiveStoneUpdate(long stone, int depth)
    {
        if (_lazyMap.TryGetValue((stone, depth), out var result))
        {
            return result;
        }

        long update = 1;

        if (depth != MaxDepth)
        {
            if (stone == 0)
            {
                update = RecursiveStoneUpdate(1, depth + 1);
            }
            else
            {
                var length = Len(stone);
                if (length % 2 == 0)
                {
                    var magnitude = Pow10(length / 2);
                    update = RecursiveStoneUpdate(stone / magnitude, depth + 1) +
                             RecursiveStoneUpdate(stone % magnitude, depth + 1);
                }
                else
                {
                    update = RecursiveStoneUpdate(stone * 2024, depth + 1);
                }
            }
        }

        _lazyMap[(stone, depth)] = update;
        return update;
    }

    private static long Pow10(int n)
    {
        var result = 1;
        for (var i = 0; i < n; i++)
        {
            result *= 10;
        }

        return result;
    }

    private static int Len(long it)
    {
        var len = 0;
        while (it > 0)
        {
            it /= 10;
            len++;
        }

        return len;
    }
}