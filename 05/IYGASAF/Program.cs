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

public class Almanac(
  List<long> seeds,
  List<SeedRange> seedRanges,
  List<Map> maps
)
{
  public List<SeedRange> SeedRanges { get; init; } = seedRanges;
  public List<long> Seeds { get; init; } = seeds;
  public List<Map> Maps { get; init; } = maps;

  private static List<long> GetSeedsFromString(string seedsString) =>
  seedsString
    .Split(
      ' ',
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
    )
    .Select(long.Parse)
    .ToList();

  public long GetSeedLocation(long seed) => Maps.Aggregate(
    seed,
    (currentSeed, map) => map.ConvertSourceToDestination(currentSeed)
  );

  public class LocationResult
  {
    public long Value { get; set; } = long.MaxValue;
  }

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

    File.WriteAllText("output.txt", lowestLocation.Value.ToString());

    return lowestLocation.Value;
  }

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

public class Map(
  List<Range> ranges
)
{
  public List<Range> Ranges { get; init; } = [.. ranges.OrderBy(range => range.SourceStart)];

  public static Map Parse(string[] ranges) =>
    new(
      ranges.Select(Range.Parse).ToList()
    );

  public Range? GetSourceRange(long sourceValue) =>
    Ranges
      .Where(range => range.SourceStart <= sourceValue && range.SourceEnd >= sourceValue)
      .FirstOrDefault();

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

public class SeedRange(
  long start,
  long length
)
{
  public long Start { get; init; } = start;
  public long Length { get; init; } = length;
  public long End => Start + Length - 1;
}

public class Range(
  long rangeLength,
  long sourceStart,
  long destinationStart
)
{
  public long RangeLength { get; init; } = rangeLength;
  public long SourceStart { get; init; } = sourceStart;
  public long SourceEnd => SourceStart + RangeLength - 1;
  public long DestinationStart { get; init; } = destinationStart;
  public long DestinationEnd => DestinationStart + RangeLength - 1;

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