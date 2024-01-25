
namespace AOC.Day2;
public static class Day2Part2
{
    private const string FileName = "day2.txt";
    private static string ReadFile()
    {
        return File.ReadAllText(FileName);
    }
    
    private static Game[] ParseToGames(string input)
    {
        string[] lines = input.Split('\n');
        Game[] games = [];
        for(var id = 0; id < lines.Length; id++)
        {
            var draws = lines[id]   // "Game 1: 3 blue, 3 green; 2 blue, 1 red; 3 blue, 3 green; 2 blue, 2 green"
                .Split(":")[1]      // ["3 blue, 3 green; 2 blue, 1 red; 3 blue, 3 green; 2 blue, 2 green"]
                .Trim().Split(';'); // ["3 blue, 3 green", "2 blue, 1 red", "3 blue, 3 green", "2 blue, 2 green"]

            CubeDraw[] cubeDraws = [];
            foreach(var draw in draws) // "3 blue, 3 green"
            {
                var g = 0;
                var b = 0;
                var r = 0;
                if (draw.Contains("green"))
                {
                    _ = int.TryParse(draw.Split(',') // ["3 blue", " 3 green"]
                        .First(x => x.Contains("green")).Trim() // "3 green"
                        .Split(' ') // ["3", "green"]
                        .First().Trim(), out g); // 3
                }
                if (draw.Contains("blue"))
                {
                    _ = int.TryParse(draw.Split(',').First(x => x.Contains("blue")).Trim().Split(' ')[0].Trim(), out b);
                }
                if (draw.Contains("red"))
                {
                    _ = int.TryParse(draw.Split(',').First(x => x.Contains("red")).Trim().Split(' ')[0].Trim(), out r);
                }
                cubeDraws = [..cubeDraws, new CubeDraw(g, b, r)];
                
            }
            games = [..games, new Game(id+1, cubeDraws)];
        }
        return games;
    }

    private static int SolveGames(Game[] games)
    {
        var result = 0;
        foreach(var game in games)
        {
            var g = 0;
            var b = 0;
            var r = 0;
            if(game.CubeDraws.Max(x => x.Green) > g) g = game.CubeDraws.Max(x => x.Green);
            if(game.CubeDraws.Max(x => x.Blue) > b) b = game.CubeDraws.Max(x => x.Blue);
            if(game.CubeDraws.Max(x => x.Red) > r) r = game.CubeDraws.Max(x => x.Red);
            
            Console.WriteLine($"id: {game.Id}");
            Console.WriteLine($"blue: {b}");
            Console.WriteLine($"green: {g}");
            Console.WriteLine($"red: {r}");

            result += g * b * r;
        }
        return result;
    }
    public static void TestSolveGames()
    {
        var games = new Game[] {
            new(1, [
                new CubeDraw(0, 3, 4),
                new CubeDraw(2, 6, 1),
                new CubeDraw(2, 0, 0)
            ]),
            new(2, [
                new CubeDraw(2, 1, 0),
                new CubeDraw(3, 4, 1),
                new CubeDraw(1, 1, 0)
            ]),
            new(3, [
                new CubeDraw(8, 6, 20),
                new CubeDraw(13, 5, 4),
                new CubeDraw(5, 0, 1),
            ]),
            new(4, [
                new CubeDraw(1, 6, 3),
                new CubeDraw(3, 0, 6),
                new CubeDraw(3, 15, 14)
            ]),
            new(5, [
                new CubeDraw(3, 1, 6),
                new CubeDraw(2, 2, 1)
            ])
        };
        var res = SolveGames(games);
        if (res == 2286)
        {
            Console.WriteLine("Test passed");
        }
        else
        {
            Console.WriteLine($"Test failed - expected {2286}, got {res}");
        }
    }
    public static void GetAnswer()
    {
        var input = ReadFile();
        var games = ParseToGames(input);
        var answer = SolveGames(games);
        Console.WriteLine(answer);
    }
}
public static class Day2Part1
{
    private const string FileName = "day2.txt";
    private static string ReadFile()
    {
        return File.ReadAllText(FileName);
    }
    
    private static int SolveGames(Game[] games, CubeDraw cubeDrawToFilterBy)
    {
        var gs = games.Where(x =>
                    x.CubeDraws.All(y => y.Red <= cubeDrawToFilterBy.Red &&
                                    y.Green <= cubeDrawToFilterBy.Green &&
                                    y.Blue <= cubeDrawToFilterBy.Blue));
        return gs.Sum(g => g.Id);
    }
    private static Game[] ParseToGames(string input)
    {
        string[] lines = input.Split('\n');
        Game[] games = [];
        for(var id = 0; id < lines.Length; id++)
        {
            var draws = lines[id].Split(":")[1].Trim().Split(';');
            CubeDraw[] cubeDraws = [];
            foreach(var draw in draws)
            {
                var g = 0;
                var b = 0;
                var r = 0;
                if (draw.Contains("green"))
                {
                    _ = int.TryParse(draw.Split(',').Where(x => x.Contains("green")).First().Trim().Split(' ')[0].Trim(), out g);
                }
                if (draw.Contains("blue"))
                {
                    _ = int.TryParse(draw.Split(',').Where(x => x.Contains("blue")).First().Trim().Split(' ')[0].Trim(), out b);
                }
                if (draw.Contains("red"))
                {
                    _ = int.TryParse(draw.Split(',').Where(x => x.Contains("red")).First().Trim().Split(' ')[0].Trim(), out r);
                }
                cubeDraws = [..cubeDraws, new CubeDraw(g, b, r)];
                
            }
            games = [..games, new Game(id+1, cubeDraws)];
        }
        return games;
    }
    public static void TestParseToGames()
    {
        var input = 
@"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue";
        var games = ParseToGames(input);
        if (games.Length == 2) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {2}, got {games.Length}");

        if (games[0].Id == 1) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {1}, got {games[0].Id}");
        if (games[1].Id == 2) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {2}, got {games[1].Id}");

        if (games[0].CubeDraws.Length == 3) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {3}, got {games[0].CubeDraws.Length}");
        if (games[1].CubeDraws.Length == 3) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {3}, got {games[1].CubeDraws.Length}");

        if (games[0].CubeDraws[0].Blue == 3) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {3}, got {games[0].CubeDraws[0].Blue}");
        if (games[0].CubeDraws[1].Blue == 6) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {6}, got {games[0].CubeDraws[1].Blue}");
        if (games[0].CubeDraws[2].Green == 2) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {2}, got {games[0].CubeDraws[2].Green}");
        if (games[0].CubeDraws[2].Red == 0) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {0}, got {games[0].CubeDraws[2].Red}");


        if (games[1].CubeDraws[0].Red == 0) Console.WriteLine("Test Passed");
        else Console.WriteLine($"Test Failed - expected {0}, got {games[0].CubeDraws[0].Red}");
    }
    public static void TestSolveGames()
    {
        var games = new Game[] {
            new(1, [
                new CubeDraw(0, 3, 1),
                new CubeDraw(2, 6, 1),
                new CubeDraw(2, 0, 0)
            ]),
            new(2, [
                new CubeDraw(2, 1, 0),
                new CubeDraw(0, 4, 1),
                new CubeDraw(1, 1, 0)
            ]),
            new(3, [
                new CubeDraw(8, 6, 20),
                new CubeDraw(13, 5, 4),
                new CubeDraw(5, 0, 1),
            ]),
            new(4, [
                new CubeDraw(1, 6, 3),
                new CubeDraw(3, 0, 6),
                new CubeDraw(3, 15, 14)
            ]),
            new(5, [
                new CubeDraw(3, 1, 6),
                new CubeDraw(2, 2, 1)
            ])
        };
        var res = SolveGames(games, new CubeDraw(14, 13, 12));
        if (res == 8)
        {
            Console.WriteLine("Test passed");
        }
        else
        {
            Console.WriteLine($"Test failed - expected {8}, got {res}");
        }
    }
    public static void TestAll()
    {
        Console.WriteLine("Test Parse Games");
        TestParseToGames();
        Console.WriteLine("Test Solve Games");
        TestSolveGames();
    }

    public static void GetAnswer()
    {
        var input = ReadFile();
        var games = ParseToGames(input);
        var answer = SolveGames(games, new CubeDraw(13, 14, 12));
        Console.WriteLine(answer);
    }
}

public record CubeDraw(int Green, int Blue, int Red);

public record Game(int Id, CubeDraw[] CubeDraws);