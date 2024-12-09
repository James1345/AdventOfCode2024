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
        
        var nmem = NMemory.Parse(input);
        nmem.Compress();
        Console.WriteLine(nmem.Checksum());
    }

    private class NMemory(List<FileSpan> files, List<FreeSpan> free)
    {
        public static NMemory Parse(string input)
        {
            int id = 0;
            int ptr = 0;
            var isFileMode = true;
            
            var files = new List<FileSpan>();
            var free = new List<FreeSpan>();
            
            foreach (var fragment in input)
            {
                var length = int.Parse(fragment.ToString());

                if (isFileMode)
                {
                    files.Add(new FileSpan(ptr, length, id));
                    id++;
                    ptr += length;
                }
                else
                {
                    free.Add(new FreeSpan(ptr, length));
                    ptr += length;
                }

                isFileMode = !isFileMode;
            }
            
            return new NMemory(files, free);
        }

        public void Compress()
        {
            foreach (var file in files.OrderByDescending(it => it.Id))
            {
                foreach (var freeSpan in free.OrderBy(it => it.Start))
                {
                    if(freeSpan.Size < file.Size) continue;
                    if(freeSpan.Start > file.Start) continue;
                    
                    file.Start = freeSpan.Start;
                    
                    freeSpan.Size -= file.Size;
                    freeSpan.Start += file.Size;

                    if (freeSpan.Size == 0)
                    {
                        free.Remove(freeSpan);
                    }
                    break;
                }
            }
        }

        public long Checksum() => files.Sum(it => it.Checksum());
    }

    private record FileSpan(int Start, int Size, int Id)
    {
        public long Checksum() => Enumerable.Range(Start, Size).Sum(i => (long)i * Id);
        public int Start { get; set; } = Start;
    }

    private record FreeSpan(int Start, int Size)
    {
        public int Start { get; set; } = Start;
        public int Size { get; set; } = Size;
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
            var mode = FileBlock.Mode;
            foreach (var fragment in input)
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

    private interface IMemoryBlock
    {
    }

    private record FileBlock(int Id) : IMemoryBlock
    {
        public static readonly int Mode = 0;
    };

    private class FreeBlock : IMemoryBlock
    {
        public static readonly int Mode = 1;

        private FreeBlock()
        {
        }

        public static readonly FreeBlock Instance = new();
    };
}