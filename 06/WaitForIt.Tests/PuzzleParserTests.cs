namespace WaitForIt.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _sut = new();

  [Theory]
  [MemberData(nameof(TestData.ValidInputData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenValidInput_ItShouldReturnAListOfRaces(string[] input, List<Race> expected)
  {
    var result = _sut.ParseRaces(input);
    result.Should().BeEquivalentTo(expected);
  }

  public static class TestData
  {
    private static readonly string[] ValidInput =
    [
      "Time: 7 15 30",
      "Distance: 9 40 200"
    ];

    public static IEnumerable<object[]> ValidInputData =>
      new List<object[]>
      {
        new object[]
        {
          ValidInput,
          new List<Race>
          {
            new(7, 9),
            new(15, 40),
            new(30, 200)
          }
        }
      };
  }
}