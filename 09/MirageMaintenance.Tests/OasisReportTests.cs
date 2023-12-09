namespace MirageMaintenance.Tests;

public class UnitTest1
{
  [Theory]
  [MemberData(nameof(TestData.OasisReportParseTestData), MemberType = typeof(TestData))]
  public void Parse_GivenAReportInput_ItShouldReturnExpectedOasisReportInstance(string[] input, OasisReport expected)
  {
    OasisReport.Parse(input).Should().BeEquivalentTo(expected);
  }

  public static class TestData
  {
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