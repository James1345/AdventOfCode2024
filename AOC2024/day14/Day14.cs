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

//         input =
// """
// p=0,4 v=3,-3
// p=6,3 v=-1,-3
// p=10,3 v=-1,2
// p=2,0 v=2,-1
// p=0,0 v=1,3
// p=3,0 v=-2,-2
// p=7,6 v=-1,-3
// p=3,0 v=-1,-2
// p=9,3 v=2,3
// p=7,3 v=-1,2
// p=2,4 v=2,-3
// p=9,5 v=-3,-3
// """.Split("\n");
//
//         w = 11;
//         h = 7;


        var robots = input.Select(Robot.Parse);
        var quadrants = robots
            .GroupBy(it => it.Quadrant(t, w, h))
            .Where(it => it.Key != null)
            .Select(it => it.Count());
        var score = quadrants.Aggregate(1L, (acc, cur) =>  acc * cur);
        Console.WriteLine(score);
    }

    private partial record Robot(int X0, int Y0, int Vx, int Vy)
    {
        public (int X, int Y) PositionAt(int t, int width, int height) => (
            Rem(X0 +(Vx * t), width),
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