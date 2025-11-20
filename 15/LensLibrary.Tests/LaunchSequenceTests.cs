namespace LensLibrary.Tests;

public class LaunchSequenceTests
{
  [Theory]
  [MemberData(nameof(TestData.InitializeTestData), MemberType = typeof(TestData))]
  public void Initialize_WhenCalled_ItShouldReturnFocusPowerOfLensConfiguration(LaunchSequence sequence, long expected)
  {
    var result = sequence.Initialize();
    result.Should().Be(expected);
  }

  [Fact]
  public void Initialize_WhenCalledOnInputLaunchSequence_ItShouldReturnFocusPowerOfLensConfiguration()
  {
    var input = File.ReadAllText("INPUT.txt");
    var sequence = LaunchSequence.Parse(input, true);
    var result = sequence.Initialize();
    result.Should().BeGreaterThan(244789);
  }


  public static class TestData
  {
    public static readonly LaunchSequence TestSequence = new(
      [
        new Step("rn", Operation.Insertion, 1),
        new Step("cm", Operation.Removal, null),
        new Step("qp", Operation.Insertion, 3),
        new Step("cm", Operation.Insertion, 2),
        new Step("qp", Operation.Removal, null),
        new Step("pc", Operation.Insertion, 4),
        new Step("ot", Operation.Insertion, 9),
        new Step("ab", Operation.Insertion, 5),
        new Step("pc", Operation.Removal, null),
        new Step("pc", Operation.Insertion, 6),
        new Step("ot", Operation.Insertion, 7),
      ]
    );

    public static IEnumerable<object[]> InitializeTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestSequence,
          145,
        }
      };
  }
}