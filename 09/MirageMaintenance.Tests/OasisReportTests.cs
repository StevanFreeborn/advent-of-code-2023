namespace MirageMaintenance.Tests;

public class UnitTest1
{
  [Theory]
  [MemberData(nameof(TestData.OasisReportParseTestData), MemberType = typeof(TestData))]
  public void Parse_GivenAReportInput_ItShouldReturnExpectedOasisReportInstance(string[] input, OasisReport expected)
  {
    OasisReport
      .Parse(input)
      .Should()
      .BeEquivalentTo(expected);
  }

  [Fact]
  public void CalculateSumOfNextValues_GivenAReportInput_ItShouldReturnTheSumOfAllNextValues()
  {
    var input = new string[]
    {
      "0 3 6 9 12 15",
      "1 3 6 10 15 21",
      "10 13 16 21 30 45",
    };

    OasisReport
      .Parse(input)
      .CalculateSumOfNextValues()
      .Should()
      .Be(114);
  }

  [Fact]
  public void CalculateSumOfNextValues_WhenGivenInput_ItShouldReturnTheSumOfAllNextValues()
  {
    OasisReport
      .Parse(TestData.Input)
      .CalculateSumOfNextValues()
      .Should()
      .Be(2175229206);
  }

  [Fact]
  public void CalculateSumOfNextValues_WhenGivenInputAndBackwardsIsTrue_ItShouldReturnTheSumOfAllNextValues()
  {
    OasisReport
      .Parse(TestData.Input)
      .CalculateSumOfNextValues(true)
      .Should()
      .Be(942);
  }

  public static class TestData
  {
    public static readonly string[] Input = File.ReadAllLines("INPUT.txt");

    public static IEnumerable<object[]> OasisReportParseTestData =>
      new List<object[]>
      {
          new object[]
          {
            new string[]
            {
              "0 3 6 9 12 15",
              "1 3 6 10 15 21",
              "10 13 16 21 30 45",
            },
            new OasisReport(
              [
                new([0, 3, 6, 9, 12, 15]),
                new([1, 3, 6, 10, 15, 21]),
                new([10, 13, 16, 21, 30, 45]),
              ]
            )
          }
      };
  }
}