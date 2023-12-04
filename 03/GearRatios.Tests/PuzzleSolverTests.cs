namespace GearRatios.Test;

public class PuzzleSolverTests
{
  private readonly PuzzleSolver _sut = new();

  [Theory]
  [MemberData(nameof(TestData.SumPartNumbersData), MemberType = typeof(TestData))]
  public void SumPartNumbers_GivenSchematic_ItShouldReturnExpectedValue(string[] schematic, int expected)
  {
    var actual = _sut.SumPartNumbers(schematic);
    actual.Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.SumGearRatios), MemberType = typeof(TestData))]
  public void SumGearRatios_GivenSchematic_ItShouldReturnExpectedValue(string[] schematic, int expected)
  {
    var actual = _sut.SumGearRatios(schematic);
    actual.Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly string[] TestSchematic =
    [
      "467..114..",
      "...*......",
      "..35..633.",
      "......#...",
      "617*......",
      ".....+.58.",
      "..592.....",
      "......755.",
      "...$.*....",
      ".664.598..",
    ];

    private static readonly string[] Input = File.ReadAllLines("INPUT.txt");

    public static IEnumerable<object[]> SumGearRatios =>
      new List<object[]>
      {
        new object[]
        {
          TestSchematic,
          467835,
        },
        new object[]
        {
          Input,
          81997870,
        }
      };

    public static IEnumerable<object?[]> SumPartNumbersData =>
      new List<object?[]>
      {
        new object?[]
        {
          TestSchematic,
          4361,
        },
        new object[]
        {
          new[]
          {
            "...................305.124................................432..............................................576..313.....514.................",
            ".............113...-......&....................&...819...........654..../..........................&901................*....869.257.........",
            "...377..&783../.................................9...........855*......940..463................-.........................844.*....@......679.",
            "......*...........197.261.....817..336.759............&742......548.......&........748......844.............#.......&........254...169..*...",
          },
          10421,
        },
        new object[]
        {
          new[]
          {
            ".............................457....834.....................94..564..........&.........498....*...304....../...*.....*..........845.....&...",
            ".......12*48.753...244..196....=......$..721....90*29.........*.................+.522.......487...............317.....531..311........20....",
            ".177...............*.....*.................*.............*969.611.......*565..338...*.............712..922$.......770......*....522.........",
          },
          8318
        },
        new object[]
        {
          new[]
          {
            "663..462...........*..........109..706*.................=.............553........712......*971......674.396...635*..........................",
            "........../...728.952...413......*......744.......%..........................300..*....782................*.......742..&424........41+...564",
            "........375...%.........*......450.456.$.........714........851.327..#...+......*...+.......179.630....854.................................*",
          },
          11612
        },
        new object[]
        {
          Input,
          550934,
        }
      };
  }
}