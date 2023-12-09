namespace MirageMaintenance;

class Program
{
  public static async Task<int> Main(string[] args)
  {
    return await Task.FromResult(0);
  }
}

public class OasisReport(
  List<ValueHistory> valueHistories
)
{
  public List<ValueHistory> ValueHistories { get; init; } = valueHistories;

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
      allHistoricalValues.Last().Sum() is not 0
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
    var values = historicalValues.Prepend(Values).ToList();
    return 0;
  }
}