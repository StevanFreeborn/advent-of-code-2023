namespace CosmicExpansion.Tests;

public class UniverseTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenUniverseWithGalaxies_ItShouldReturnListOfGalaxies(string[] givenInput, Universe expectedUniverse)
  {
    var result = Universe.Parse(givenInput);
    result.Should().BeEquivalentTo(expectedUniverse);
  }

  [Theory]
  [MemberData(nameof(TestData.GetShortestPathsBetweenGalaxiesTestData), MemberType = typeof(TestData))]
  public void GetShortestPathsBetweenGalaxies_WhenGivenUniverseWithGalaxies_ItShouldReturnShortestPathsBetweenGalaxies(Universe givenUniverse, int expectedNumberOrPaths)
  {
    var result = givenUniverse.GetShortestPathsBetweenGalaxies();
    result.Count.Should().Be(expectedNumberOrPaths);
  }

  [Theory]
  [MemberData(nameof(TestData.SumOfShortestPathsBetweenGalaxiesTestData), MemberType = typeof(TestData))]
  public void SumOfShortestPathsBetweenGalaxies_WhenGivenUniverseWithGalaxies_ItShouldReturnSumOfShortestPathsBetweenGalaxies(Universe givenUniverse, int expectedSumOfShortestPaths)
  {
    givenUniverse.SumOfShortestPathsBetweenGalaxies.Should().Be(expectedSumOfShortestPaths);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateDistanceBetweenGalaxiesTestData), MemberType = typeof(TestData))]
  public void CalculateDistanceBetweenGalaxies_WhenGivenTwoGalaxies_ItShouldReturnDistanceBetweenGalaxies(Galaxy startGalaxy, Galaxy endGalaxy, int expectedDistance)
  {
    var result = Universe.CalculateDistanceBetweenGalaxies(startGalaxy, endGalaxy);
    result.Should().Be(expectedDistance);
  }

  [Fact]
  public void SumOfShortestPathsBetweenGalaxies_WhenGivenInput_ItShouldReturnSumOfShortestPathsBetweenGalaxies()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var universe = Universe.Parse(input);
    universe.SumOfShortestPathsBetweenGalaxies.Should().Be(10292708);
  }

  [Fact]
  public void SumOfShortestPathsBetweenGalaxies_WhenGivenInputAndIsPart2_ItShouldReturnSumOfShortestPathsBetweenGalaxies()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var universe = Universe.Parse(input, 1_000_000);
    universe.SumOfShortestPathsBetweenGalaxies.Should().Be(790194712336);
  }



  public static class TestData
  {
    private static readonly Universe TestUniverse = new(
      12,
      13,
      [
        new(0, 4),
        new(1, 9),
        new(2, 0),
        new(5, 8),
        new(6, 1),
        new(7, 12),
        new(10, 9),
        new(11, 0),
        new(11, 5),
      ]
    );

    public static IEnumerable<object[]> ParseTestData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "...#......",
            ".......#..",
            "#.........",
            "..........",
            "......#...",
            ".#........",
            ".........#",
            "..........",
            ".......#..",
            "#...#.....",
          },
          TestUniverse,
        }
      };

    public static IEnumerable<object[]> GetShortestPathsBetweenGalaxiesTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestUniverse,
          36,
        }
      };

    public static IEnumerable<object[]> SumOfShortestPathsBetweenGalaxiesTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestUniverse,
          374,
        }
      };

    public static IEnumerable<object[]> CalculateDistanceBetweenGalaxiesTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestUniverse.Galaxies[4],
          TestUniverse.Galaxies[8],
          9,
        },
        new object[]
        {
          TestUniverse.Galaxies[0],
          TestUniverse.Galaxies[6],
          15,
        },
        new object[]
        {
          TestUniverse.Galaxies[2],
          TestUniverse.Galaxies[5],
          17,
        },
        new object[]
        {
          TestUniverse.Galaxies[7],
          TestUniverse.Galaxies[8],
          5,
        }
      };
  }
}