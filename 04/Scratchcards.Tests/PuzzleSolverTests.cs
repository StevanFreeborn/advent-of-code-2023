namespace Scratchcards.Tests;

public class PuzzleSolverTests
{
  private readonly PuzzleSolver _sut = new();

  [Theory]
  [MemberData(nameof(TestData.SumCardValuesData), MemberType = typeof(TestData))]
  public void SumCardValues_GivenCards_ItShouldReturnExpectedValue(string[] cards, int expected)
  {
    var result = _sut.SumCardValues(cards);
    result.Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.GetTotalCardCountData), MemberType = typeof(TestData))]
  public void GetTotalCardCount_GivenCards_ItShouldReturnExpectedValue(string[] cards, int expected)
  {
    var result = _sut.GetTotalCardCount(cards);
    result.Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly string[] ExampleInput =
    [
      "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
      "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
      "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
      "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
      "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
      "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
    ];

    private static string[] Input => File.ReadAllLines("INPUT.txt");

    public static IEnumerable<object[]> GetTotalCardCountData =>
      new List<object[]>
      {
        new object[]
        {
          ExampleInput,
          30,
        },
        new object[]
        {
          Input,
          5920640,
        },
      };

    public static IEnumerable<object[]> SumCardValuesData =>
      new List<object[]>
      {
        new object[]
        {
          ExampleInput,
          13,
        },
        new object[]
        {
          Input,
          23235,
        },
      };
  }
}