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

  [Theory]
  [MemberData(nameof(TestData.FindPointOfReflectionWithoutSmudgeTestData), MemberType = typeof(TestData))]
  public void FindPointOfReflection_GivenAPatternAndCleanSmudgeIsTrue_ItShouldReturnPointOfReflection(string[] input, PointOfReflection expected)
  {
    var pattern = Pattern.Parse(input);
    var result = pattern.FindPointOfReflection(true);
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

    var summaries = patterns
      .Select(Pattern.Parse)
      .Select(p => p.SummarizePatternNotes());

    var result = summaries.Sum();

    result.Should().Be(33728);
  }

  [Fact]
  public void SummarizePatternNotes_GivenInputAndIsPartTwo_ItShouldReturnExpectedValue()
  {
    var input = File
      .ReadAllText("INPUT.txt")
      .ReplaceLineEndings();

    var patterns = input
      .Split(Environment.NewLine + Environment.NewLine)
      .Select(p => p.Split(Environment.NewLine));

    var summaries = patterns
      .Select(Pattern.Parse)
      .Select(p => p.SummarizePatternNotes(true));

    var result = summaries.Sum();

    result.Should().Be(28235);
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

    public static IEnumerable<object[]> FindPointOfReflectionWithoutSmudgeTestData =>
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
            StartIndex = 0,
            EndIndex = 1,
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
            Type = ReflectionType.Horizontal,
            StartIndex = 2,
            EndIndex = 3,
          }
        },
        new object[]
        {
          new string[]
          {
            "#..#.#........#",
            "#..######..####",
            ".##..#.#.##.#.#",
            "#..##..........",
            "######........#",
            "#..####......##",
            ".##.##.#...##.#",
          },
          new PointOfReflection
          {
            Type = ReflectionType.Vertical,
            StartIndex = 9,
            EndIndex = 10,
          }
        },
        new object[]
        {
          new string[]
          {
            "####.##.....###.#",
            "#####.......#..#.",
            "#####.......#..#.",
            "####..#.....###.#",
            "#..####..#.#.##.#",
            ".##..#..........#",
            "######..#.##....#",
            "#..####...###...#",
            "####..#..###.....",
            ".......#.##.##...",
            "....##.###.##..#.",
            "#..##..###...#.#.",
            "....#...#.##...##",
            ".##........##....",
            ".....#.####..##.#",
            "#..#..#..#.#.#..#",
            "#######.#.#.#....",
          },
          new PointOfReflection
          {
            Type = ReflectionType.Horizontal,
            StartIndex = 1,
            EndIndex = 2,
          },
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