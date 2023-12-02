namespace CubeConundrum.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _sut = new();

  [Theory, MemberData(nameof(TestData.CubeStrings), MemberType = typeof(TestData))]
  public void ParseCube_GivenAStringThatRepresentsACube_ItShouldReturnTheEquivalentCubeModel(string input, Cube expected)
  {
    var result = _sut.ParseCube(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory, MemberData(nameof(TestData.ResultStrings), MemberType = typeof(TestData))]
  public void ParseResult_GivenAStringThatRepresentsAResult_ItShouldReturnTheEquivalentResultModel(string input, Result expected)
  {
    var result = _sut.ParseResult(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory, MemberData(nameof(TestData.GameStrings), MemberType = typeof(TestData))]
  public void ParseGame_GivenAStringThatRepresentsAGame_ItShouldReturnTheEquivalentGameModel(string input, Game expected)
  {
    var result = _sut.ParseGame(input);
    result.Should().BeEquivalentTo(expected);
  }

  public static class TestData
  {
    public static readonly IEnumerable<object[]> CubeStrings = new List<object[]>
    {
      new object[] { "4 red", new Cube { Count = 4, Color = CubeColor.Red } },
      new object[] { "1 green", new Cube { Count = 1, Color = CubeColor.Green } },
      new object[] { "2 blue", new Cube { Count = 2, Color = CubeColor.Blue } }
    };

    public static readonly IEnumerable<object[]> ResultStrings = new List<object[]>
    {
      new object[]
      {
        "4 red, 1 green, 2 blue",
        new Result
        {
          Cubes =
          [
            new (){ Count = 4, Color = CubeColor.Red },
            new (){ Count = 1, Color = CubeColor.Green },
            new() { Count = 2, Color = CubeColor.Blue }
          ]
        }
      }
    };

    public static readonly IEnumerable<object[]> GameStrings = new List<object[]>
    {
      new object[]
      {
        "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
        new Game
        {
          Id = 1,
          Results =
          [
            new()
            {
              Cubes =
              [
                new() { Count = 3, Color = CubeColor.Blue },
                new() { Count = 4, Color = CubeColor.Red }
              ]
            },
            new Result
            {
              Cubes =
              [
                new() { Count = 1, Color = CubeColor.Red },
                new() { Count = 2, Color = CubeColor.Green },
                new() { Count = 6, Color = CubeColor.Blue }
              ]
            },
            new Result
            {
              Cubes =
              [
                new(){ Count = 2, Color = CubeColor.Green }
              ]
            }
          ]
        }
      }
    };
  }
}