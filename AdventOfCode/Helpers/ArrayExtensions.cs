namespace AdventOfCode.Helpers;

public static class ArrayExtensions
{
    public static char[,] ConvertJaggedToRectangular(this char[][] jaggedArray)
    {
        var rows = jaggedArray.Length;
        var columns = jaggedArray[0].Length;

        var rectangularArray = new char[rows, columns];
        for (var i = 0; i < rows; i++)
        for (var j = 0; j < columns; j++)
            rectangularArray[i, j] = jaggedArray[i][j];

        return rectangularArray;
    }
}