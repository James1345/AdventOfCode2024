namespace AOC2024.day12;

public class Day12
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day12/input.txt");

//         input =
// """
// RRRRIICCFF
// RRRRIICCCF
// VVRRRCCFFF
// VVRCCCJFFF
// VVVVCJJCFE
// VVIVCCJJEE
// VVIIICJJEE
// MIIIIIJJEE
// MIIISIJEEE
// MMMISSJEEE
// """.Split('\n');
        
        
        (int w, int h) size = (input[0].Length, input.Length);
        
        var set = new HashSet<(int x, int y)>();

        var total = 0;

        for (var y =0; y < size.h; y++)
        {
            for (var x = 0; x < size.w; x++)
            {
                if (set.Contains((x, y)))
                {
                    continue;
                }
                
                var value = input[y][x];
                var q = new Queue<(int x, int y)>();
                q.Enqueue((x, y));

                var area = 0;
                var perimeter = 0;
                var inside = new HashSet<(int x, int y)>();

                while (q.Count > 0)
                {
                    var (px, py) = q.Dequeue();
                    
                    if(inside.Contains((px, py))) continue;

                    if (px >= 0 && py >= 0 && px < size.w && py < size.h && input[py][px] == value)
                    {
                        area++;
                        set.Add((px, py));
                        inside.Add((px, py));
                        
                        q.Enqueue((px - 1, py));
                        q.Enqueue((px + 1, py));
                        q.Enqueue((px, py - 1));
                        q.Enqueue((px, py + 1));
                        
                    }
                    else
                    {
                        perimeter++;
                    }
                }
                
                total += perimeter * area;
            }
        }
        Console.WriteLine(total);
    }
    
}