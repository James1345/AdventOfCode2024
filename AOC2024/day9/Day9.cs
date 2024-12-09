using System.Text;

namespace AOC2024.day9;

public class Day9
{
    public static void Invoke()
    {
        var input = File.ReadAllText("day9/input.txt");
        var memory = Memory.Parse(input);
        memory.Compress();
        Console.WriteLine(memory.Checksum());
    }

    private class Memory(List<IMemoryBlock> blocks)
    {
        public void Compress()
        {
            var left = 0;
            var right = blocks.Count - 1;

            while (left < right)
            {
                if (blocks[right] == FreeBlock.Instance)
                {
                    right--;
                }
                else if (blocks[left] is FileBlock)
                {
                    left++;
                }
                else
                {
                    blocks[left] = blocks[right];
                    blocks[right] = FreeBlock.Instance;
                }
            }
        }

        public long Checksum()
        {
            long sum = 0;
            for (var i = 0; i < blocks.Count; i++)
            {
                if (blocks[i] is FileBlock file) sum += (long)file.Id * i;
            }
            return sum;
        }

        public static Memory Parse(string input)
        {
            List<IMemoryBlock> blocks = [];
            int id = 0;
            var mode = FileBlock.Mode;            foreach (var fragment in input)
            {
                var length = int.Parse(fragment.ToString());

                if (mode == FileBlock.Mode)
                {
                    for (var i = 0; i < length; i++)
                    {
                        blocks.Add(new FileBlock(id));
                    }
                    mode = FreeBlock.Mode;
                    id++;    
                }
                else if (mode == FreeBlock.Mode)
                {
                    for (var i = 0; i < length; i++)
                    {
                        blocks.Add(FreeBlock.Instance);
                    }
                    mode = FileBlock.Mode;
                }
            }

            return new Memory(blocks);
        }

        public string ToString()
        {
            var builder = new StringBuilder();
            foreach (var block in blocks)
            {

                if (block is FileBlock file)
                {
                    builder.Append(file.Id);
                }
                else
                {
                    builder.Append('.');
                }
            }

            return builder.ToString();
        }
    }

    private interface IMemoryBlock {}

    private record FileBlock(int Id) : IMemoryBlock
    {
        public static readonly int Mode = 0;
    };
    private class FreeBlock : IMemoryBlock
    {
        public static readonly int Mode = 1;
        
        private FreeBlock() {}
        
        public static readonly FreeBlock Instance = new();
    };
    
}