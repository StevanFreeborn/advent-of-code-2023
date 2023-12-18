using System.Diagnostics;

namespace HotSprings;

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

    var result = input
      .Select(r => Condition.Parse(r, isPart2))
      .Sum(c => c.GetNumberOfPossibleConfigurations());

    stopwatch.Stop();

    Console.WriteLine($"The number of possible configurations is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// Represents a condition for a set of springs.
/// </summary>
/// <param name="DamagedContiguousGroupSizes">The sizes of the contiguous groups of damaged springs.</param>
/// <param name="TotalSpringsRequired">The total number of springs required to be available for each contiguous group of damaged springs.</param>
/// <param name="Springs">The conditions of the springs.</param>
public class Condition(
  List<int> damagedContiguousGroupSizes,
  List<int> totalSpringsRequired,
  List<SpringCondition> springs
)
{
  private const char OperationalSymbol = '.';
  private const char DamagedSymbol = '#';
  private const char UnknownSymbol = '?';

  public List<int> DamagedContiguousGroupSizes { get; set; } = damagedContiguousGroupSizes;
  public List<int> TotalSpringsRequired { get; set; } = totalSpringsRequired;
  public List<SpringCondition> Springs { get; set; } = springs;
  private readonly Dictionary<(int, int), long> _resultCache = [];
  private bool IsNotOperational(int index) => Springs[index] is not SpringCondition.Operational;
  private bool IsNotOperational(int index, int length) => Enumerable.Range(index, length).All(IsNotOperational);

  private bool IsDamaged(int index) =>
    index >= 0 && index < Springs.Count && Springs[index] is SpringCondition.Damaged;

  private bool RemainingSpringsAreNotDamaged(int index) =>
    Springs.Skip(index).All(sc => sc is not SpringCondition.Damaged);

  private long CalculateConfigurations(int springIndex, int groupIndex)
  {
    // check if all damaged groups have been checked
    if (groupIndex == DamagedContiguousGroupSizes.Count)
    {
      // if none of remaining springs are damaged, then we have a valid configuration
      return RemainingSpringsAreNotDamaged(springIndex) ? 1 : 0;
    }

    // if the number of springs required for current group size is
    // greater than the number of remaining springs, then we won't
    // be able to achieve a valid configuration
    if (springIndex + TotalSpringsRequired[groupIndex] > Springs.Count)
    {
      return 0;
    }

    long sum = 0;
    var groupSize = DamagedContiguousGroupSizes[groupIndex];
    var nextIndex = springIndex + groupSize + 1;

    // if the number of springs next to current spring
    // equal to the group size are not operational
    // and the spring next to the last spring in the group
    // is not damaged, then we have a configuration that
    // satisfies the current group size and can move
    // on to the next group
    if (IsNotOperational(springIndex, groupSize) && IsDamaged(springIndex + groupSize) is false)
    {
      sum += CountConfigurations(nextIndex, groupIndex + 1);
    }

    // if the current spring is not damaged
    // then we can move on to checking configuration
    // with next spring but same group size.
    if (IsDamaged(springIndex) is false)
    {
      sum += CountConfigurations(springIndex + 1, groupIndex);
    }

    return sum;
  }

  private long CountConfigurations(int springIndex, int groupIndex)
  {
    // use a tuple consisting of springIndex and groupIndex
    // as a key for the cache
    var key = (springIndex, groupIndex);

    // if the result for the current key is not in the cache
    // then calculate it and add it to the cache
    if (_resultCache.TryGetValue(key, out var result) is false)
    {
      result = CalculateConfigurations(springIndex, groupIndex);
      _resultCache[key] = result;
    }

    return result;
  }

  /// <summary>
  /// Returns the number of possible configurations for the condition.
  /// </summary>
  /// <returns>The number of possible configurations.</returns>
  public long GetNumberOfPossibleConfigurations() => CountConfigurations(0, 0);

  /// <summary>
  /// Parses the given input into a <see cref="Condition"/> instance.
  /// </summary>
  /// <param name="input">The input to parse.</param>
  /// <param name="unfold">Whether to unfold the condition.</param>
  /// <returns>The parsed <see cref="Condition"/> instance.</returns>
  public static Condition Parse(string input, bool unfold = false)
  {
    var springs = new List<SpringCondition>();

    var parts = input.Split(
      ' ',
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
    );
    var springsInput = parts[0];
    var contiguousGroupSizesInput = parts[1];

    foreach (var spring in springsInput)
    {
      switch (spring)
      {
        case OperationalSymbol:
          springs.Add(SpringCondition.Operational);
          break;
        case DamagedSymbol:
          springs.Add(SpringCondition.Damaged);
          break;
        case UnknownSymbol:
          springs.Add(SpringCondition.Unknown);
          break;
        default:
          throw new Exception($"Invalid spring condition: {spring}");
      }
    }

    var contiguousGroupSizes = contiguousGroupSizesInput
      .Split(
        ',',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
      )
      .Select(int.Parse)
      .ToList();

    if (unfold)
    {
      springs = [
        ..springs,
        SpringCondition.Unknown,
        ..springs,
        SpringCondition.Unknown,
        ..springs,
        SpringCondition.Unknown,
        ..springs,
        SpringCondition.Unknown,
        ..springs,
      ];

      contiguousGroupSizes = [
        ..contiguousGroupSizes,
        ..contiguousGroupSizes,
        ..contiguousGroupSizes,
        ..contiguousGroupSizes,
        ..contiguousGroupSizes,
      ];
    }

    var springsRequired = 0;
    var totalSpringsRequired = new int[contiguousGroupSizes.Count];

    for (var i = contiguousGroupSizes.Count - 1; i >= 0; i--)
    {
      springsRequired += contiguousGroupSizes[i];
      totalSpringsRequired[i] = springsRequired;
      springsRequired += 1;
    }

    return new Condition(
      contiguousGroupSizes,
      [.. totalSpringsRequired],
      springs
    );
  }
}

/// <summary>
/// Represents the condition of a spring.
/// </summary>
public enum SpringCondition
{
  /// <summary>
  /// The spring is operational.
  /// </summary>
  Operational,

  /// <summary>
  /// The spring is damaged.
  /// </summary>
  Damaged,

  /// <summary>
  /// The condition of the spring is unknown.
  /// </summary>
  Unknown,
}