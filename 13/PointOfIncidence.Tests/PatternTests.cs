namespace PointOfIncidence.Tests;

public class PatternTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_GivenAPattern_ItShouldReturnPatternInstance(string[] input, Pattern expected)
  {
    var result = Pattern.Parse(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.FindPointOfReflectionTestData), MemberType = typeof(TestData))]
  public void FindPointOfReflection_GivenAPattern_ItShouldReturnPointOfReflection(string[] input, PointOfReflection expected)
  {
    var pattern = Pattern.Parse(input);
    var result = pattern.FindPointOfReflection();
    result.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public void SummarizePatternNotes_GivenExampleInput_ItShouldReturnExpectedValue()
  {
    var input = File
      .ReadAllText("EXAMPLE.txt")
      .ReplaceLineEndings();

    var patterns = input
      .Split(Environment.NewLine + Environment.NewLine)
      .Select(p => p.Split(Environment.NewLine));

    var result = patterns
      .Select(Pattern.Parse)
      .Sum(p => p.SummarizePatternNotes());

    result.Should().Be(405);
  }


  [Fact]
  public void SummarizePatternNotes_GivenInput_ItShouldReturnExpectedValue()
  {
    var input = File
      .ReadAllText("INPUT.txt")
      .ReplaceLineEndings();

    var patterns = input
      .Split(Environment.NewLine + Environment.NewLine)
      .Select(p => p.Split(Environment.NewLine));

    var result = patterns
      .Select(Pattern.Parse)
      .Select(p => p.SummarizePatternNotes())
      .Sum();

    result.Should().Be(33728);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> ParseTestData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "#.##..##.",
            "..#.##.#.",
          },
          new Pattern(
            [
              ['#', '.', '#', '#', '.', '.', '#', '#', '.'],
              ['.', '.', '#', '.', '#', '#', '.', '#', '.'],
            ],
            [
              ['#', '.'],
              ['.', '.'],
              ['#', '#'],
              ['#', '.'],
              ['.', '#'],
              ['.', '#'],
              ['#', '.'],
              ['#', '#'],
              ['.', '.'],
            ]
          ),
        }
      };

    public static IEnumerable<object[]> FindPointOfReflectionTestData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "#...##..#",
            "#....#..#",
            "..##..###",
            "#####.##.",
            "#####.##.",
            "..##..###",
            "#....#..#",
          },
          new PointOfReflection
          {
            Type = ReflectionType.Horizontal,
            StartIndex = 3,
            EndIndex = 4,
          },
        },
        new object[]
        {
          new string[]
          {
            "#.##..##.",
            "..#.##.#.",
            "##......#",
            "##......#",
            "..#.##.#.",
            "..##..##.",
            "#.#.##.#.",
          },
          new PointOfReflection
          {
            Type = ReflectionType.Vertical,
            StartIndex = 4,
            EndIndex = 5,
          }
        },
        new object[]
        {
          new string[]
          {
            "...##..##.#.#",
            ".##.#####.###",
            "##.#....####.",
            "#.#....#.#..#",
            ".#..#.#...#.#",
            ".#..#.#...#.#",
            "#.#....#.#..#",
            "##.#....####.",
            ".########.###",
            "...##..##.#.#",
            "#.#.###.....#",
            "#.#.###.....#",
            "...##..##.#.#",
          },
          new PointOfReflection
          {
            Type = ReflectionType.Horizontal,
            StartIndex = 10,
            EndIndex = 11,
          },
        },
      };
  }
}