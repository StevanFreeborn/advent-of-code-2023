using System.Diagnostics;

namespace WaitForIt;

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

    var parser = new PuzzleParser();
    var input = await File.ReadAllLinesAsync(args[0]);
    var isPart2 = args.Length > 1 && args[1] == "part2";

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    long result = isPart2
      ? parser
        .ParseRace(input)
        .CalculateNumberOfWaysToWin()
      : parser.ParseRaces(input)
        .Select(r => r.CalculateNumberOfWaysToWin())
        .Aggregate((long)1, (acc, curr) => acc * curr);

    stopWatch.Stop();

    if (isPart2)
    {
      Console.WriteLine($"The number of ways to win is {result}. ({stopWatch.ElapsedMilliseconds}ms)");
    }
    else
    {
      Console.WriteLine($"The total number of ways to win is {result}. ({stopWatch.ElapsedMilliseconds}ms)");
    }

    return (int)result;
  }
}

/// <summary>
/// Parses the puzzle input.
/// </summary>
public class PuzzleParser
{
  private List<long> GetValues(string input) => input
    .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
    .Skip(1)
    .Select(long.Parse)
    .ToList();

  /// <summary>
  /// Parses the puzzle input to list of races
  /// </summary>
  /// <param name="racesInput">The list of races</param>
  /// <returns>A list containing instances of <see cref="Race"/>.</returns>
  public List<Race> ParseRaces(string[] racesInput)
  {
    var races = new List<Race>();
    var durations = GetValues(racesInput[0]);
    var distances = GetValues(racesInput[1]);

    if (durations.Count != distances.Count)
    {
      throw new ArgumentException("The number of times and distances must be equal.");
    }

    for (int i = 0; i < durations.Count; i++)
    {
      races.Add(new Race(durations[i], distances[i]));
    }

    return races;
  }

  /// <summary>
  /// Parses the puzzle input as single race
  /// </summary>
  /// <param name="racesInput">The list of races</param>
  /// <returns>An instance of <see cref="Race"/>.</returns>
  public Race ParseRace(string[] racesInput)
  {
    var duration = string.Join("", GetValues(racesInput[0]).Select(v => v.ToString()));
    var distance = string.Join("", GetValues(racesInput[1]).Select(v => v.ToString()));

    return new Race(long.Parse(duration), long.Parse(distance));
  }
}

/// <summary>
/// Represents a Race
/// </summary>
/// <param name="duration">The duration of the race</param>
/// <param name="distanceRecord">The distance record</param>
/// <returns>An instance of <see cref="Race"/>.</returns> 
public class Race(
  long duration,
  long distanceRecord
)
{
  /// <summary>
  /// Gets the duration of the race
  /// </summary>
  public long Duration { get; init; } = duration;

  /// <summary>
  /// Gets the distance record
  /// </summary>
  public long DistanceRecord { get; init; } = distanceRecord;

  /// <summary>
  /// Calculates the number of ways the race can be won.
  /// </summary>
  /// <returns>The number of ways the race can be won.</returns>
  public long CalculateNumberOfWaysToWin()
  {
    var minDuration = 0.0;
    var maxDuration = Math.Floor(Duration / 2.0);

    while (minDuration < maxDuration - 1)
    {
      var middleDuration = Math.Floor((maxDuration + minDuration) / 2);
      var speed = 1 * middleDuration;
      var distance = speed * (Duration - middleDuration);

      if (distance >= DistanceRecord)
      {
        maxDuration = middleDuration;
      }
      else
      {
        minDuration = middleDuration;
      }
    }

    var result = Duration % 2 == 0
      ? Duration - ((long)maxDuration * 2) - 1
      : Duration - ((long)maxDuration * 2) + 1;

    return result;
  }
}