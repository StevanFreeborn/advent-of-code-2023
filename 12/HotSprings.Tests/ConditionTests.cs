namespace HotSprings.Tests;

public class ConditionTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenConditionRecordInput_ItShouldReturnExpectedConditionInstance(string input, Condition expected)
  {
    var result = Condition.Parse(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.GetNumberOfPossibleConfigurationsTestData), MemberType = typeof(TestData))]
  public void GetNumberOfPossibleConfigurations_WhenGivenCondition_ItShouldReturnExpectedNumberOfPossibleConfigurations(Condition condition, long expected)
  {
    var result = condition.GetNumberOfPossibleConfigurations();
    result.Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.ParseAndUnFoldTestData), MemberType = typeof(TestData))]
  public void ParseAndUnFold_WhenGivenConditionRecordInput_ItShouldReturnExpectedConditionInstance(string input, Condition expected)
  {
    var result = Condition.Parse(input, true);
    result.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public void Condition_WhenGivenInput_ItShouldReturnExpectedCount()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var result = input
      .Select(c => Condition.Parse(c))
      .Select(c => c.GetNumberOfPossibleConfigurations())
      .Sum();

    result.Should().Be(6958);
  }

  [Fact]
  public void Condition_WhenGivenInputAndPartTwo_ItShouldReturnExpectedCount()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var result = input
      .Select(c => Condition.Parse(c, true))
      .Select(c => c.GetNumberOfPossibleConfigurations())
      .Sum();

    result.Should().Be(6555315065024);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> GetNumberOfPossibleConfigurationsTestData =>
      new List<object[]>
      {
        new object[]
        {
          new Condition(
            [1, 1, 3],
            [7, 5, 3],
            [
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged
            ]
          ),
          1,
        },
        new object[]
        {
          new Condition(
            [1, 1, 3],
            [7, 5, 3],
            [
              SpringCondition.Operational,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Operational,
              SpringCondition.Operational,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Operational,
              SpringCondition.Operational,
              SpringCondition.Operational,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Operational,
            ]
          ),
          4,
        },
        new object[]
        {
          new Condition(
            [1, 3, 1, 6],
            [11, 10, 6, 1],
            [
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
            ]
          ),
          1,
        },
        new object[]
        {
          new Condition(
            [1, 6, 5],
            [12, 6, 1],
            [
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
            ]
          ),
          4,
        },
        new object[]
        {
          new Condition(
            [3, 2, 1],
            [6, 3, 1],
            [
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
              SpringCondition.Unknown,
            ]
          ),
          10,
        }
      };

    public static IEnumerable<object[]> ParseAndUnFoldTestData =>
      new List<object[]>
      {
        new object[]
        {
          "#.#.### 1,1,3",
          new Condition(
            [1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3, 1, 1, 3],
            [39, 37, 35, 31, 29, 27, 23, 21, 19, 15, 13, 11, 7, 5, 3],
            [
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Unknown,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
            ]
          )
        }
      };


    public static IEnumerable<object[]> ParseTestData =>
      new List<object[]>
      {
        new object[]
        {
          "#.#.### 1,1,3",
          new Condition(
            [1, 1, 3],
            [7, 5, 3],
            [
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Operational,
              SpringCondition.Damaged,
              SpringCondition.Damaged,
              SpringCondition.Damaged
            ]
          )
        }
      };
  }
}