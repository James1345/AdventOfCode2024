namespace AOC2024.day10;

public class Day10
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day10/input.txt");
        var (map, trailheads) = ParseMap(input);
        var score = trailheads.Sum(it => Score(map, it).score);
        var rating = trailheads.Sum(it => Score(map, it).rating);
        Console.WriteLine(score);
        Console.WriteLine(rating);
    }
    
    private static (Dictionary<Point2D, char>, HashSet<Point2D> trailheads) ParseMap(string[] input)
    {
        var map = new Dictionary<Point2D, char>();
        var trailheads = new HashSet<Point2D>();
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                var z = (char)(input[x][y] - '0');
                var xy = new Point2D(x, y);
                
                map[xy] = z ;
                if(z == 0) trailheads.Add(xy);
            }
        }
        
        return (map, trailheads);
    }
    
    private static (int score, int rating) Score(Dictionary<Point2D, char> map, Point2D trailhead)
    {
        var q = new Queue<Point2D>();
        var trailends = new HashSet<Point2D>();
        var rating = 0;
        
        q.Enqueue(trailhead);    

        while (q.Count > 0)
        {
            var xy = q.Dequeue();
            var z = map[xy];
            
            foreach (var next in xy.Next())
            {
                if (!map.TryGetValue(next, out var nextz)) continue;
                
                if (nextz != z + 1) continue;
                
                if (nextz == 9)
                {
                    trailends.Add(next);
                    rating++;
                }
                else q.Enqueue(next);
            }
        }
        
        return (trailends.Count, rating);
    }

    private record Point2D(int X, int Y)
    {
        public List<Point2D> Next() =>
        [
            this with { X = X + 1 },
            this with { X = X - 1 },
            this with { Y = Y + 1 },
            this with { Y = Y - 1 }
        ];
    }
    
}