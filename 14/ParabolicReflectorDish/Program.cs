using System.Diagnostics;

namespace ParabolicReflectorDish;

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

    var result = Dish
      .Parse(input)
      .CalculateTotalLoad(isPart2);

    stopwatch.Stop();

    Console.WriteLine($"The total load is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// Represents a parabolic reflector dish.
/// </summary>
/// <param name="rows">The rows of the dish.</param>
/// <returns>An instance of the <see cref="Dish"/> class.</returns>
public class Dish(
  List<List<char>> rows
)
{
  private const char RoundRockSymbol = 'O';
  private const char SquareRockSymbol = '#';
  private const char EmptySpaceSymbol = '.';

  /// <summary>
  /// Gets the rows of the dish.
  /// </summary>
  public List<List<char>> Rows { get; init; } = rows;

  private List<List<char>> TiltDishToNorth(List<List<char>> rows)
  {
    // create a copy of the rows which we can modify
    var modifiedRows = rows
      .Select(r => r.ToList())
      .ToList();

    var numOfColumns = modifiedRows[0].Count;
    var numOfRows = modifiedRows.Count;

    // iterate over the columns from left to right
    for (var currentColumnIndex = 0; currentColumnIndex < numOfColumns; currentColumnIndex++)
    {
      // iterate over the rows from top to bottom
      for (var currentRowIndex = 0; currentRowIndex < numOfRows; currentRowIndex++)
      {
        // get the current symbol
        var current = modifiedRows[currentRowIndex][currentColumnIndex];

        // if the current symbol is an empty space
        if (current is EmptySpaceSymbol)
        {
          // iterate over the rows below the current row
          // starting from the row after the current row
          for (var i = currentRowIndex + 1; i < numOfRows; i++)
          {
            // get the next symbol in the current column
            var next = modifiedRows[i][currentColumnIndex];

            // if the next symbol is a square rock
            // we can stop iterating over the rows below
            // because the square rock blocks moving
            // any round rock to fill the empty space
            if (next is SquareRockSymbol)
            {
              break;
            }

            // if the next symbol is a round rock
            // we can move the round rock to the empty space
            // and stop iterating over the rows below
            if (next is RoundRockSymbol)
            {
              modifiedRows[currentRowIndex][currentColumnIndex] = RoundRockSymbol;
              modifiedRows[i][currentColumnIndex] = EmptySpaceSymbol;
              break;
            }
          }
        }
      }
    }

    return modifiedRows;
  }

  private List<List<char>> TiltDishToWest(List<List<char>> rows)
  {
    // create a copy of the rows which we can modify
    var modifiedRows = rows
      .Select(r => r.ToList())
      .ToList();

    var numOfRows = modifiedRows.Count;
    var numOfColumns = modifiedRows[0].Count;

    // iterate over the rows from top to bottom
    for (var currentRowIndex = 0; currentRowIndex < numOfRows; currentRowIndex++)
    {
      // iterate over the columns from left to right
      for (var currentColumnIndex = 0; currentColumnIndex < numOfRows; currentColumnIndex++)
      {
        // get the current symbol
        var current = modifiedRows[currentRowIndex][currentColumnIndex];

        // if the current symbol is an empty space
        if (current is EmptySpaceSymbol)
        {
          // iterate over the columns to the left of the current column
          for (var i = currentColumnIndex + 1; i < numOfColumns; i++)
          {
            // get the next symbol in the current row
            var next = modifiedRows[currentRowIndex][i];

            // if the next symbol is a square rock
            // we can stop iterating over the columns to the left
            // because the square rock blocks moving
            // any round rock to fill the empty space
            if (next is SquareRockSymbol)
            {
              break;
            }

            // if the next symbol is a round rock
            // we can move the round rock to the empty space
            // and stop iterating over the columns to the left
            if (next is RoundRockSymbol)
            {
              modifiedRows[currentRowIndex][currentColumnIndex] = RoundRockSymbol;
              modifiedRows[currentRowIndex][i] = EmptySpaceSymbol;
              break;
            }
          }
        }
      }
    }

    return modifiedRows;
  }

  private List<List<char>> TiltDishToSouth(List<List<char>> rows)
  {
    // create a copy of the rows which we can modify
    var modifiedRows = rows
      .Select(r => r.ToList())
      .ToList();

    var numOfColumns = modifiedRows[0].Count;
    var numOfRows = modifiedRows.Count;

    // iterate over the columns from left to right
    for (var currentColumnIndex = 0; currentColumnIndex < numOfColumns; currentColumnIndex++)
    {
      // iterate over the rows from bottom to top
      for (var currentRowIndex = numOfRows - 1; currentRowIndex >= 0; currentRowIndex--)
      {
        // get the current symbol
        var current = modifiedRows[currentRowIndex][currentColumnIndex];

        // if the current symbol is an empty space
        if (current is EmptySpaceSymbol)
        {
          // iterate over the rows above the current row
          // starting from the row before the current row
          for (var i = currentRowIndex - 1; i >= 0; i--)
          {
            // get the next symbol in the current column
            var next = modifiedRows[i][currentColumnIndex];

            // if the next symbol is a square rock
            // we can stop iterating over the rows below
            // because the square rock blocks moving
            // any round rock to fill the empty space
            if (next is SquareRockSymbol)
            {
              break;
            }

            // if the next symbol is a round rock
            // we can move the round rock to the empty space
            // and stop iterating over the rows below
            if (next is RoundRockSymbol)
            {
              modifiedRows[currentRowIndex][currentColumnIndex] = RoundRockSymbol;
              modifiedRows[i][currentColumnIndex] = EmptySpaceSymbol;
              break;
            }
          }
        }
      }
    }

    return modifiedRows;
  }

  private List<List<char>> TiltDishToEast(List<List<char>> rows)
  {
    // create a copy of the rows which we can modify
    var modifiedRows = rows
      .Select(r => r.ToList())
      .ToList();

    var numOfRows = modifiedRows.Count;
    var numOfColumns = modifiedRows[0].Count;

    // iterate over the rows from top to bottom
    for (var currentRowIndex = 0; currentRowIndex < numOfRows; currentRowIndex++)
    {
      // iterate over the columns from right to left
      for (var currentColumnIndex = numOfColumns - 1; currentColumnIndex >= 0; currentColumnIndex--)
      {
        // get the current symbol
        var current = modifiedRows[currentRowIndex][currentColumnIndex];

        // if the current symbol is an empty space
        if (current is EmptySpaceSymbol)
        {
          // iterate over the columns to the left of the current column
          for (var i = currentColumnIndex - 1; i >= 0; i--)
          {
            // get the next symbol in the current row
            var next = modifiedRows[currentRowIndex][i];

            // if the next symbol is a square rock
            // we can stop iterating over the columns to the left
            // because the square rock blocks moving
            // any round rock to fill the empty space
            if (next is SquareRockSymbol)
            {
              break;
            }

            // if the next symbol is a round rock
            // we can move the round rock to the empty space
            // and stop iterating over the columns to the left
            if (next is RoundRockSymbol)
            {
              modifiedRows[currentRowIndex][currentColumnIndex] = RoundRockSymbol;
              modifiedRows[currentRowIndex][i] = EmptySpaceSymbol;
              break;
            }
          }
        }
      }
    }

    return modifiedRows;
  }

  /// <summary>
  /// Tilt the dish to the north, west, south and east.
  /// </summary>
  /// <param name="numOfCycles">The number of cycles to tilt the dish.</param>
  /// <returns>An instance of the <see cref="Dish"/> class.</returns>
  public Dish TiltDish(long numOfCycles)
  {
    var modifiedRows = Rows
      .Select(r => r.ToList())
      .ToList();

    // create a cache to store the modified rows
    // and the index of the cycle in which they were modified
    // this should allow us to detect cycles
    var cache = new Dictionary<string, long>();

    for (var i = 0; i < numOfCycles; i++)
    {
      modifiedRows = TiltDishToNorth(modifiedRows);
      modifiedRows = TiltDishToWest(modifiedRows);
      modifiedRows = TiltDishToSouth(modifiedRows);
      modifiedRows = TiltDishToEast(modifiedRows);

      // use the modified rows as a key for the cache
      var key = string.Join(Environment.NewLine, modifiedRows.Select(r => string.Join(string.Empty, r)));

      // if the cache already contains the key
      if (cache.TryGetValue(key, out var matchingIndex))
      {
        // calculate the start index of the current cycle
        // and identify the key for the start index value
        var cycleLength = i - matchingIndex;
        var remainingCycles = numOfCycles - i;
        var cycleIndex = remainingCycles % cycleLength;
        var cycleStartIndex = cycleIndex + matchingIndex - 1;
        var startCycleKey = cache.First(c => c.Value == cycleStartIndex).Key;

        // parse the key for the start index value
        // and use it as the modified rows
        modifiedRows = startCycleKey
          .Split(Environment.NewLine)
          .Select(s => s.ToList())
          .ToList();
        break;
      }

      // add the modified rows to the cache
      cache.Add(key, i);
    }

    return new Dish(modifiedRows);
  }

  /// <summary>
  /// Calculate the total load of the dish.
  /// </summary>
  /// <param name="spinDish">Whether to spin the dish.</param>
  /// <returns>The total load of the dish.</returns>
  public long CalculateTotalLoad(bool spinDish = false)
  {
    var rows = spinDish
      ? TiltDish(1_000_000_000).Rows
      : TiltDishToNorth(Rows);

    rows.Reverse();

    return rows
      .Select((r, index) =>
      {
        var numOfRoundRocks = r.Where(c => c is RoundRockSymbol).Count();
        return (index + 1) * numOfRoundRocks;
      })
      .Sum();
  }

  /// <summary>
  /// Parse the input into an instance of the <see cref="Dish"/> class.
  /// </summary>
  /// <param name="input">The input to parse.</param>
  /// <returns>An instance of the <see cref="Dish"/> class.</returns>
  public static Dish Parse(string[] input)
  {
    var inputList = input
      .Select(s => s.ToList())
      .ToList();

    return new Dish(inputList);
  }
}