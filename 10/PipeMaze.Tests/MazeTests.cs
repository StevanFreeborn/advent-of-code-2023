namespace PipeMaze.Tests;

public class MazeTests
{

  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenInput_ItShouldReturnExpectedMaze(string[] input, Maze expectedMaze, Pipe? expectedStartingPipe)
  {
    var maze = Maze.Parse(input);

    maze
      .Should()
      .BeEquivalentTo(expectedMaze);

    maze.StartingPipe
      .Should()
      .BeEquivalentTo(expectedStartingPipe);
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
    maze.GetFarthestStepFromStart()
      .Should()
      .Be(expectedFurthestStepFromStart);
  }

  [Theory]
  [MemberData(nameof(TestData.AreaTestData), MemberType = typeof(TestData))]
  public void Area_WhenGivenInput_ItShouldReturnExpectedArea(string[] input, int expectedArea)
  {
    Maze
      .Parse(input)
      .GetAreaOfLoop()
      .Should()
      .Be(expectedArea);
  }

  public static class TestData
  {
    private static readonly string[] Input = File.ReadAllLines("INPUT.txt");

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

    public static IEnumerable<object[]> AreaTestData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "...........",
            ".S-------7.",
            ".|F-----7|.",
            ".||.....||.",
            ".||.....||.",
            ".|L-7.F-J|.",
            ".|..|.|..|.",
            ".L--J.L--J.",
            "...........",
          },
          4,
        },
        new object[]
        {
          new string[]
          {
            ".F----7F7F7F7F-7....",
            ".|F--7||||||||FJ....",
            ".||.FJ||||||||L7....",
            "FJL7L7LJLJ||LJ.L-7..",
            "L--J.L7...LJS7F-7L7.",
            "....F-J..F7FJ|L7L7L7",
            "....L7.F7||L7|.L7L7|",
            ".....|FJLJ|FJ|F7|.LJ",
            "....FJL-7.||.||||...",
            "....L---J.LJ.LJLJ...",
          },
          8
        },
        new object[]
        {
          new string[]
          {
            "FF7FSF7F7F7F7F7F---7",
            "L|LJ||||||||||||F--J",
            "FL-7LJLJ||||||LJL-77",
            "F--JF--7||LJLJ7F7FJ-",
            "L---JF-JLJ.||-FJLJJ7",
            "|F|F-JF---7F7-L7L|7|",
            "|FFJF7L7F-JF7|JL---7",
            "7-L-JL7||F7|L7F-7F7|",
            "L.L7LFJ|||||FJL7||LJ",
            "L7JLJL-JLJLJL--JLJ.L",
          },
          10
        },
        new object[]
        {
          Input,
          287,
        }
      };

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
        },
        new object[]
        {
          Maze.Parse(Input),
          6870,
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