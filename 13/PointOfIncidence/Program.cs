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

    var isPart2 = args.Length > 1 && args[1] == "part2";
    var input = await File.ReadAllTextAsync(args[0]);

    var patterns = input
      .ReplaceLineEndings()
      .Split(Environment.NewLine + Environment.NewLine)
      .Select(p => p.Split(Environment.NewLine));

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = patterns
      .Select(Pattern.Parse)
      .Sum(p => p.SummarizePatternNotes(isPart2));

    stopwatch.Stop();

    Console.WriteLine($"The total of all pattern note summaries is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// Represents a pattern.
/// </summary>
/// <param name="rows">The rows of the pattern.</param>
/// <param name="columns">The columns of the pattern.</param>
/// <returns>A <see cref="Pattern"/> instance.</returns>
public class Pattern(
  List<List<char>> rows,
  List<List<char>> columns
)
{
  /// <summary>
  /// Gets the rows of the pattern.
  /// </summary>
  public List<List<char>> Rows { get; init; } = rows;

  /// <summary>
  /// Gets the columns of the pattern.
  /// </summary>
  public List<List<char>> Columns { get; init; } = columns;

  private bool HasSmudge(List<char> row, List<char> otherRow)
  {
    var numOfDiffs = 0;

    for (var i = 0; i < row.Count; i++)
    {
      if (row[i] != otherRow[i])
      {
        numOfDiffs++;
      }
    }

    return numOfDiffs is 1;
  }

  private bool CheckForReflection(
    int startIndex,
    int endIndex,
    ReflectionType reflectionType,
    out PointOfReflection pointOfReflection
  )
  {
    List<char> start;
    List<char> end;
    List<List<char>> collection;
    var isReflection = false;
    pointOfReflection = new PointOfReflection();

    // maintain the original indices for the point of reflection
    var originalStartIndex = startIndex;
    var originalEndIndex = endIndex;

    // set the start and end points and the collection to check
    // based on the reflection type
    if (reflectionType == ReflectionType.Horizontal)
    {
      start = Rows[startIndex];
      end = Rows[endIndex];
      collection = Rows;
    }
    else
    {
      start = Columns[startIndex];
      end = Columns[endIndex];
      collection = Columns;
    }

    // check if the start and end collections are equal
    if (start.SequenceEqual(end))
    {
      isReflection = true;

      // move the start and end indices outwards
      // until the start index reaches start of the collection
      // or the end index reaches the end of the collection
      while (startIndex > 0 && endIndex < collection.Count - 1)
      {
        var previous = collection[startIndex - 1];
        var next = collection[endIndex + 1];

        // if the previous and next collections
        // are not equal then the reflection is
        // not complete
        if (previous.SequenceEqual(next) is false)
        {
          isReflection = false;
          break;
        }

        startIndex--;
        endIndex++;
      }
    }

    // if the reflection is complete
    // set the point of reflection
    if (isReflection)
    {
      pointOfReflection = new PointOfReflection
      {
        StartIndex = originalStartIndex,
        EndIndex = originalEndIndex,
        Type = reflectionType,
      };
    }

    return isReflection;
  }

  private bool CheckForReflectionWithoutSmudges(
    PointOfReflection originalPointOfReflection,
    int startIndex,
    int endIndex,
    ReflectionType reflectionType,
    out PointOfReflection pointOfReflection
  )
  {
    List<char> start;
    List<char> end;
    List<List<char>> collection;
    var isReflection = false;
    var smudgeCleaned = false;
    pointOfReflection = new PointOfReflection();

    // maintain the original indices for the point of reflection
    var originalStartIndex = startIndex;
    var originalEndIndex = endIndex;

    // set the start and end points and the collection to check
    // based on the reflection type
    if (reflectionType == ReflectionType.Horizontal)
    {
      start = Rows[startIndex];
      end = Rows[endIndex];
      collection = Rows;
    }
    else
    {
      start = Columns[startIndex];
      end = Columns[endIndex];
      collection = Columns;
    }

    var isNotOriginalPointOfReflection =
    (
      startIndex != originalPointOfReflection.StartIndex &&
      endIndex != originalPointOfReflection.EndIndex
    ) || originalPointOfReflection.Type != reflectionType;

    do
    {
      // check if the start and end collections are equal
      // and that they are not the original point of reflection
      // or that the smudge has been cleaned
      if ((start.SequenceEqual(end) && isNotOriginalPointOfReflection) || smudgeCleaned)
      {
        isReflection = true;

        // move the start and end indices outwards
        // until the start index reaches start of the collection
        // or the end index reaches the end of the collection
        while (startIndex > 0 && endIndex < collection.Count - 1)
        {
          var previous = collection[startIndex - 1];
          var next = collection[endIndex + 1];

          // if the previous and next collections
          // are not equal then the reflection is
          // not complete
          if (previous.SequenceEqual(next) is false)
          {
            // if we can clean the smudge and 
            // keep reflection intact continue
            // checking for reflection at this point
            if (HasSmudge(previous, next) && smudgeCleaned is false)
            {
              smudgeCleaned = true;
              startIndex--;
              endIndex++;
              continue;
            }

            isReflection = false;
            break;
          }

          startIndex--;
          endIndex++;
        }

        smudgeCleaned = false;
      }
      else
      {
        // if we can clean the smudge and
        // then we need to check for reflection
        // at this point
        if (HasSmudge(start, end))
        {
          smudgeCleaned = true;
          continue;
        }
      }
    } while (smudgeCleaned);

    // if the reflection is complete
    // set the point of reflection
    if (isReflection)
    {
      pointOfReflection = new PointOfReflection
      {
        StartIndex = originalStartIndex,
        EndIndex = originalEndIndex,
        Type = reflectionType,
      };
    }

    return isReflection;
  }

  /// <summary>
  /// Finds the point of reflection.
  /// </summary>
  /// <param name="cleanSmudge">Indicates whether to clean smudges.</param>
  /// <returns>A <see cref="PointOfReflection"/> instance at which the reflection occurs.</returns>
  public PointOfReflection FindPointOfReflection(bool cleanSmudge = false)
  {
    var orgPointOfReflection = new PointOfReflection();

    var startRowIndex = 0;
    var endRowIndex = 1;

    // check for horizontal reflection
    while (endRowIndex < Rows.Count)
    {
      var isReflection = CheckForReflection(
        startRowIndex,
        endRowIndex,
        ReflectionType.Horizontal,
        out var reflectionPoint
      );

      if (isReflection)
      {
        orgPointOfReflection = reflectionPoint;
        break;
      }

      startRowIndex++;
      endRowIndex++;
    }

    // if no horizontal reflection was found
    if (orgPointOfReflection.Type == ReflectionType.None)
    {
      var startColumnIndex = 0;
      var endColumnIndex = 1;

      // check for vertical reflection
      while (endColumnIndex < Columns.Count)
      {
        var isReflection = CheckForReflection(
          startColumnIndex,
          endColumnIndex,
          ReflectionType.Vertical,
          out var reflectionPoint
        );

        if (isReflection)
        {
          orgPointOfReflection = reflectionPoint;
          break;
        }

        startColumnIndex++;
        endColumnIndex++;
      }
    }

    // if we are not factoring in smudges
    // return the original point of reflection
    if (cleanSmudge is false)
    {
      return orgPointOfReflection;
    }

    var newPointsOfReflection = new List<PointOfReflection>();
    startRowIndex = 0;
    endRowIndex = 1;

    // check for horizontal reflection
    // without smudges
    while (endRowIndex < Rows.Count)
    {
      var isReflection = CheckForReflectionWithoutSmudges(
        orgPointOfReflection,
        startRowIndex,
        endRowIndex,
        ReflectionType.Horizontal,
        out var reflectionPoint
      );

      if (isReflection)
      {
        newPointsOfReflection.Add(reflectionPoint);
        break;
      }

      startRowIndex++;
      endRowIndex++;
    }

    // if no horizontal reflection was found
    // without smudges or the original point of reflection
    // was found again
    if (newPointsOfReflection.Count is 0 || newPointsOfReflection[0].Equals(orgPointOfReflection))
    {
      var startColumnIndex = 0;
      var endColumnIndex = 1;

      // check for vertical reflection
      // without smudges
      while (endColumnIndex < Columns.Count)
      {
        var isReflection = CheckForReflectionWithoutSmudges(
          orgPointOfReflection,
          startColumnIndex,
          endColumnIndex,
          ReflectionType.Vertical,
          out var reflectionPoint
        );

        if (isReflection)
        {
          newPointsOfReflection.Add(reflectionPoint);
          break;
        }

        startColumnIndex++;
        endColumnIndex++;
      }
    }

    // return the first point of reflection
    // that is not the original point of reflection
    return newPointsOfReflection.First(
      p => p.Equals(orgPointOfReflection) is false
    );
  }

  /// <summary>
  /// Summarizes the pattern notes.
  /// </summary>
  /// <param name="cleanSmudge">Indicates whether to clean smudges.</param>
  /// <returns>A number that represents the summary of the pattern notes.</returns>
  public long SummarizePatternNotes(bool cleanSmudge = false)
  {
    var pointOfReflection = FindPointOfReflection(cleanSmudge);

    return pointOfReflection.Type switch
    {
      ReflectionType.Horizontal => (pointOfReflection.StartIndex + 1) * 100,
      ReflectionType.Vertical => pointOfReflection.StartIndex + 1,
      _ => 0,
    };
  }

  /// <summary>
  /// Parses the input into a <see cref="Pattern"/> instance.
  /// </summary>
  /// <param name="input">The input to parse.</param>
  /// <returns>A <see cref="Pattern"/> instance.</returns>
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

/// <summary>
/// Represents a point of reflection
/// </summary>
public class PointOfReflection : IEquatable<PointOfReflection>
{
  /// <summary>
  /// Gets or sets the type of the reflection.
  /// </summary>
  public ReflectionType Type { get; set; } = ReflectionType.None;

  /// <summary>
  /// Gets or sets the start index of the reflection.
  /// </summary>
  public int StartIndex { get; set; } = -1;

  /// <summary>
  /// Gets or sets the end index of the reflection.
  /// </summary>
  public int EndIndex { get; set; } = -1;

  /// <summary>
  /// Indicates whether the current instance is equal to another instance.
  /// </summary>
  /// <param name="other">An instance of <see cref="PointOfReflection"/> to compare with this instance.</param>
  /// <returns>true if the current instance is equal to the other parameter; otherwise, false.</returns>
  public bool Equals(PointOfReflection? other) =>
    other is not null &&
    Type == other.Type &&
    StartIndex == other.StartIndex &&
    EndIndex == other.EndIndex;

  /// <summary>
  /// Determines whether the specified object is equal to the current object.
  /// </summary>
  /// <param name="obj">The object to compare with the current object.</param>
  /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
  public override bool Equals(object? obj)
  {
    return Equals(obj as PointOfReflection);
  }

  /// <summary>
  /// Returns the hash code for this instance.
  /// </summary>
  /// <returns>An integer that is the hash code for this instance.</returns>
  public override int GetHashCode()
  {
    return HashCode.Combine(Type, StartIndex, EndIndex);
  }
}

/// <summary>
/// Represents the type of reflection
/// </summary>
public enum ReflectionType
{
  /// <summary>
  /// Reflection that occurs on the horizontal axis
  /// </summary>
  Horizontal,

  /// <summary>
  /// Reflection that occurs on the vertical axis
  /// </summary>
  Vertical,

  /// <summary>
  /// No reflection
  /// </summary>
  None,
}