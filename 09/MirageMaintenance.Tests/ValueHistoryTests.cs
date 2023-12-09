namespace MirageMaintenance.Tests;

public class ValueHistoryTests
{
  [Theory]
  [MemberData(nameof(TestData.CalculateDifferencesTestData), MemberType = typeof(TestData))]
  public void CalculateDifferences_GivenAListOfValues_ItShouldReturnAListOfDifferences(List<long> values, List<long> expected)
  {
    new ValueHistory([]).CalculateDifferences(values).Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateHistoryTest), MemberType = typeof(TestData))]
  public void CalculateHistory_GivenAListOfValues_ItShouldReturnAListOfHistoricalValues(List<long> values, List<List<long>> expected)
  {
    new ValueHistory(values).CalculateHistory().Should().BeEquivalentTo(expected);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> CalculateHistoryTest =>
      new List<object[]>
      {
          new object[]
          {
            new List<long> { 0, 3, 6, 9, 12, 15 },
            new List<List<long>>
            {
              new() { 3, 3, 3, 3, 3 },
              new() { 0, 0, 0, 0 },
            },
          },
          new object[]
          {
            new List<long> { 1, 3, 6, 10, 15, 21 },
            new List<List<long>>
            {
              new() { 2, 3, 4, 5, 6 },
              new() { 1, 1, 1, 1 },
              new() { 0, 0, 0 },
            },
          },
          new object[]
          {
            new List<long> { 10, 13, 16, 21, 30, 45 },
            new List<List<long>>
            {
              new() { 3, 3, 5, 9, 15 },
              new() { 0, 2, 4, 6 },
              new() { 2, 2, 2 },
              new() { 0, 0 },
            },
          },
      };

    public static IEnumerable<object[]> CalculateDifferencesTestData =>
      new List<object[]>
      {
          new object[]
          {
            new List<long> { 0, 3, 6, 9, 12, 15 },
            new List<long> { 3, 3, 3, 3, 3 },
          },
          new object[]
          {
            new List<long> { 1, 3, 6, 10, 15, 21 },
            new List<long> { 2, 3, 4, 5, 6 },
          },
          new object[]
          {
            new List<long> { 10, 13, 16, 21, 30, 45 },
            new List<long> { 3, 3, 5, 9, 15 },
          },
      };
  }
}