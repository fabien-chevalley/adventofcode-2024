using System.Diagnostics;

namespace AdventOfCode.Puzzles;

using FreeBlock = List<Day9Puzzle.Block>;
using Files = List<Day9Puzzle.Block>;

public class Day9Puzzle : Puzzle
{
    public override async ValueTask<long> PartOne()
    {
        var input = await File.ReadAllTextAsync(Filename);
        var diskMap = input
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        var disk = GetDiskData(diskMap);

        var index = 0;
        var moveIndex = disk.Count - 1;

        while (index < moveIndex)
            if (disk[index] == null)
            {
                if (disk[moveIndex] == null)
                    moveIndex--;
                else
                    (disk[index], disk[moveIndex]) = (disk[moveIndex], disk[index]);
            }
            else
            {
                index++;
            }

        return Checksum(disk);
    }

    public override async ValueTask<long> PartTwo()
    {
        var input = await File.ReadAllTextAsync(Filename);
        var diskMap = input
            .Select(x => int.Parse(x.ToString()))
            .ToArray();

        var disk = GetDiskData(diskMap);
        var (freeBlocks, fileBlocks) = PrepareData(diskMap);
        
        for (var i = fileBlocks.Count - 1; i >= 0; i--)
        {
            var fileBlock = fileBlocks[i];
            for (var j = 0; j < freeBlocks.Count; j++)
            {
                var freeBlock = freeBlocks[j];
                if (fileBlock.Index < freeBlock.Index) break;

                var block = SwappableBlock(freeBlock, fileBlock);
                if (block != null)
                {
                    freeBlocks[j] = block;
                    UpdateDisk(disk, fileBlock, freeBlock);
                    break;
                }
            }
        }

        return Checksum(disk);
    }

    private static void UpdateDisk(List<int?> disk, Block fileBlock, Block freeBlock)
    {
        for (var i = 0; i < fileBlock.Length; i++)
        {
            var previousFileValue = disk[fileBlock.Index + i];
            var previousFreeValue = disk[freeBlock.Index + i];
            disk[freeBlock.Index + i] = previousFileValue;
            disk[fileBlock.Index + i] = previousFreeValue;
        }
    }

    private static Block? SwappableBlock(Block free, Block file)
    {
        return free.Length >= file.Length ? 
            new Block(free.Index + file.Length, free.Length - file.Length)
            : null;
    }

    private List<int?> GetDiskData(int[] diskMap)
    {
        var id = 0;
        var disk = new List<int?>();

        for (var i = 0; i < diskMap.Length; i++)
        {
            if (i % 2 == 0)
            {
                for (var j = 0; j < diskMap[i]; j++) disk.Add(id);
                id++;
            }
            else
            {
                for (var j = 0; j < diskMap[i]; j++) disk.Add(null);
            }
        }
        
        return disk;
    }
    
    private (FreeBlock, Files) PrepareData(int[] diskMap)
    {
        var freeBlocks = new FreeBlock();
        var files = new Files();

        var position = 0;
        for (var i = 0; i < diskMap.Length; i++)
        {
            var block = new Block(position, diskMap[i]);
            if (i % 2 == 0)
            {
                files.Add(block);
            }
            else
            {
                freeBlocks.Add(block);
            }

            position += diskMap[i];
        }
        
        return (freeBlocks, files);
    }

    private long Checksum(List<int?> disk)
    {
        long checksum = 0;
        for (var i = 0; i < disk.Count; i++)
        {
            var value = disk[i];
            if (value != null)
            {
                checksum += value.Value * i;
            }
        }

        return checksum;
    }

    public record Block(int Index, int Length);
}