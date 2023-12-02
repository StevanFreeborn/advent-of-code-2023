namespace CubeConundrum.Tests;

public class GameTests
{
  [Theory, MemberData(nameof(TestData.Games), MemberType = typeof(TestData))]
  public void PowerOfMinimumCubesNeeded_GivenAGame_ItShouldReturnTheSumOfTheMinimumCubesNeededForEachResult(Game game, int expected)
  {
    game.PowerOfMinimumCubesNeeded.Should().Be(expected);
  }

  public static class TestData
  {
    public static readonly IEnumerable<object[]> Games = new List<object[]>
    {
      new object[]
      {
        new Game
        {
          Id = 1,
          Results =
          {
            new()
            {
              Cubes =
              {
                new() { Count = 3, Color = CubeColor.Blue },
                new() { Count = 4, Color = CubeColor.Red }
              }
            },
            new()
            {
              Cubes =
              {
                new() { Count = 1, Color = CubeColor.Red },
                new() { Count = 2, Color = CubeColor.Green },
                new() { Count = 6, Color = CubeColor.Blue }
              }
            },
            new()
            {
              Cubes =
              {
                new() { Count = 2, Color = CubeColor.Green },
              }
            }
          }
        },
        48
      }
    };
  }
}