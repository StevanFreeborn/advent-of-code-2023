namespace IYGASAF.Tests;

public class AlmanacTests
{
  private readonly Almanac _sut = TestData.TestAlmanac;

  [Theory]
  [MemberData(nameof(TestData.TestAlmanacData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenValidAlmanac_ItShouldReturnExpectedAlmanac(string[] almanac, Almanac expected)
  {
    var result = Almanac.Parse(almanac);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [InlineData(79, 82)]
  [InlineData(14, 43)]
  [InlineData(55, 86)]
  [InlineData(13, 35)]
  public void GetSeedLocation_WhenGivenValidSeed_ItShouldReturnExpectedLocation(int seed, int expected)
  {
    var result = _sut.GetSeedLocation(seed);
    result.Should().Be(expected);
  }

  [Fact]
  public void GetLowestSeedLocation_WhenCalled_ItShouldReturnExpectedLocation()
  {
    var result = _sut.GetLowestSeedLocation();
    result.Should().Be(35);
  }

  [Fact]
  public void GetLowestSeedLocation_WhenCalledWithPuzzleInput_ItShouldReturnExpectedLocation()
  {
    var almanac = File.ReadAllLines("INPUT.txt");
    var sut = Almanac.Parse(almanac);
    var result = sut.GetLowestSeedLocation();
    result.Should().Be(806029445);
  }

  public static class TestData
  {
    public static readonly Almanac TestAlmanac =
      new(
        [
          79,
          14,
          55,
          13,
        ],
        [
          new(
            [
              new(2, 98, 50),
              new(48, 50, 52),
            ]
          ),
          new(
            [
              new(37, 15, 0),
              new(2, 52, 37),
              new(15, 0, 39),
            ]
          ),
          new(
            [
              new(8, 53, 49),
              new(42, 11, 0),
              new(7, 0, 42),
              new(4, 7, 57),
            ]
          ),
          new(
            [
              new(7, 18, 88),
              new(70, 25, 18),
            ]
          ),
          new(
            [
              new(23, 77, 45),
              new(19, 45, 81),
              new(13, 64, 68),
            ]
          ),
          new(
            [
              new(1, 69, 0),
              new(69, 0, 1),
            ]
          ),
          new(
            [
              new(37, 56, 60),
              new(4, 93, 56),
            ]
          ),
        ]
      );

    public static readonly IEnumerable<object[]> TestAlmanacData =
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "seeds: 79 14 55 13",
            "seed-to-soil map:",
            "50 98 2",
            "52 50 48",
            "soil-to-fertilizer map:",
            "0 15 37",
            "37 52 2",
            "39 0 15",
            "fertilizer-to-water map:",
            "49 53 8",
            "0 11 42",
            "42 0 7",
            "57 7 4",
            "water-to-light map:",
            "88 18 7",
            "18 25 70",
            "light-to-temperature map:",
            "45 77 23",
            "81 45 19",
            "68 64 13",
            "temperature-to-humidity map:",
            "0 69 1",
            "1 0 69",
            "humidity-to-location map:",
            "60 56 37",
            "56 93 4",
          },
          TestAlmanac,
        },
      };
  }
}