using System.Diagnostics;

namespace PointOfIncidence;

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

    var input = await File.ReadAllTextAsync(args[0]);

    var patterns = input
      .ReplaceLineEndings()
      .Split(Environment.NewLine + Environment.NewLine)
      .Select(p => p.Split(Environment.NewLine));

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = patterns
      .Select(Pattern.Parse)
      .Sum(p => p.SummarizePatternNotes());

    stopwatch.Stop();

    Console.WriteLine($"The total of all pattern note summaries is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

public class Pattern(
  List<List<char>> rows,
  List<List<char>> columns
)
{
  public List<List<char>> Rows { get; init; } = rows;
  public List<List<char>> Columns { get; init; } = columns;

  public PointOfReflection FindPointOfReflection()
  {
    var pointOfReflection = new PointOfReflection();
    var startRowIndex = 0;
    var endRowIndex = 1;

    while (endRowIndex < Rows.Count)
    {
      var startRow = Rows[startRowIndex];
      var endRow = Rows[endRowIndex];

      if (startRow.SequenceEqual(endRow))
      {
        var isHorizontalReflection = true;
        var possibleReflectionPoint = new PointOfReflection
        {
          StartIndex = startRowIndex,
          EndIndex = endRowIndex,
          Type = ReflectionType.Horizontal,
        };

        while (startRowIndex > 0 && endRowIndex < Rows.Count - 1)
        {
          var previousRow = Rows[startRowIndex - 1];
          var nextRow = Rows[endRowIndex + 1];

          if (previousRow.SequenceEqual(nextRow) is false)
          {
            isHorizontalReflection = false;
            startRowIndex = possibleReflectionPoint.StartIndex;
            endRowIndex = possibleReflectionPoint.EndIndex;
            break;
          }

          startRowIndex--;
          endRowIndex++;
        }

        if (isHorizontalReflection)
        {
          pointOfReflection = possibleReflectionPoint;
          break;
        }
      }

      startRowIndex++;
      endRowIndex++;
    }

    if (pointOfReflection.Type == ReflectionType.None)
    {
      var startColumnIndex = 0;
      var endColumnIndex = 1;

      while (endColumnIndex < Columns.Count)
      {
        var startColumn = Columns[startColumnIndex];
        var endColumn = Columns[endColumnIndex];

        if (startColumn.SequenceEqual(endColumn))
        {
          var isVerticalReflection = true;
          var possibleReflectionPoint = new PointOfReflection
          {
            StartIndex = startColumnIndex,
            EndIndex = endColumnIndex,
            Type = ReflectionType.Vertical,
          };

          while (startColumnIndex > 0 && endColumnIndex < Columns.Count - 1)
          {
            var previousColumn = Columns[startColumnIndex - 1];
            var nextColumn = Columns[endColumnIndex + 1];

            if (previousColumn.SequenceEqual(nextColumn) is false)
            {
              isVerticalReflection = false;
              startColumnIndex = possibleReflectionPoint.StartIndex;
              endColumnIndex = possibleReflectionPoint.EndIndex;
              break;
            }

            startColumnIndex--;
            endColumnIndex++;
          }

          if (isVerticalReflection)
          {
            pointOfReflection = possibleReflectionPoint;
            break;
          }
        }

        startColumnIndex++;
        endColumnIndex++;
      }
    }

    return pointOfReflection;
  }

  public long SummarizePatternNotes()
  {
    var pointOfReflection = FindPointOfReflection();

    if (pointOfReflection.Type is ReflectionType.None)
    {
      return 0;
    }

    if (pointOfReflection.Type is ReflectionType.Vertical)
    {
      return pointOfReflection.StartIndex + 1;
    }

    return (pointOfReflection.StartIndex + 1) * 100;
  }

  public static Pattern Parse(string[] input)
  {
    var columns = new List<List<char>>();

    var height = input.Length;
    var width = input[0].Length;

    for (var i = 0; i < width; i++)
    {
      var column = new List<char>();

      for (var j = 0; j < height; j++)
      {
        column.Add(input[j][i]);
      }

      columns.Add(column);
    }

    var rows = input
      .Select(
        row => row
          .ToCharArray()
          .ToList()
      )
      .ToList();

    return new Pattern(rows, columns);
  }
}

public class PointOfReflection
{
  public ReflectionType Type = ReflectionType.None;
  public int StartIndex { get; set; } = -1;
  public int EndIndex { get; set; } = -1;
}

public enum ReflectionType
{
  Horizontal,
  Vertical,
  None,
}