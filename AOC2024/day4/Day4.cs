using System.Text;

namespace AOC2024.day4;

public abstract class Day4
{
    public static void Invoke()
    {
        var input = File.ReadAllLines("day4/input.txt");
        var data = input.Select(line => line.ToList()).ToList();
        
        var grid = new Grid(data);
        
        Console.WriteLine(grid.CountWord("XMAS", Enum.GetValues<Direction>()));
        Console.WriteLine(grid.CountMasCross());
    }
}
 
public enum Direction
{
    North,
    South,
    East,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}

public class Grid(List<List<char>> data)
{
    public int CountMasCross()
    {
        const string word = "MAS";
        var count = 0;
        for (var y = 0; y < data.Count-2; y++)
        {
            for (var x = 0; x < data[y].Count-2; x++)
            {
                var subgrid = Subgrid(x, y, 3, 3);
                if (subgrid.CountWord(word,
                        [Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest]) == 2)
                {
                    count++;
                }
            }
        }
        return count;
    }

    private Grid Subgrid(int x, int y, int width, int height)
    {
        var data1 = new List<List<char>>();
        for (var i = y; i < y + height; i++)
        {
            var row = new List<char>();
            for (var j = x; j < x + width; j++)
            {
                row.Add(data[i][j]);
            }
            data1.Add(row);
        }

        return new Grid(data1);
    }
    
    public int CountWord(string word, IList<Direction> directions)
    {
        var count = 0;
        for (var y = 0; y < data.Count; y++)
        {
            for (var x = 0; x < data[y].Count; x++)
            {
                foreach (var direction in directions)
                {
                    var wordAt = Word(x, y, direction, word.Length);
                    if (wordAt == word)
                    {
                        count++;
                    }
                }
            }
        }
        return count;
    }

    private string? Word(int startX, int startY, Direction direction, int length)
    {
        try
        {
            var word = new StringBuilder(length);
            var (x, y) = (startX, startY);
            for (var i = 0; i < length; i++)
            {
                word.Append(data[y][x]);
                (x, y) = Move(x, y, direction);
            }

            return word.ToString();
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }

    private static (int x, int y) Move(int x, int y, Direction direction) =>
        direction switch
        {
            Direction.North => (x, y - 1),
            Direction.South => (x, y + 1),
            Direction.East => (x + 1, y),
            Direction.West => (x - 1, y),
            Direction.NorthEast => (x + 1, y - 1),
            Direction.NorthWest => (x - 1, y - 1),
            Direction.SouthEast => (x + 1, y + 1),
            Direction.SouthWest => (x - 1, y + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
}