namespace IYGASAF.Tests;

public class MapTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseMapData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenValidMap_ItShouldReturnExpectedMap(string[] ranges, Map expected)
  {
    var result = Map.Parse(ranges);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.ConvertData), MemberType = typeof(TestData))]
  public void ConvertSourceToDestination_WhenGivenValidSourceValue_ItShouldReturnExpectedDestinationValue(Map map, int sourceValue, int expected)
  {
    var result = map.ConvertSourceToDestination(sourceValue);
    result.Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly Map TestSeedToSoilMap = new(
      [
        new(2, 98, 50),
        new(48, 50, 52),
      ]
    );

    private static readonly Map TestSoilToFertilizerMap = new(
      [
        new(37, 15, 0),
        new(2, 52, 37),
        new(15, 0, 39),
      ]
    );

    private static readonly Map TestFertilizerToWaterMap = new(
      [
        new(8, 53, 49),
        new(42, 11, 0),
        new(7, 0, 42),
        new(4, 7, 57),
      ]
    );

    private static readonly Map TestWaterToLightMap = new(
      [
        new(7, 18, 88),
        new(70, 25, 18),
      ]
    );

    private static readonly Map TestLightToTemperatureMap = new(
      [
        new(23, 77, 45),
        new(19, 45, 81),
        new(13, 64, 68),
      ]
    );

    private static readonly Map TestTemperatureToHumidityMap = new(
      [
        new(1, 69, 0),
        new(69, 0, 1),
      ]
    );

    private static readonly Map TestHumidityToLocationMap = new(
      [
        new(37, 56, 60),
        new(4, 93, 56),
      ]
    );

    public static IEnumerable<object[]> ConvertData =>
      new List<object[]>
      {
        new object[]
        {
          TestSeedToSoilMap,
          79,
          81,
        },
        new object[]
        {
          TestSeedToSoilMap,
          14,
          14,
        },
        new object[]
        {
          TestSeedToSoilMap,
          55,
          57,
        },
        new object[]
        {
          TestSeedToSoilMap,
          13,
          13,
        },
        new object[]
        {
          TestSoilToFertilizerMap,
          81,
          81,
        },
        new object[]
        {
          TestSoilToFertilizerMap,
          14,
          53,
        },
        new object[]
        {
          TestSoilToFertilizerMap,
          57,
          57,
        },
        new object[]
        {
          TestSoilToFertilizerMap,
          13,
          52,
        },
        new object[]
        {
          TestFertilizerToWaterMap,
          81,
          81,
        },
        new object[]
        {
          TestFertilizerToWaterMap,
          53,
          49,
        },
        new object[]
        {
          TestFertilizerToWaterMap,
          57,
          53,
        },
        new object[]
        {
          TestFertilizerToWaterMap,
          52,
          41,
        },
        new object[]
        {
          TestWaterToLightMap,
          81,
          74,
        },
        new object[]
        {
          TestWaterToLightMap,
          49,
          42,
        },
        new object[]
        {
          TestWaterToLightMap,
          53,
          46,
        },
        new object[]
        {
          TestWaterToLightMap,
          41,
          34,
        },
        new object[]
        {
          TestLightToTemperatureMap,
          74,
          78,
        },
        new object[]
        {
          TestLightToTemperatureMap,
          42,
          42,
        },
        new object[]
        {
          TestLightToTemperatureMap,
          46,
          82,
        },
        new object[]
        {
          TestLightToTemperatureMap,
          34,
          34,
        },
        new object[]
        {
          TestTemperatureToHumidityMap,
          78,
          78,
        },
        new object[]
        {
          TestTemperatureToHumidityMap,
          42,
          43,
        },
        new object[]
        {
          TestTemperatureToHumidityMap,
          82,
          82,
        },
        new object[]
        {
          TestTemperatureToHumidityMap,
          34,
          35,
        },
        new object[]
        {
          TestHumidityToLocationMap,
          78,
          82,
        },
        new object[]
        {
          TestHumidityToLocationMap,
          43,
          43,
        },
        new object[]
        {
          TestHumidityToLocationMap,
          82,
          86,
        },
        new object[]
        {
          TestHumidityToLocationMap,
          35,
          35,
        },
      };

    public static IEnumerable<object[]> ParseMapData =>
      new List<object[]>
      {
          new object[]
          {
            new string[] { "50 98 2", "52 50 48" },
            new Map(
              [
                new(2, 98, 50),
                new(48, 50, 52)
              ]
            ),
          },
          new object[]
          {
            new string[] {"0 15 37", "37 52 2", "39 0 15"},
            new Map(
              [
                new(37, 15, 0),
                new(2, 52, 37),
                new(15, 0, 39),
              ]
            ),
          }
      };
  }
}