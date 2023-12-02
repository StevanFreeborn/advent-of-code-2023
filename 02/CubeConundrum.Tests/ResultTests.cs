namespace CubeConundrum.Tests;

public class ResultTests
{
  [Fact]
  public void CountProperties_GivenAResult_ItShouldReturnTheCorrectCount()
  {
    var result = new Result
    {
      Cubes =
      {
        new() { Count = 4, Color = CubeColor.Red },
        new() { Count = 1, Color = CubeColor.Green },
        new() { Count = 2, Color = CubeColor.Blue }
      }
    };

    result.TotalCubeCount.Should().Be(7);
    result.RedCubeCount.Should().Be(4);
    result.GreenCubeCount.Should().Be(1);
    result.BlueCubeCount.Should().Be(2);
  }
}