using System.Diagnostics;

namespace MirageMaintenance;

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

    var result = OasisReport.Parse(input).CalculateSumOfNextValues(isPart2);

    stopwatch.Stop();

    Console.WriteLine($"The sum of all next values is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// Represents a report from the Oasis.
/// </summary>
/// <param name="ValueHistories">The value histories.</param>
/// <returns>An instance of <see cref="OasisReport"/>.</returns>
public class OasisReport(
  List<ValueHistory> valueHistories
)
{
  /// <summary>
  /// Gets the value histories.
  /// </summary>
  public List<ValueHistory> ValueHistories { get; init; } = valueHistories;

  /// <summary>
  /// Calculates the sum of all next values.
  /// </summary>
  /// <param name="backwards">if set to true will extrapolate next values backwards</param>
  /// <returns>The sum of all next values.</returns>
  public long CalculateSumOfNextValues(bool backwards = false) => ValueHistories
    .Select(valueHistory => valueHistory.CalculateNextValue(backwards))
    .Sum();

  /// <summary>
  /// Parses the specified oasis report input.
  /// </summary>
  /// <param name="oasisReportInput">The oasis report input.</param>
  /// <returns>An instance of <see cref="OasisReport"/>.</returns>
  public static OasisReport Parse(string[] oasisReportInput)
  {
    var valueHistories = new List<ValueHistory>();

    foreach (var line in oasisReportInput)
    {
      var values = line
        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToList();

      var valueHistory = new ValueHistory(values);
      valueHistories.Add(valueHistory);
    }

    return new OasisReport(valueHistories);
  }
}

/// <summary>
/// Represents a value history.
/// </summary>
/// <param name="Values">The values.</param>
/// <returns>An instance of <see cref="ValueHistory"/>.</returns>
public class ValueHistory(
  List<long> values
)
{
  /// <summary>
  /// Gets the values.
  /// </summary>
  List<long> Values { get; init; } = values;

  /// <summary>
  /// Calculates the differences between each value.
  /// </summary>
  /// <param name="values">The values.</param>
  /// <returns>The differences between each value.</returns>
  public List<long> CalculateDifferences(List<long> values) => Enumerable
    .Range(0, values.Count - 1)
    .Select(i => values[i + 1] - values[i])
    .ToList();

  /// <summary>
  /// Calculates the history.
  /// </summary>
  /// <returns>The history.</returns>
  public List<List<long>> CalculateHistory()
  {
    var allHistoricalValues = new List<List<long>>();
    var currentValues = Values;

    while (
      allHistoricalValues.Count is 0 ||
      allHistoricalValues.Last().Any(value => value is not 0)
    )
    {
      var historicalValues = CalculateDifferences(currentValues);
      allHistoricalValues.Add(historicalValues);
      currentValues = historicalValues;
    }

    return allHistoricalValues;
  }

  /// <summary>
  /// Calculates the next value.
  /// </summary>
  /// <param name="backwards">if set to true will extrapolate next value backwards</param>
  /// <returns>The next value.</returns>
  public long CalculateNextValue(bool backwards = false)
  {
    var historicalValues = CalculateHistory();
    var values = historicalValues.Prepend(Values).Reverse().ToList();

    if (backwards)
    {
      values.ForEach(value => value.Reverse());
    }

    for (var i = 0; i < values.Count; i++)
    {
      var currentValues = values[i];

      if (i is 0)
      {
        currentValues.Add(0);
        continue;
      }

      var previousValues = values[i - 1];

      var extrapolatedValue = backwards
        ? currentValues.Last() - previousValues.Last()
        : currentValues.Last() + previousValues.Last();

      currentValues.Add(extrapolatedValue);
    }

    return values.Last().Last();
  }
}