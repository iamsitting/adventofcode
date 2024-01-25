
using System.Drawing;

namespace AOC.Day3;

public static class Day3Part1
{
    public record Pos(int X, int Y);
    private const string FileName = "day3.txt";
    private static string ReadFile()
    {
        return File.ReadAllText(FileName);

    }
    private static bool IsSymbol(char ch)
    {
        if (ch == '.') return false;
        else return ch < '0' || ch > '9';
    }
    private static int Solve(string input)
    {
        var lines = input.Split('\n');
        var x = lines[0].Length;
        var y = lines.Length;
        Console.WriteLine($"Width: {x}");
        Console.WriteLine($"Length: {y}");

        int[] nums = [];

        for (var j = 0; j < y; j++)
        {
            Console.WriteLine($"line: {j + 1}");
            Pos? startPos = null;
            Pos? endPos = null;
            bool shouldAdd = false;
            for (var i = 0; i < x; i++)
            {
                var ch = lines[j][i];
                if ('0' <= ch && ch <= '9')
                {
                    if (startPos == null) // current char is beginning of a new number
                    {
                        startPos = new Pos(i, j);
                        if (i > 0 && IsSymbol(lines[j][i - 1])) shouldAdd = true; // left
                    }

                    endPos = new Pos(i, j); // update end position

                    if (!shouldAdd) // if has not been added, check if it should
                    {
                        if (j > 0 && IsSymbol(lines[j - 1][i])) shouldAdd = true; // top
                        else if (j < y - 1 && IsSymbol(lines[j + 1][i])) shouldAdd = true; // bottom
                        else if (i > 0 && j > 0 && IsSymbol(lines[j - 1][i - 1])) shouldAdd = true; // top-left
                        else if (i > 0 && j < y - 1 && IsSymbol(lines[j + 1][i - 1])) shouldAdd = true; // bottom-left
                        else if (i < x - 1 && j > 0 && IsSymbol(lines[j - 1][i + 1])) shouldAdd = true; // top-right
                        else if (i < x - 1 && j < y - 1 && IsSymbol(lines[j + 1][i + 1])) shouldAdd = true; // bottom-right
                    }


                    if (i == x - 1) // if at the end of the line
                    {
                        var numStr = lines[startPos.Y][startPos.X..(endPos!.X + 1)];
                        if (shouldAdd)
                        {
                            Console.WriteLine($"Added: {numStr}");
                            nums = [.. nums, int.Parse($"{numStr}")];
                            shouldAdd = false;
                        }
                        startPos = null;
                        endPos = null;
                    }

                }
                else if (startPos != null) // a number has been started, and now the number has ended
                {

                    // if not added, check if end char is a symbol
                    if (!shouldAdd && IsSymbol(lines[j][i])) shouldAdd = true;

                    var numStr = lines[startPos.Y][startPos.X..(endPos!.X + 1)];
                    if (shouldAdd)
                    {
                        Console.WriteLine($"Added: {numStr}");
                        nums = [.. nums, int.Parse($"{numStr}")];
                        shouldAdd = false;
                    }
                    startPos = null;
                    endPos = null;
                }
            }
        }
        return nums.Sum();
    }

    public static void Test()
    {
        var input =
@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        var result = Solve(input);
        if (result == 4361)
        {
            Console.WriteLine("Test passed");
        }
        else
        {
            Console.WriteLine($"Test failed - expected {4361}, got {result}");
        }
    }

    public static void GetAnswer()
    {
        var input = ReadFile();
        var answer = Solve(input);
        Console.WriteLine(answer);
    }
}

public static class Day3Part2
{

    private const string FileName = "day3.txt";
    private static string ReadFile()
    {
        return File.ReadAllText(FileName);

    }
    private static bool IsDigit(char ch) => ch >= '0' && ch <= '9';
    
    private static int Solve(string input)
    {
        var lines = input.Split('\n');
        int[] products = [];
        for (var i = 0; i < lines.Length; i++)
        {
            Console.WriteLine($"Line {i + 1}");
            for (var j = 0; j < lines[i].Length; j++)
            {
                var ch = lines[i][j];
                int[] gearRatios = [];
                char[] gearRatio = [];
                if (ch != '*') continue;

                // to the left
                for (var k = j - 1; k >= 0; k--)
                {
                    if (IsDigit(lines[i][k])) gearRatio = [lines[i][k], .. gearRatio];
                    else break;
                }
                if (gearRatio.Length > 0)
                {
                    Console.WriteLine($"{int.Parse(gearRatio)}");
                    gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                    gearRatio = [];
                }

                // to the right
                for (var k = j + 1; k < lines[i].Length; k++)
                {
                    if (IsDigit(lines[i][k])) gearRatio = [.. gearRatio, lines[i][k]];
                    else break;
                }
                if (gearRatio.Length > 0)
                {
                    Console.WriteLine($"{int.Parse(gearRatio)}");
                    gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                    gearRatio = [];
                }

                // to the top
                var up = i - 1;
                if (IsDigit(lines[up][j])) // top is digit, means there is only one number
                {
                    gearRatio = [lines[up][j]];
                    for (var k = j - 1; k >= 0; k--) // go to the left
                    {
                        if (IsDigit(lines[up][k])) gearRatio = [lines[up][k], .. gearRatio];
                        else break;
                    }
                    // go to the right
                    for (var k = j + 1; k < lines[up].Length; k++)
                    {
                        if (IsDigit(lines[up][k])) gearRatio = [.. gearRatio, lines[up][k]];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                }
                else // top is non-digit, could be two numbers
                {
                    for (var k = j - 1; k >= 0; k--) // go to the left
                    {
                        if (IsDigit(lines[up][k])) gearRatio = [lines[up][k], .. gearRatio];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                    // go to the right
                    for (var k = j + 1; k < lines[up].Length; k++)
                    {
                        if (IsDigit(lines[up][k])) gearRatio = [.. gearRatio, lines[up][k]];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                }

                // to the bottom
                var down = i + 1;
                if (IsDigit(lines[down][j])) // means there is only one number
                {
                    gearRatio = [lines[down][j]];
                    for (var k = j - 1; k >= 0; k--) // go to the left
                    {
                        if (IsDigit(lines[down][k])) gearRatio = [lines[down][k], .. gearRatio];
                        else break;
                    }
                    // go to the right
                    for (var k = j + 1; k < lines[down].Length; k++)
                    {
                        if (IsDigit(lines[down][k])) gearRatio = [.. gearRatio, lines[down][k]];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                }
                else // top is symbol, could be two numbers
                {
                    for (var k = j - 1; k > 0; k--) // go to the left
                    {
                        if (IsDigit(lines[down][k])) gearRatio = [lines[down][k], .. gearRatio];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                    // go to the right
                    for (var k = j + 1; k < lines[down].Length; k++)
                    {
                        if (IsDigit(lines[down][k])) gearRatio = [.. gearRatio, lines[down][k]];
                        else break;
                    }
                    if (gearRatio.Length > 0)
                    {
                        Console.WriteLine($"{int.Parse(gearRatio)}");
                        gearRatios = [.. gearRatios, int.Parse(gearRatio)];
                        gearRatio = [];
                    }
                }

                if(gearRatios.Length == 2)
                {
                    products = [..products, gearRatios.Aggregate((a, x) => a * x)];
                    gearRatios = [];
                }

            }
        }

        return products.Sum();

    }
    public static void Test()
    {
        var input =
@"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

        var result = Solve(input);
        if (result == 467835)
        {
            Console.WriteLine("Test passed");
        }
        else
        {
            Console.WriteLine($"Test failed - expected {467835}, got {result}");
        }
    }

    public static void GetAnswer()
    {
        var input = ReadFile();
        var answer = Solve(input);
        Console.WriteLine(answer);
    }
}