namespace GearRatios.Test;

public class LineTests
{
  [Theory, MemberData(nameof(TestData.LineData), MemberType = typeof(TestData))]
  public void Parse_GivenLine_ItShouldParseExpectedValues(string line, Line expected)
  {
    var actual = Line.Parse(line);
    actual.Should().BeEquivalentTo(expected);
  }

  [Theory, MemberData(nameof(TestData.PartNumberData), MemberType = typeof(TestData))]
  public void GetPartNumbers_GivenLine_ItShouldReturnExpectedValues(Line currentLine, Line? prevLine, Line? nextLine, List<int> expected)
  {
    var actual = currentLine.GetPartNumbers(prevLine, nextLine);
    actual.Should().BeEquivalentTo(expected);
  }

  [Theory, MemberData(nameof(TestData.GearData), MemberType = typeof(TestData))]
  public void GetGearRatios_GivenLine_ItShouldReturnExpectedValues(Line currentLine, Line? prevLine, Line? nextLine, List<int> expected)
  {
    var actual = currentLine.GetGearRatios(prevLine, nextLine);
    actual.Should().BeEquivalentTo(expected);
  }


  public static class TestData
  {
    public static IEnumerable<object?[]> GearData =>
      new List<object?[]>
      {
        new object?[]
        {
          new Line(
            [],
            []
          ),
          null,
          null,
          new List<int>(),
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 467, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(5, 7) },
              },
            ],
            [
            ]
          ),
          null,
          null,
          new List<int>(),
        },
        new object?[]
        {
          new Line(
            [],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          null,
          null,
          new List<int>(),
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 617, new Indexes(0, 2) },
                { 114, new Indexes(4, 6) },
              },
            ],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          null,
          null,
          new List<int> { 617 * 114 },
        },
        new object?[]
        {
          new Line(
            [],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          new Line(
            [
              new()
              {
                { 467, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(5, 7) },
              },
            ],
            []
          ),
          new Line(
            [
              new()
              {
                { 35, new Indexes(2, 3) },
              },
              new()
              {
                { 633, new Indexes(6, 8) },
              },
            ],
            []
          ),
          new List<int> { 467 * 35 },
        }
      };

    public static IEnumerable<object?[]> PartNumberData =>
      new List<object?[]>
      {
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 467, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(5, 7) },
              },
            ],
            [
            ]
          ),
          null,
          null,
          new List<int>(),
        },
        new object?[]
        {
          new Line(
            [],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          null,
          null,
          new List<int>(),
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 617, new Indexes(0, 2) },
              },
            ],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          null,
          null,
          new List<int> { 617 },
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 617, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(6, 8) },
              },
            ],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
              new()
              {
                { "*", new Indexes(9, 9) },
              },
            ]
          ),
          null,
          null,
          new List<int> { 617, 114 },
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 467, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(5, 7) },
              },
            ],
            [
            ]
          ),
          null,
          new Line(
            [],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          new List<int>() { 467 },
        },
        new object?[]
        {
          new Line(
            [
              new()
              {
                { 58, new Indexes(7, 8) },
              },
            ],
            [
              new()
              {
                { "+", new Indexes(5, 5) },
              },
            ]
          ),
          new Line(
            [
              new()
              {
                { 617, new Indexes(0, 2) },
              },
            ],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
          new Line(
            [
              new()
              {
                { 592, new Indexes(2, 4) },
              },
            ],
            []
          ),
          new List<int>(),
        },
      };

    public static IEnumerable<object[]> LineData =>
      new List<object[]>
      {
        new object[]
        {
          "467..114..",
          new Line(
            [
              new()
              {
                { 467, new Indexes(0, 2) },
              },
              new()
              {
                { 114, new Indexes(5, 7) },
              },
            ],
            []
        ),
        },
        new object[]
        {
          "...*......",
          new Line(
            [],
            [
              new()
              {
                { "*", new Indexes(3, 3) },
              },
            ]
          ),
        },
        new object[]
        {
          "...41+...564",
          new Line(
            [
              new()
              {
                { 41, new Indexes(3, 4) },
              },
              new()
              {
                { 564, new Indexes(9, 11) },
              },
            ],
            [
              new()
              {
                { "+", new Indexes(5, 5) },
              },
            ]
          ),
        }
      };
  };
}