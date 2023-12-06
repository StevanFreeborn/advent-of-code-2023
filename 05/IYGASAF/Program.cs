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
    var result = Almanac.Parse(input).GetLowestSeedLocation();

    Console.WriteLine($"The lowest seed location is {result}.");

    return (int)result;
  }
}

public class Almanac(
  List<long> seeds,
  List<Map> maps
)
{
  public List<long> Seeds { get; init; } = seeds;
  public List<Map> Maps { get; init; } = maps;

  public long GetSeedLocation(long seed) => Maps.Aggregate(
    seed,
    (currentSeed, map) => map.ConvertSourceToDestination(currentSeed)
  );

  public long GetLowestSeedLocation() => Seeds
    .Select(GetSeedLocation)
    .Min();

  public static Almanac Parse(string[] almanac)
  {
    var seedsString = almanac[0].Split(':')[1];
    var seeds = seedsString
      .Split(
        ' ',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
      )
      .Select(long.Parse)
      .ToList();

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
      maps
    );
  }
}

public class Map(
  List<Range> ranges
)
{
  public List<Range> Ranges { get; init; } = ranges;

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