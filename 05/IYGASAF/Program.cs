namespace IYGASAF;

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
    var seedsAreRanges = args.Length > 1 && args[1] == "part2";

    Console.WriteLine("Parsing almanac...");
    var almanac = Almanac.Parse(input);

    Console.WriteLine("Getting lowest seed location...");
    var result = almanac.GetLowestSeedLocation(seedsAreRanges);

    Console.WriteLine($"The lowest seed location is {result}.");

    return (int)result;
  }
}

/// <summary>
/// Represents an almanac.
/// </summary>
/// <param name="seeds">The seeds.</param>
/// <param name="seedRanges">The seed ranges.</param>
/// <param name="maps">The maps.</param>
/// <returns>An instance of <see cref="Almanac"/>.</returns>
public class Almanac(
  List<long> seeds,
  List<SeedRange> seedRanges,
  List<Map> maps
)
{
  /// <summary>
  /// Gets the seed ranges.
  /// </summary>
  public List<SeedRange> SeedRanges { get; init; } = seedRanges;

  /// <summary>
  /// Gets the seeds.
  /// </summary>
  public List<long> Seeds { get; init; } = seeds;

  /// <summary>
  /// Gets the maps.
  /// </summary>
  public List<Map> Maps { get; init; } = maps;

  private static List<long> GetSeedsFromString(string seedsString) =>
  seedsString
    .Split(
      ' ',
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
    )
    .Select(long.Parse)
    .ToList();

  /// <summary>
  /// Gets the seed location.
  /// </summary>
  /// <param name="seed">The seed.</param>
  /// <returns>The seed location.</returns>
  public long GetSeedLocation(long seed) => Maps.Aggregate(
    seed,
    (currentSeed, map) => map.ConvertSourceToDestination(currentSeed)
  );

  private class LocationResult
  {
    public long Value { get; set; } = long.MaxValue;
  }

  /// <summary>
  /// Gets the lowest seed location.
  /// </summary>
  /// <param name="seedsAreRanges">If set to true seeds are treated as ranges</param>
  /// <returns>The lowest seed location.</returns>
  public long GetLowestSeedLocation(bool seedsAreRanges = false)
  {
    var lowestLocation = new LocationResult();

    if (seedsAreRanges)
    {
      Parallel.ForEach(SeedRanges, (currentSeedRange) =>
      {
        var currentSeedRangeStart = currentSeedRange.Start;
        var currentSeedRangeEnd = currentSeedRange.End;

        for (long j = currentSeedRangeStart; j <= currentSeedRangeEnd; j++)
        {
          var currentSeed = j;
          var currentSeedLocation = GetSeedLocation(currentSeed);

          lock (lowestLocation)
          {
            if (currentSeedLocation < lowestLocation.Value)
            {
              lowestLocation.Value = currentSeedLocation;
            }
          }
        }
      });
    }
    else
    {
      for (int i = 0; i < Seeds.Count; i++)
      {
        var currentSeed = Seeds[i];
        var currentSeedLocation = GetSeedLocation(currentSeed);

        if (currentSeedLocation < lowestLocation.Value)
        {
          lowestLocation.Value = currentSeedLocation;
        }
      }
    }

    return lowestLocation.Value;
  }

  /// <summary>
  /// Parses the seeds as ranges.
  /// </summary>
  /// <param name="seedsString">The seeds string.</param>
  /// <returns>The seeds as ranges.</returns>
  public static List<SeedRange> ParseSeedsAsRanges(string seedsString)
  {
    var seeds = new List<SeedRange>();
    var seedNumbers = GetSeedsFromString(seedsString);

    for (int i = 0; i < seedNumbers.Count; i += 2)
    {
      var start = seedNumbers[i];
      var length = seedNumbers[i + 1];

      seeds.Add(new(start, length));
    }

    return seeds;
  }

  /// <summary>
  /// Parses the specified almanac.
  /// </summary>
  /// <param name="almanac">The almanac.</param>
  /// <returns>An instance of <see cref="Almanac"/>.</returns> 
  public static Almanac Parse(string[] almanac)
  {
    var seedsString = almanac[0].Split(':')[1];
    var seeds = GetSeedsFromString(seedsString);
    var seedRanges = ParseSeedsAsRanges(seedsString);

    var maps = new List<Map>();
    var ranges = new List<string>();

    for (int i = 1; i < almanac.Length; i++)
    {
      var currentLine = almanac[i];

      if (string.IsNullOrWhiteSpace(currentLine))
      {
        continue;
      }

      if (currentLine.Contains(':'))
      {
        if (ranges.Count is not 0)
        {
          maps.Add(Map.Parse([.. ranges]));
          ranges.Clear();
        }
        continue;
      }

      ranges.Add(currentLine);

      if (i == almanac.Length - 1)
      {
        maps.Add(Map.Parse([.. ranges]));
      }
    }

    return new Almanac(
      seeds,
      seedRanges,
      maps
    );
  }
}

/// <summary>
/// Represents a map.
/// </summary>
/// <param name="ranges">The ranges contained within the map</param>
/// <returns>An instance of <see cref="Map"/>.</returns>
public class Map(
  List<Range> ranges
)
{
  /// <summary>
  /// Gets the ranges.
  /// </summary>
  public List<Range> Ranges { get; init; } = [.. ranges.OrderBy(range => range.SourceStart)];

  /// <summary>
  /// Parses a map from the specified ranges.
  /// </summary>
  /// <param name="ranges">The ranges.</param>
  /// <returns>An instance of <see cref="Map"/>.</returns>
  public static Map Parse(string[] ranges) =>
    new(
      ranges.Select(Range.Parse).ToList()
    );

  /// <summary>
  /// Gets the source range.
  /// </summary>
  /// <param name="sourceValue">The source value.</param>
  /// <returns>The source <see cref="Range"/>.</returns>
  public Range? GetSourceRange(long sourceValue) =>
    Ranges
      .Where(range => range.SourceStart <= sourceValue && range.SourceEnd >= sourceValue)
      .FirstOrDefault();

  /// <summary>
  /// Converts the source to destination.
  /// </summary>
  /// <param name="valueToConvert">The value to convert.</param>
  /// <returns>The converted value.</returns>
  public long ConvertSourceToDestination(long valueToConvert)
  {
    var sourceRange = GetSourceRange(valueToConvert);

    if (sourceRange is null)
    {
      return valueToConvert;
    }

    var sourceToDestinationOffset = Math.Abs(sourceRange.SourceStart - sourceRange.DestinationStart);

    return sourceRange.DestinationStart <= sourceRange.SourceStart
      ? valueToConvert - sourceToDestinationOffset
      : valueToConvert + sourceToDestinationOffset;
  }
}

/// <summary>
/// Represents a seed range.
/// </summary>
/// <param name="start">The start.</param>
/// <param name="length">The length.</param>
/// <returns>An instance of <see cref="SeedRange"/>.</returns>
public class SeedRange(
  long start,
  long length
)
{
  /// <summary>
  /// Gets the start of the range. It is inclusive.
  /// </summary>
  public long Start { get; init; } = start;

  /// <summary>
  /// Gets the length of the range.
  /// </summary>
  public long Length { get; init; } = length;

  /// <summary>
  /// Gets the end of the range. It is inclusive.
  /// </summary>
  public long End => Start + Length - 1;
}

/// <summary>
/// Represents a map range.
/// </summary>
/// <param name="rangeLength">Length of the range.</param>
/// <param name="sourceStart">The source start.</param>
/// <param name="destinationStart">The destination start.</param>
/// <returns>An instance of <see cref="Range"/>.</returns>
public class Range(
  long rangeLength,
  long sourceStart,
  long destinationStart
)
{
  /// <summary>
  /// Gets the length of the range.
  /// </summary>
  public long RangeLength { get; init; } = rangeLength;

  /// <summary>
  /// Gets the start of the source range. It is inclusive.
  /// </summary>
  public long SourceStart { get; init; } = sourceStart;

  /// <summary>
  /// Gets the end of the source range. It is inclusive.
  /// </summary>
  public long SourceEnd => SourceStart + RangeLength - 1;

  /// <summary>
  /// Gets the start of the destination range. It is inclusive.
  /// </summary>
  public long DestinationStart { get; init; } = destinationStart;

  /// <summary>
  /// Gets the end of the destination range. It is inclusive.
  /// </summary>
  public long DestinationEnd => DestinationStart + RangeLength - 1;

  /// <summary>
  /// Parses the specified range.
  /// </summary>
  /// <param name="range">The range.</param>
  /// <returns>An instance of <see cref="Range"/>.</returns>
  /// <exception cref="ArgumentException">Range must be in the format of 'DestinationRangeStart:long SourceRangeStart:long RangeLength:long'</exception>
  public static Range Parse(string range)
  {
    var parts = range.Split(
      ' ',
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
    );

    if (parts.Length is not 3 || parts.Any(part => long.TryParse(part, out _) is false))
    {
      throw new ArgumentException("Range must be in the format of 'DestinationRangeStart:long SourceRangeStart:long RangeLength:long'");
    }

    var rangeLength = long.Parse(parts[2]);
    var sourceStart = long.Parse(parts[1]);
    var destinationStart = long.Parse(parts[0]);

    return new Range(
      rangeLength,
      sourceStart,
      destinationStart
    );
  }
}