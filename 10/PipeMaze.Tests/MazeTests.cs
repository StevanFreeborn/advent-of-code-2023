namespace PipeMaze.Tests;

public class MazeTests
{

  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenInput_ItShouldReturnExpectedMaze(string[] input, Maze expectedMaze, Pipe? expectedStartingPipe)
  {
    var maze = Maze.Parse(input);
    maze.Should().BeEquivalentTo(expectedMaze);
    maze.StartingPipe.Should().BeEquivalentTo(expectedStartingPipe);
  }

  [Theory]
  [MemberData(nameof(TestData.IdentifyPipeTypeTestData), MemberType = typeof(TestData))]
  public void IdentifyPipeType_WhenGivenInput_ItShouldReturnExpectedPipeType(Maze maze, PipeType expectedPipeType)
  {
    maze
      .IdentifyStartPipeType()
      .Should()
      .Be(expectedPipeType);
  }

  [Theory]
  [MemberData(nameof(TestData.GetLoopTestData), MemberType = typeof(TestData))]
  public void GetLoop_WhenGivenInput_ItShouldReturnExpectedLoop(Maze maze, List<Pipe> expectedLoop)
  {
    maze
      .GetLoop()
      .Should()
      .BeEquivalentTo(expectedLoop);
  }

  [Theory]
  [MemberData(nameof(TestData.FurthestStepFromStartTestData), MemberType = typeof(TestData))]
  public void FurthestStepFromStart_WhenGivenInput_ItShouldReturnExpectedFurthestStepFromStart(Maze maze, int expectedFurthestStepFromStart)
  {
    maze.FarthestStepFromStart
      .Should()
      .Be(expectedFurthestStepFromStart);
  }

  public static class TestData
  {
    private static readonly string[] TestMazeInput =
    {
      ".....",
      ".S-7.",
      ".|.|.",
      ".L-J.",
      ".....",
    };

    private static readonly Maze TestMaze =
      new(
        5,
        5,
        [
          new('S', PipeType.Unknown, 1, 1),
          new('-', PipeType.LeftAndRight, 1, 2),
          new('7', PipeType.DownAndLeft, 1, 3),
          new('|', PipeType.UpAndDown, 2, 1),
          new('|', PipeType.UpAndDown, 2, 3),
          new('L', PipeType.UpAndRight, 3, 1),
          new('-', PipeType.LeftAndRight, 3, 2),
          new('J', PipeType.UpAndLeft, 3, 3),
        ]
      );

    public static IEnumerable<object[]> FurthestStepFromStartTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestMaze,
          4
        },
        new object[]
        {
          new Maze(
            5,
            5,
            [
              new('7', PipeType.DownAndLeft, 0, 0),
              new('-', PipeType.UpAndRight, 0, 1),
              new('F', PipeType.DownAndRight, 0, 2),
              new('7', PipeType.DownAndLeft, 0, 3),
              new('-', PipeType.LeftAndRight, 0, 4),

              new('F', PipeType.DownAndRight, 1, 1),
              new('J', PipeType.UpAndLeft, 1, 2),
              new('|', PipeType.UpAndDown, 1, 3),
              new('7', PipeType.DownAndLeft, 1, 4),

              new('S', PipeType.Unknown, 2, 0),
              new('J', PipeType.UpAndLeft, 2, 1),
              new('L', PipeType.UpAndRight, 2, 2),
              new('L', PipeType.UpAndRight, 2, 3),
              new('7', PipeType.DownAndLeft, 2, 4),

              new('|', PipeType.UpAndDown, 3, 0),
              new('F', PipeType.DownAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('-', PipeType.LeftAndRight, 3, 3),
              new('J', PipeType.UpAndLeft, 3, 4),

              new('L', PipeType.UpAndRight, 4, 0),
              new('J', PipeType.UpAndLeft, 4, 1),
              new('L', PipeType.UpAndRight, 4, 3),
              new('J', PipeType.UpAndLeft, 4, 4),
            ]
          ),
          8,
        }
      };

    public static IEnumerable<object[]> GetLoopTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestMaze,
          new List<Pipe>
          {
            new('S', PipeType.DownAndRight, 1, 1),
            new('-', PipeType.LeftAndRight, 1, 2),
            new('7', PipeType.DownAndLeft, 1, 3),
            new('|', PipeType.UpAndDown, 2, 1),
            new('|', PipeType.UpAndDown, 2, 3),
            new('L', PipeType.UpAndRight, 3, 1),
            new('-', PipeType.LeftAndRight, 3, 2),
            new('J', PipeType.UpAndLeft, 3, 3),
          }
        },
        new object[]
        {
          new Maze(
            5,
            5,
            [
              new('-', PipeType.LeftAndRight, 0, 0),
              new('L', PipeType.UpAndRight, 0, 1),
              new('|', PipeType.UpAndDown, 0, 2),
              new('F', PipeType.DownAndRight, 0, 3),
              new('7', PipeType.DownAndLeft, 0, 4),

              new('7', PipeType.DownAndLeft, 1, 0),
              new('S', PipeType.Unknown, 1, 1),
              new('-', PipeType.LeftAndRight, 1, 2),
              new('7', PipeType.DownAndLeft, 1, 3),
              new('|', PipeType.UpAndDown, 1, 4),

              new('L', PipeType.UpAndRight, 2, 0),
              new('|', PipeType.UpAndDown, 2, 1),
              new('7', PipeType.DownAndLeft, 2, 2),
              new('|', PipeType.UpAndDown, 2, 3),
              new('|', PipeType.UpAndDown, 2, 4),

              new('-', PipeType.LeftAndRight, 3, 0),
              new('L', PipeType.UpAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('J', PipeType.UpAndLeft, 3, 3),
              new('|', PipeType.UpAndDown, 3, 4),

              new('L', PipeType.UpAndRight, 4, 0),
              new('|', PipeType.UpAndDown, 4, 1),
              new('-', PipeType.LeftAndRight, 4, 2),
              new('J', PipeType.UpAndLeft, 4, 3),
              new('F', PipeType.DownAndRight, 4, 4),
            ]
          ),
          new List<Pipe>
          {
            new('S', PipeType.DownAndRight, 1, 1),
            new('-', PipeType.LeftAndRight, 1, 2),
            new('7', PipeType.DownAndLeft, 1, 3),
            new('|', PipeType.UpAndDown, 2, 1),
            new('|', PipeType.UpAndDown, 2, 3),
            new('L', PipeType.UpAndRight, 3, 1),
            new('-', PipeType.LeftAndRight, 3, 2),
            new('J', PipeType.UpAndLeft, 3, 3),
          }
        },
        new object[]
        {
          new Maze(
            5,
            5,
            [
              new('F', PipeType.DownAndRight, 0, 2),
              new('7', PipeType.DownAndLeft, 0, 3),

              new('F', PipeType.DownAndRight, 1, 1),
              new('J', PipeType.UpAndLeft, 1, 2),
              new('|', PipeType.UpAndDown, 1, 3),

              new('S', PipeType.Unknown, 2, 0),
              new('J', PipeType.UpAndLeft, 2, 1),
              new('L', PipeType.UpAndRight, 2, 3),
              new('7', PipeType.DownAndLeft, 2, 4),

              new('|', PipeType.UpAndDown, 3, 0),
              new('F', PipeType.DownAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('-', PipeType.LeftAndRight, 3, 3),
              new('J', PipeType.UpAndLeft, 3, 4),

              new('L', PipeType.UpAndRight, 4, 0),
              new('J', PipeType.UpAndLeft, 4, 1),
            ]
          ),
          new List<Pipe>
          {
            new('F', PipeType.DownAndRight, 0, 2),
            new('7', PipeType.DownAndLeft, 0, 3),

            new('F', PipeType.DownAndRight, 1, 1),
            new('J', PipeType.UpAndLeft, 1, 2),
            new('|', PipeType.UpAndDown, 1, 3),

            new('S', PipeType.DownAndRight, 2, 0),
            new('J', PipeType.UpAndLeft, 2, 1),
            new('L', PipeType.UpAndRight, 2, 3),
            new('7', PipeType.DownAndLeft, 2, 4),

            new('|', PipeType.UpAndDown, 3, 0),
            new('F', PipeType.DownAndRight, 3, 1),
            new('-', PipeType.LeftAndRight, 3, 2),
            new('-', PipeType.LeftAndRight, 3, 3),
            new('J', PipeType.UpAndLeft, 3, 4),

            new('L', PipeType.UpAndRight, 4, 0),
            new('J', PipeType.UpAndLeft, 4, 1),
          }
        },
        new object[]
        {
          new Maze(
            5,
            5,
            [
              new('7', PipeType.DownAndLeft, 0, 0),
              new('-', PipeType.UpAndRight, 0, 1),
              new('F', PipeType.DownAndRight, 0, 2),
              new('7', PipeType.DownAndLeft, 0, 3),
              new('-', PipeType.LeftAndRight, 0, 4),

              new('F', PipeType.DownAndRight, 1, 1),
              new('J', PipeType.UpAndLeft, 1, 2),
              new('|', PipeType.UpAndDown, 1, 3),
              new('7', PipeType.DownAndLeft, 1, 4),

              new('S', PipeType.Unknown, 2, 0),
              new('J', PipeType.UpAndLeft, 2, 1),
              new('L', PipeType.UpAndRight, 2, 2),
              new('L', PipeType.UpAndRight, 2, 3),
              new('7', PipeType.DownAndLeft, 2, 4),

              new('|', PipeType.UpAndDown, 3, 0),
              new('F', PipeType.DownAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('-', PipeType.LeftAndRight, 3, 3),
              new('J', PipeType.UpAndLeft, 3, 4),

              new('L', PipeType.UpAndRight, 4, 0),
              new('J', PipeType.UpAndLeft, 4, 1),
              new('L', PipeType.UpAndRight, 4, 3),
              new('J', PipeType.UpAndLeft, 4, 4),
            ]
          ),
          new List<Pipe>
          {
            new('F', PipeType.DownAndRight, 0, 2),
            new('7', PipeType.DownAndLeft, 0, 3),

            new('F', PipeType.DownAndRight, 1, 1),
            new('J', PipeType.UpAndLeft, 1, 2),
            new('|', PipeType.UpAndDown, 1, 3),

            new('S', PipeType.DownAndRight, 2, 0),
            new('J', PipeType.UpAndLeft, 2, 1),
            new('L', PipeType.UpAndRight, 2, 3),
            new('7', PipeType.DownAndLeft, 2, 4),

            new('|', PipeType.UpAndDown, 3, 0),
            new('F', PipeType.DownAndRight, 3, 1),
            new('-', PipeType.LeftAndRight, 3, 2),
            new('-', PipeType.LeftAndRight, 3, 3),
            new('J', PipeType.UpAndLeft, 3, 4),

            new('L', PipeType.UpAndRight, 4, 0),
            new('J', PipeType.UpAndLeft, 4, 1),
          }
        }
      };

    public static IEnumerable<object[]> IdentifyPipeTypeTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestMaze,
          PipeType.DownAndRight,
        }
      };

    public static IEnumerable<object?[]> ParseTestData =>
      new List<object?[]>
      {
        new object?[]
        {
          new string[]
          {
            ".....",
            ".F-7.",
            ".|.|.",
            ".L-J.",
            ".....",
          },
          new Maze(
            5,
            5,
            [
              new('F', PipeType.DownAndRight, 1, 1),
              new('-', PipeType.LeftAndRight, 1, 2),
              new('7', PipeType.DownAndLeft, 1, 3),
              new('|', PipeType.UpAndDown, 2, 1),
              new('|', PipeType.UpAndDown, 2, 3),
              new('L', PipeType.UpAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('J', PipeType.UpAndLeft, 3, 3),
            ]
          ),
          null
        },
        new object?[]
        {
          TestMazeInput,
          new Maze(
            5,
            5,
            [
              new('S', PipeType.Unknown, 1, 1),
              new('-', PipeType.LeftAndRight, 1, 2),
              new('7', PipeType.DownAndLeft, 1, 3),
              new('|', PipeType.UpAndDown, 2, 1),
              new('|', PipeType.UpAndDown, 2, 3),
              new('L', PipeType.UpAndRight, 3, 1),
              new('-', PipeType.LeftAndRight, 3, 2),
              new('J', PipeType.UpAndLeft, 3, 3),
            ]
          ),
          new Pipe('S', PipeType.Unknown, 1, 1)
        }
      };
  }
}