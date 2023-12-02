namespace CubeConundrum;

public class Program
{
  public async static Task<int> Main(string[] args)
  {
    if (args.Length is 0)
    {
      Console.WriteLine("Please provide a path to the input file.");
      return -1;
    }

    if (File.Exists(args[0]) is false)
    {
      Console.WriteLine("The provided file does not exist.");
      return -2;
    }

    var puzzleParser = new PuzzleParser();
    var puzzleSolver = new PuzzleSolver();

    var input = await File.ReadAllLinesAsync(args[0]);
    var games = input.Select(puzzleParser.ParseGame);

    var result = args.Length > 1 && args[1] == "part2"
      ? puzzleSolver.SumGameMinimumPowers(games)
      : puzzleSolver.SumPossibleGameIds(games);

    Console.WriteLine($"The sum of all possible game ids is {result}.");

    return result;
  }
}

/// <summary>
///  Puzzle solver.
/// </summary>
public class PuzzleSolver
{
  /// <summary>
  /// The maximum number of cubes that can be used in a game.
  /// </summary>
  private readonly int _maxCubeCount = 39;

  /// <summary>
  /// The maximum number of red cubes that can be used in a game.
  /// </summary>
  private readonly int _maxRedCubeCount = 12;

  /// <summary>
  /// The maximum number of green cubes that can be used in a game.
  /// </summary>
  private readonly int _maxGreenCubeCount = 13;

  /// <summary>
  /// The maximum number of blue cubes that can be used in a game.
  /// </summary>
  private readonly int _maxBlueCubeCount = 14;

  /// <summary>
  /// Determines whether a result is possible.
  /// </summary>
  /// <param name="result">The result to check.</param>
  /// <returns>True if the result is possible, otherwise false.</returns>
  public bool IsResultPossible(Result result) => result.TotalCubeCount <= _maxCubeCount
    && result.RedCubeCount <= _maxRedCubeCount
    && result.GreenCubeCount <= _maxGreenCubeCount
    && result.BlueCubeCount <= _maxBlueCubeCount;

  /// <summary>
  /// Determines whether a game is possible.
  /// </summary>
  /// <param name="game">The game to check.</param>
  /// <returns>True if the game is possible, otherwise false.</returns>
  public bool IsGamePossible(Game game) => game.Results.All(IsResultPossible);

  /// <summary>
  /// Sums all possible game ids.
  /// </summary>
  /// <param name="games">The games to sum.</param>
  /// <returns>The sum of all possible game ids.</returns>
  public int SumPossibleGameIds(IEnumerable<Game> games) => games
    .Where(IsGamePossible)
    .Sum(g => g.Id);

  /// <summary>
  /// Sums the minimum powers of all games.
  /// </summary>
  /// <param name="games">The games to sum.</param>
  /// <returns>The sum of the minimum powers of all games.</returns>
  public int SumGameMinimumPowers(IEnumerable<Game> games) => games
    .Select(g => g.PowerOfMinimumCubesNeeded)
    .Sum();
}

/// <summary>
/// Puzzle parser responsible for parsing puzzle input.
/// </summary>
public class PuzzleParser
{
  /// <summary>
  /// Parses a cube from a string.
  /// </summary>
  /// <param name="cubeString">The string to parse.</param>
  /// <returns>An instance of <see cref="CubeCollection"/>.</returns>
  public CubeCollection ParseCube(string cubeString)
  {
    var parts = cubeString.Split(' ');
    var count = int.Parse(parts[0]);
    var color = Enum.Parse<CubeColor>(parts[1], true);

    return new()
    {
      Count = count,
      Color = color
    };
  }

  /// <summary>
  /// Parses a result from a string.
  /// </summary>
  /// <param name="resultString">The string to parse.</param>
  /// <returns>An instance of <see cref="Result"/>.</returns>
  public Result ParseResult(string resultString)
  {
    var cubes = resultString
      .Split(',')
      .Select(s => s.Trim())
      .Select(ParseCube)
      .ToList();

    return new()
    {
      Cubes = cubes
    };
  }

  /// <summary>
  /// Parses a game from a string.
  /// </summary>
  /// <param name="gameString">The string to parse.</param>
  /// <returns>An instance of <see cref="Game"/>.</returns>
  public Game ParseGame(string gameString)
  {
    var parts = gameString.Split(':');
    var gameId = int.Parse(parts[0].Split(' ')[1]);
    var results = parts[1]
      .Split(';')
      .Select(s => s.Trim())
      .Select(ParseResult)
      .ToList();

    return new()
    {
      Id = gameId,
      Results = results
    };
  }
}

/// <summary>
/// Represents a game.
/// </summary>
public class Game
{
  /// <summary>
  /// The id of the game.
  /// </summary>
  public int Id { get; set; }

  /// <summary>
  /// The results of the game.
  /// </summary>
  public List<Result> Results { get; set; } = [];

  private int MinimumRedCubesNeeded => Results.Max(r => r.RedCubeCount);
  private int MinimumGreenCubesNeeded => Results.Max(r => r.GreenCubeCount);
  private int MinimumBlueCubesNeeded => Results.Max(r => r.BlueCubeCount);

  /// <summary>
  /// The power of the minimum number of each cube type needed to play the game.
  /// </summary>
  public int PowerOfMinimumCubesNeeded => MinimumRedCubesNeeded * MinimumGreenCubesNeeded * MinimumBlueCubesNeeded;
}

/// <summary>
/// Represents a result.
/// </summary>
public class Result
{
  /// <summary>
  /// The cubes in the result.
  /// </summary>
  public List<CubeCollection> Cubes { get; set; } = [];

  /// <summary>
  /// The total number of cubes in the result.
  /// </summary>
  public int TotalCubeCount => Cubes.Sum(c => c.Count);

  /// <summary>
  /// The number of red cubes in the result.
  /// </summary>
  public int RedCubeCount => Cubes.Where(c => c.Color == CubeColor.Red).Sum(c => c.Count);

  /// <summary>
  /// The number of green cubes in the result.
  /// </summary>
  public int GreenCubeCount => Cubes.Where(c => c.Color == CubeColor.Green).Sum(c => c.Count);

  /// <summary>
  /// The number of blue cubes in the result.
  /// </summary>
  public int BlueCubeCount => Cubes.Where(c => c.Color == CubeColor.Blue).Sum(c => c.Count);
}

/// <summary>
/// Represents cubes of a given color.
/// </summary>
public class CubeCollection
{
  /// <summary>
  /// The number of cubes.
  /// </summary>
  public int Count { get; set; }

  /// <summary>
  /// The color of the cubes.
  /// </summary>
  public CubeColor Color { get; set; }
}

/// <summary>
/// Represents a cube color.
/// </summary>
public enum CubeColor
{
  /// <summary>
  /// The red cube color.
  /// </summary>
  Red,

  /// <summary>
  /// The green cube color.
  /// </summary>
  Green,

  /// <summary>
  /// The blue cube color.
  /// </summary>
  Blue
}