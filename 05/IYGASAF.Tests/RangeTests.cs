namespace IYGASAF.Tests;

public class RangeTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseRangeData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenValidRange_ItShouldReturnExpectedRange(string range, Range expected)
  {
    var result = Range.Parse(range);
    result.Should().BeEquivalentTo(expected);
  }


  public static class TestData
  {
    public static IEnumerable<object[]> ParseRangeData =>
      new List<object[]>
      {
          new object[] { "50 98 2", new Range(2, 98, 50) },
          new object[] { "52 50 48", new Range(48, 50, 52) },
      };
  }
}