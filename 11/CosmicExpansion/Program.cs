using System.Diagnostics;

namespace CosmicExpansion;

public class Program
{
  public static async Task<int> Main(string[] args)
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

    var isPart2 = args.Length > 1 && args[1] == "part2";
    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var universe = isPart2
      ? Universe.Parse(input, 1_000_000)
      : Universe.Parse(input);

    var result = universe.SumOfShortestPathsBetweenGalaxies;

    stopwatch.Stop();

    Console.WriteLine($"The sum of the shortest paths between galaxies is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// Represents the universe.
/// </summary>
/// <param name="Height">The height of the universe.</param>
/// <param name="Width">The width of the universe.</param>
/// <param name="Galaxies">The galaxies in the universe.</param>
/// <returns>An instance of <see cref="Universe"/>.</returns>
public class Universe(
  int height,
  int width,
  List<Galaxy> galaxies
)
{
  private const char GalaxySymbol = '#';

  /// <summary>
  /// Gets the height of the universe.
  /// </summary>
  public int Height { get; init; } = height;

  /// <summary>
  /// Gets the width of the universe.
  /// </summary>
  public int Width { get; init; } = width;

  /// <summary>
  /// Gets the galaxies in the universe.
  /// </summary>
  public List<Galaxy> Galaxies { get; init; } = galaxies;

  /// <summary>
  /// Gets the sum of the shortest paths between galaxies.
  /// </summary>
  public long SumOfShortestPathsBetweenGalaxies =>
    GetShortestPathsBetweenGalaxies()
    .Values
    .Sum();

  /// <summary>
  /// Gets the shortest paths between all pairs of galaxies.
  /// </summary>
  /// <returns>A dictionary containing the shortest paths between galaxies.</returns>
  public Dictionary<(int galaxyOneIndex, int galaxyTwoIndex), long> GetShortestPathsBetweenGalaxies()
  {
    var shortestPaths = new Dictionary<(int galaxyOneIndex, int galaxyTwoIndex), long>();

    for (int i = 0; i < Galaxies.Count; i++)
    {
      var galaxyOne = Galaxies[i];

      for (int j = i + 1; j < Galaxies.Count; j++)
      {
        var galaxyTwo = Galaxies[j];

        if (shortestPaths.ContainsKey((i, j)) || shortestPaths.ContainsKey((j, i)))
        {
          continue;
        }

        var shortestPath = CalculateDistanceBetweenGalaxies(galaxyOne, galaxyTwo);
        shortestPaths.Add((i, j), shortestPath);
      }
    }

    return shortestPaths;
  }

  /// <summary>
  /// Calculates the distance between two galaxies.
  /// </summary>
  /// <param name="galaxyOne">The first galaxy.</param>
  /// <param name="galaxyTwo">The second galaxy.</param>
  /// <returns>The distance between the two galaxies.</returns>
  public static long CalculateDistanceBetweenGalaxies(Galaxy galaxyOne, Galaxy galaxyTwo) =>
    Math.Abs(galaxyTwo.Row - galaxyOne.Row) + Math.Abs(galaxyTwo.Column - galaxyOne.Column);

  /// <summary>
  /// Parses the input into an instance of <see cref="Universe"/>.
  /// </summary>
  /// <param name="input">The input.</param>
  /// <param name="expandFactor">The factor by which to expand the universe.</param>
  /// <returns>An instance of <see cref="Universe"/>.</returns>
  public static Universe Parse(string[] input, int expandFactor = 2)
  {
    var emptyRows = new List<int>();
    var emptyColumns = new List<int>();
    var galaxies = new List<Galaxy>();

    for (int i = 0; i < input.Length; i++)
    {
      var currentRow = input[i];

      if (currentRow.Contains(GalaxySymbol) is false)
      {
        emptyRows.Add(i);
      }
    }

    for (int i = 0; i < input[0].Length; i++)
    {
      var galaxyInColumn = false;

      for (int j = 0; j < input.Length; j++)
      {
        var currentPosition = input[j][i];

        if (currentPosition == GalaxySymbol)
        {
          galaxyInColumn = true;
          break;
        }
      }

      if (galaxyInColumn is false)
      {
        emptyColumns.Add(i);
      }
    }

    for (int i = 0; i < input.Length; i++)
    {
      var currentRow = input[i];

      for (int j = 0; j < currentRow.Length; j++)
      {
        var currentPosition = currentRow[j];

        if (currentPosition == GalaxySymbol)
        {
          var numOfEmptyRowsBefore = emptyRows.Where(row => row < i).Count();
          var rowOffset = numOfEmptyRowsBefore is 0
            ? 0
            : (numOfEmptyRowsBefore * expandFactor) - numOfEmptyRowsBefore;

          var numOfEmptyColumnsBefore = emptyColumns.Where(column => column < j).Count();
          var columnOffset = numOfEmptyColumnsBefore is 0
            ? 0
            : (numOfEmptyColumnsBefore * expandFactor) - numOfEmptyColumnsBefore;

          galaxies.Add(new Galaxy(i + rowOffset, j + columnOffset));
        }
      }
    }

    var heightOffset = emptyRows.Count is 0
      ? 0
      : (emptyRows.Count * expandFactor) - emptyRows.Count;
    var widthOffset = emptyColumns.Count is 0
      ? 0
      : (emptyColumns.Count * expandFactor) - emptyColumns.Count;

    var height = input.Length + heightOffset;
    var width = input[0].Length + widthOffset;

    return new Universe(
      height,
      width,
      galaxies
    );
  }
}

/// <summary>
/// Represents a galaxy.
/// </summary>
/// <param name="Row">The row of the galaxy.</param>
/// <param name="Column">The column of the galaxy.</param>
/// <returns>An instance of <see cref="Galaxy"/>.</returns>
public class Galaxy(
  long row,
  long column
)
{
  /// <summary>
  /// Gets the row of the galaxy.
  /// </summary>
  public long Row { get; init; } = row;

  /// <summary>
  /// Gets the column of the galaxy.
  /// </summary>
  public long Column { get; init; } = column;
}