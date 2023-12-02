namespace CubeConundrum.Tests;

public class PartOnePuzzleSolverTests
{
  private readonly PartOnePuzzleSolver _sut = new();

  [Theory, MemberData(nameof(TestData.Results), MemberType = typeof(TestData))]
  public void IsResultPossible_GivenAResult_ItShouldReturnExpectedOutcome(Result givenResult, bool expected)
  {
    var result = _sut.IsResultPossible(givenResult);
    result.Should().Be(expected);
  }

  [Theory, MemberData(nameof(TestData.Games), MemberType = typeof(TestData))]
  public void IsGamePossible_GivenAGame_ItShouldReturnExpectedOutcome(Game givenGame, bool expected)
  {
    var result = _sut.IsGamePossible(givenGame);
    result.Should().Be(expected);
  }

  [Fact]
  public void SumPossibleGameIds_GivenAGameCollection_ItShouldReturnTheSumOfTheIdsOfAllPossibleGames()
  {
    var games = TestData.Games.Select(o => (Game)o[0]);
    var result = _sut.SumPossibleGameIds(games);
    result.Should().Be(1);
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
                new() { Count = 2, Color = CubeColor.Green }
              }
            }
          }
        },
        true
      },
      new object[]
      {
        new Game
        {
          Id = 2,
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
                new() { Count = 40, Color = CubeColor.Green }
              }
            },
          }
        },
        false
      },
    };

    public static readonly IEnumerable<object[]> Results = new List<object[]>
    {
      new object[]
      {
        new Result
        {
          Cubes =
          {
            new() { Count = 4, Color = CubeColor.Red },
            new() { Count = 1, Color = CubeColor.Green },
            new() { Count = 2, Color = CubeColor.Blue }
          }
        },
        true
      },
      new object[]
      {
        new Result
        {
          Cubes =
          {
            new() { Count = 13, Color = CubeColor.Red },
            new() { Count = 14, Color = CubeColor.Green },
            new() { Count = 14, Color = CubeColor.Blue },
          }
        },
        false
      },
      new object[]
      {
        new Result
        {
          Cubes =
          {
            new() { Count = 13, Color = CubeColor.Red },
            new() { Count = 5, Color = CubeColor.Green },
            new() { Count = 5, Color = CubeColor.Blue },
          }
        },
        false
      },
      new object[]
      {
        new Result
        {
          Cubes =
          {
            new() { Count = 40, Color = CubeColor.Red },
          }
        },
        false
      }
    };
  }
}