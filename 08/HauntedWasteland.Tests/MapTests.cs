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
  [MemberData(nameof(TestData.CountStepsTestData), MemberType = typeof(TestData))]
  public void CountStepsToZ_WhenGivenMap_ItShouldReturnNumberOfSteps(string[] input, int expected)
  {
    Map.Parse(input).CountStepsToZ().Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CountStepsToAllZNodesData), MemberType = typeof(TestData))]
  public void CountStepsToAllZNodes_WhenGivenMap_ItShouldReturnNumberOfSteps(string[] input, long expected)
  {
    Map.Parse(input).CountStepsToAllZNodes().Should().Be(expected);
  }

  [Fact]
  public void FindPrimeFactors_WhenGivenNumber_ItShouldReturnPrimeFactors()
  {
    new Map([], []).FindPrimeFactors(11911).Should().BeEquivalentTo([43, 277]);
  }

  [Fact]
  public void FindLeastCommonMultiple_WhenGivenNumbers_ItShouldReturnLeastCommonMultiple()
  {
    new Map([], []).FindLeastCommonMultiple([16343, 11911, 20221, 21883, 13019, 19667]).Should().Be(13524038372771);
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

    private static readonly string[] Input = File.ReadAllLines("INPUT.txt");

    public static IEnumerable<object[]> CountStepsTestData =>
      new List<object[]>
      {
        new object[]
        {
          MapInput,
          2
        },
        new object[]
        {
          Input,
          13019
        }
      };

    public static IEnumerable<object[]> CountStepsToAllZNodesData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "LR",
            "",
            "11A = (11B, XXX)",
            "11B = (XXX, 11Z)",
            "11Z = (11B, XXX)",
            "22A = (22B, XXX)",
            "22B = (22C, 22C)",
            "22C = (22Z, 22Z)",
            "22Z = (22B, 22B)",
            "XXX = (XXX, XXX)",
          },
          6
        },
        new object[]
        {
          Input,
          13524038372771
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