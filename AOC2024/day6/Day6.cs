namespace AOC2024.day6;

public abstract class Day6
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day6/input.txt");
        
        var (obstacles, guard, size) = Parse(input);

        var visited = new HashSet<Point2D>();

        while (guard.Location is { X: >= 0, Y: >= 0 } && guard.Location.X < size.Width && guard.Location.Y < size.Height)
        {
            visited.Add(guard.Location);
            guard.Move(obstacles);
        }
        
        Console.WriteLine(visited.Count);
    }


    private class Guard(Point2D location, Direction facing)
    {
        public Point2D Location = location;
        private Direction _facing = facing;

        public void Move(HashSet<Point2D> obstacles)
        {
            while (obstacles.Contains(Location.Next(_facing)))
            {
                Rotate();
            }

            Location = Location.Next(_facing);
        }

        private void Rotate()
        {
            _facing = (Direction)(((int)_facing + 1) % 4);
        }
    }


    private static (HashSet<Point2D> obstacles, Guard guard, Size size) Parse(string[] input)
    {
        HashSet<Point2D> obstacles = new();
        Point2D? location = null;

        for (var y = 0; y < input.Length; y++)
        {
            var line = input[y];
            for (var x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    obstacles.Add(new Point2D(x, y));
                }

                if (line[x] == '^')
                {
                    location = new Point2D(x, y);
                }
            }
        }

        return (obstacles, new Guard(location!, Direction.Up), new Size(input[0].Length, input.Length));
    }


    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private record Point2D(int X, int Y)
    {
        public Point2D Next(Direction direction) =>
            direction switch
            {
                Direction.Up => this with { Y = Y - 1 },
                Direction.Down => this with { Y = Y + 1 },
                Direction.Left => this with { X = X - 1 },
                Direction.Right => this with { X = X + 1 },
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }

    private record Size(int Width, int Height);
}