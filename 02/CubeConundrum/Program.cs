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