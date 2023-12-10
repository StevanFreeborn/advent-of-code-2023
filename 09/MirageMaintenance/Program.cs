using System.Diagnostics;

namespace MirageMaintenance;

class Program
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

    var result = OasisReport.Parse(input).CalculateSumOfNextValues();

    stopwatch.Stop();

    Console.WriteLine($"The sum of all next values is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

public class OasisReport(
  List<ValueHistory> valueHistories
)
{
  public List<ValueHistory> ValueHistories { get; init; } = valueHistories;

  public long CalculateSumOfNextValues() => ValueHistories
    .Select(valueHistory => valueHistory.CalculateNextValue())
    .Sum();

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

public class ValueHistory(
  List<long> values
)
{
  List<long> Values { get; init; } = values;

  public List<long> CalculateDifferences(List<long> values) => Enumerable
    .Range(0, values.Count - 1)
    .Select(i => values[i + 1] - values[i])
    .ToList();

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

  public long CalculateNextValue()
  {
    var historicalValues = CalculateHistory();
    var values = historicalValues.Prepend(Values).Reverse().ToList();

    for (var i = 0; i < values.Count; i++)
    {
      var currentValues = values[i];

      if (i is 0)
      {
        currentValues.Add(0);
        continue;
      }

      var previousValues = values[i - 1];

      currentValues.Add(currentValues.Last() + previousValues.Last());
    }

    return values.Last().Last();
  }
}