namespace CubeConundrum;

public class Program
{
  public async static Task<int> Main(string[] args)
  {
    return await Task.FromResult(0);
  }
}

public class PartOnePuzzleSolver
{
  private readonly int _maxCubeCount = 39;
  private readonly int _maxRedCubeCount = 12;
  private readonly int _maxGreenCubeCount = 13;
  private readonly int _maxBlueCubeCount = 14;

  public bool IsResultPossible(Result result) => result.TotalCubeCount <= _maxCubeCount
    && result.RedCubeCount <= _maxRedCubeCount
    && result.GreenCubeCount <= _maxGreenCubeCount
    && result.BlueCubeCount <= _maxBlueCubeCount;

  public bool IsGamePossible(Game game) => game.Results.All(IsResultPossible);

  public int SumPossibleGameIds(IEnumerable<Game> games) => games
    .Where(IsGamePossible)
    .Sum(g => g.Id);
}

public class PuzzleParser
{
  public Cube ParseCube(string cubeString)
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

public class Game
{
  public int Id { get; set; }
  public List<Result> Results { get; set; } = [];
}

public class Result
{
  public List<Cube> Cubes { get; set; } = [];
  public int TotalCubeCount => Cubes.Sum(c => c.Count);
  public int RedCubeCount => Cubes.Where(c => c.Color == CubeColor.Red).Sum(c => c.Count);
  public int GreenCubeCount => Cubes.Where(c => c.Color == CubeColor.Green).Sum(c => c.Count);
  public int BlueCubeCount => Cubes.Where(c => c.Color == CubeColor.Blue).Sum(c => c.Count);
}

public class Cube
{
  public int Count { get; set; }
  public CubeColor Color { get; set; }
}

public enum CubeColor
{
  Red,
  Green,
  Blue
}