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

    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = Dish
      .Parse(input)
      .CalculateTotalLoad();

    stopwatch.Stop();

    Console.WriteLine($"The total load is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

public class Dish(
  List<List<char>> rows
)
{
  private const char RoundRockSymbol = 'O';
  private const char SquareRockSymbol = '#';
  private const char EmptySpaceSymbol = '.';

  public List<List<char>> Rows { get; init; } = rows;

  private List<List<char>> TiltDishToNorth(List<List<char>> rows)
  {
    // create a copy of the rows which we can modify
    var modifiedRows = rows.ToList();
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

  public long CalculateTotalLoad()
  {
    var rows = TiltDishToNorth(Rows);
    rows.Reverse();

    return rows
      .Select((r, index) =>
      {
        var numOfRoundRocks = r.Where(c => c is RoundRockSymbol).Count();
        return (index + 1) * numOfRoundRocks;
      })
      .Sum();
  }

  public static Dish Parse(string[] input)
  {
    var inputList = input
      .Select(s => s.ToList())
      .ToList();

    return new Dish(inputList);
  }
}