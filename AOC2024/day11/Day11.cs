namespace AOC2024.day11;

public class Day11
{
    public static void Invoke()
    {
        IEnumerable<string> stones = File.ReadAllText("day11/input.txt").Split(' ');
        for (var i = 0; i < 25; i++)
        {
            stones = stones.SelectMany<string, string>(it =>
            {
                if (it == "0") return ["1"];
                if(it.Length % 2 == 0) return [long.Parse(it[..(it.Length / 2)]).ToString(), long.Parse(it[(it.Length / 2)..]).ToString()]; 
                return [(long.Parse(it) * 2024).ToString()];
            });
        }
        Console.WriteLine(stones.Count());
    }
}