namespace HauntedWasteland.Tests;

public class MapTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseMapTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenStringArray_ItShouldReturnMap(string[] input, Map expected)
  {
    Map.Parse(input).Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CountTurnsTestData), MemberType = typeof(TestData))]
  public void CountStepsToZ_WhenGivenMap_ItShouldReturnNumberOfSteps(string[] input, int expected)
  {
    Map.Parse(input).CountStepsToZ().Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly string[] MapInput =
    [
      "RL",
      "",
      "AAA = (BBB, CCC)",
      "BBB = (DDD, EEE)",
      "CCC = (ZZZ, GGG)",
      "DDD = (DDD, DDD)",
      "EEE = (EEE, EEE)",
      "GGG = (GGG, GGG)",
      "ZZZ = (ZZZ, ZZZ)",
    ];

    public static IEnumerable<object[]> CountTurnsTestData =>
      new List<object[]>
      {
        new object[]
        {
          MapInput,
          2
        },
        new object[]
        {
          File.ReadAllLines("INPUT.txt"),
          13019
        }
      };

    public static IEnumerable<object[]> ParseMapTestData =>
      new List<object[]>
      {
        new object[]
        {
          MapInput,
          new Map(
            ['R', 'L'],
            [
              new("AAA", "BBB", "CCC"),
              new("BBB", "DDD", "EEE"),
              new("CCC", "ZZZ", "GGG"),
              new("DDD", "DDD", "DDD"),
              new("EEE", "EEE", "EEE"),
              new("GGG", "GGG", "GGG"),
              new("ZZZ", "ZZZ", "ZZZ"),
            ]
          )
        },
      };
  }
}