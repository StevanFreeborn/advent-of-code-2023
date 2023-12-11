using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PipeMaze;

public class Program
{
  public static async Task<int> Main(string[] args)
  {
    if (args.Length is 0)
    {
      Console.WriteLine("Please provide a path to the input file.");
      return -1;
    }

    if (File.Exists(args[0]) is false)
    {
      Console.WriteLine("The provided file does not exist.");
      return -2;
    }

    var isPart2 = args.Length > 1 && args[1] == "part2";
    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var maze = Maze.Parse(input);
    var result = isPart2 ? maze.GetAreaOfLoop() : maze.GetFarthestStepFromStart();

    stopwatch.Stop();

    if (isPart2)
    {
      Console.WriteLine($"The area of the loop is {result}. ({stopwatch.ElapsedMilliseconds}ms)");
    }
    else
    {
      Console.WriteLine($"The farthest step from the start is {result}. ({stopwatch.ElapsedMilliseconds}ms)");
    }

    return result;
  }
}

/// <summary>
/// Represents a maze of pipes.
/// </summary>
/// <param name="width">The width of the maze.</param>
/// <param name="height">The height of the maze.</param>
/// <param name="pipes">The pipes in the maze.</param>
/// <returns>An instance of <see cref="Maze"/>.</returns>
public class Maze(
  int width,
  int height,
  List<Pipe> pipes
)
{
  private static readonly Dictionary<char, PipeType> _pipes = new()
  {
    { '|', PipeType.UpAndDown },
    { '-', PipeType.LeftAndRight },
    { 'L', PipeType.UpAndRight },
    { 'J', PipeType.UpAndLeft },
    { '7', PipeType.DownAndLeft },
    { 'F', PipeType.DownAndRight },
    { 'S', PipeType.Unknown },
  };

  private int Width { get; init; } = width;
  private int Height { get; init; } = height;

  /// <summary>
  /// Gets the pipes in the maze.
  /// </summary>
  public List<Pipe> Pipes { get; init; } = pipes;

  /// <summary>
  /// Gets the starting pipe in the maze.
  /// </summary>
  public Pipe? StartingPipe => Pipes.FirstOrDefault(p => p.Type is PipeType.Unknown);

  /// <summary>
  /// Gets the farthest step from the start.
  /// </summary>
  /// <returns>The farthest step from the start.</returns>
  public int GetFarthestStepFromStart() => GetLoop().Count / 2;

  /// <summary>
  /// Gets the area of the loop.
  /// </summary>
  /// <returns>The area of the loop.</returns>
  public int GetAreaOfLoop()
  {
    var count = 0;
    var loop = GetLoop();
    var areInsideLoop = false;
    var verticalBoundaries = new List<PipeType>()
    {
      PipeType.UpAndDown,
      PipeType.UpAndRight,
      PipeType.UpAndLeft,
    };

    // Loop through each row
    for (var row = 0; row < Height; row++)
    {
      // Loop through each column in the row
      for (var column = 0; column < Width; column++)
      {
        // Check if a pipe in loop exists at the current coordinate
        var pipe = loop.FirstOrDefault(p => p.Coordinate.Column == column && p.Coordinate.Row == row);

        // If a pipe does not exist at the current coordinate 
        // and we are inside the loop, increment the count
        if (pipe is null && areInsideLoop is true)
        {
          count++;
        }

        // If a pipe exists at the current coordinate and the pipe is 
        // representative of a vertical boundary, toggle the areInsideLoop flag
        if (
          pipe is not null &&
          verticalBoundaries.Contains(pipe.Type) is true
        )
        {
          areInsideLoop = !areInsideLoop;
        }
      }
    }

    return count;
  }

  /// <summary>
  /// Gets the loop in the maze.
  /// </summary>
  /// <returns>The loop in the maze.</returns>
  public List<Pipe> GetLoop()
  {
    if (StartingPipe is null)
    {
      return [];
    }

    var loop = new List<Pipe>();
    var startingPipeType = IdentifyStartPipeType();
    var currentPipe = new Pipe(
      StartingPipe.Symbol,
      startingPipeType,
      StartingPipe.Coordinate.Row,
      StartingPipe.Coordinate.Column
    );

    do
    {
      loop.Add(currentPipe);

      var connectingPipes = GetConnectingPipes(currentPipe);
      currentPipe = connectingPipes
        .FirstOrDefault(
          p =>
            p is not null &&
            loop.Any(pipe => pipe.Coordinate.Equals(p.Coordinate)) is false
        );

    } while (currentPipe?.Equals(StartingPipe) is false);

    return loop;
  }

  /// <summary>
  /// Identifies the type of the starting pipe.
  /// </summary>
  /// <returns>The type of the starting pipe.</returns>
  public PipeType IdentifyStartPipeType()
  {
    if (StartingPipe is null)
    {
      return PipeType.Unknown;
    }

    var connectingPipes = GetConnectingPipes(StartingPipe);
    var pipeAbove = connectingPipes.FirstOrDefault(p => p?.Coordinate.Row < StartingPipe.Coordinate.Row);
    var pipeBelow = connectingPipes.FirstOrDefault(p => p?.Coordinate.Row > StartingPipe.Coordinate.Row);
    var pipeToTheLeft = connectingPipes.FirstOrDefault(p => p?.Coordinate.Column < StartingPipe.Coordinate.Column);
    var pipeToTheRight = connectingPipes.FirstOrDefault(p => p?.Coordinate.Column > StartingPipe.Coordinate.Column);

    var isPipeAboveConnected = pipeAbove is not null &&
      pipeAbove.Type is PipeType.UpAndDown or PipeType.DownAndRight or PipeType.DownAndLeft;

    var isPipeBelowConnected = pipeBelow is not null &&
      pipeBelow.Type is PipeType.UpAndDown or PipeType.UpAndRight or PipeType.UpAndLeft;

    var isPipeToTheLeftConnected = pipeToTheLeft is not null &&
      pipeToTheLeft.Type is PipeType.LeftAndRight or PipeType.DownAndRight or PipeType.UpAndRight;

    var isPipeToTheRightConnected = pipeToTheRight is not null &&
      pipeToTheRight.Type is PipeType.LeftAndRight or PipeType.DownAndLeft or PipeType.UpAndLeft;

    if (isPipeAboveConnected && isPipeBelowConnected && isPipeToTheLeftConnected && isPipeToTheRightConnected)
    {
      return PipeType.Unknown;
    }

    if (isPipeAboveConnected && isPipeBelowConnected)
    {
      return PipeType.UpAndDown;
    }

    if (isPipeToTheLeftConnected && isPipeToTheRightConnected)
    {
      return PipeType.LeftAndRight;
    }

    if (isPipeAboveConnected && isPipeToTheRightConnected)
    {
      return PipeType.UpAndRight;
    }

    if (isPipeAboveConnected && isPipeToTheLeftConnected)
    {
      return PipeType.UpAndLeft;
    }

    if (isPipeBelowConnected && isPipeToTheRightConnected)
    {
      return PipeType.DownAndRight;
    }

    if (isPipeBelowConnected && isPipeToTheLeftConnected)
    {
      return PipeType.DownAndLeft;
    }

    return PipeType.Unknown;
  }

  /// <summary>
  /// Gets the connecting pipes of the provided pipe.
  /// </summary>
  /// <param name="pipe">The pipe to get the connecting pipes of.</param>
  /// <returns>The connecting pipes of the provided pipe.</returns>
  public List<Pipe?> GetConnectingPipes(Pipe pipe)
  {
    var connectingPipes = new List<Pipe?>();
    var isNotOnFirstRow = pipe.Coordinate.Row > 0;
    var isNotOnFirstColumn = pipe.Coordinate.Column > 0;
    var isNotOnLastRow = pipe.Coordinate.Row < Height - 1;
    var isNotOnLastColumn = pipe.Coordinate.Column < Width - 1;
    var nextRow = pipe.Coordinate.Row + 1;
    var nextColumn = pipe.Coordinate.Column + 1;
    var previousRow = pipe.Coordinate.Row - 1;
    var previousColumn = pipe.Coordinate.Column - 1;

    switch (pipe.Type)
    {
      case PipeType.UpAndDown:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == previousRow
              )
            );
          }

          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == nextRow
              )
            );
          }

          break;
        }
      case PipeType.LeftAndRight:
        {
          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == previousColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == nextColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      case PipeType.UpAndRight:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == previousRow
              )
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == nextColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      case PipeType.UpAndLeft:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == previousRow
              )
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == previousColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      case PipeType.DownAndRight:
        {
          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == nextRow
              )
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == nextColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      case PipeType.DownAndLeft:
        {
          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == nextRow
              )
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == previousColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      case PipeType.Unknown:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == previousRow
              )
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == nextColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == pipe.Coordinate.Column &&
                  p.Coordinate.Row == nextRow
              )
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(
                p =>
                  p.Coordinate.Column == previousColumn &&
                  p.Coordinate.Row == pipe.Coordinate.Row
              )
            );
          }

          break;
        }
      default:
        break;
    }

    return connectingPipes;
  }

  /// <summary>
  /// Parses the provided input into a maze.
  /// </summary>
  /// <param name="input">The input to parse.</param>
  /// <returns>An instance of <see cref="Maze"/>.</returns>
  public static Maze Parse(string[] input)
  {
    var width = input[0].Length;
    var height = input.Length;
    var pipes = new List<Pipe>();

    for (var row = 0; row < height; row++)
    {
      for (var column = 0; column < width; column++)
      {
        var symbol = input[row][column];

        if (_pipes.TryGetValue(symbol, out PipeType value))
        {
          pipes.Add(new(symbol, value, row, column));
        }
      }
    }

    return new Maze(width, height, pipes);
  }
}

/// <summary>
/// Represents a pipe in a maze.
/// </summary>
/// <param name="symbol">The symbol of the pipe.</param>
/// <param name="type">The type of the pipe.</param>
/// <param name="row">The row of the pipe.</param>
/// <param name="column">The column of the pipe.</param>
/// <returns>An instance of <see cref="Pipe"/>.</returns>
public class Pipe(
  char symbol,
  PipeType type,
  int row,
  int column
)
{
  /// <summary>
  /// Gets the symbol of the pipe.
  /// </summary>
  public char Symbol { get; init; } = symbol;

  /// <summary>
  /// Gets the type of the pipe.
  /// </summary>
  public PipeType Type { get; init; } = type;

  /// <summary>
  /// Gets the coordinate of the pipe.
  /// </summary>
  public PipeCoordinate Coordinate { get; init; } = new(column, row);
}

/// <summary>
/// Represents a coordinate of a pipe in a maze.
/// </summary>
/// <param name="column">The column of the pipe.</param>
/// <param name="row">The row of the pipe.</param>
/// <returns>An instance of <see cref="PipeCoordinate"/>.</returns>
public class PipeCoordinate(
  int column,
  int row
) : IEquatable<PipeCoordinate>
{
  /// <summary>
  /// Gets the column of the pipe.
  /// </summary>
  public int Column { get; init; } = column;

  /// <summary>
  /// Gets the row of the pipe.
  /// </summary>
  public int Row { get; init; } = row;

  /// <summary>
  /// Determines whether the provided object is equal to the current object.
  /// </summary>
  public bool Equals(PipeCoordinate? other) =>
    other is not null &&
    Column == other.Column &&
    Row == other.Row;
}

/// <summary>
/// Represents the type of a pipe in a maze.
/// </summary>
public enum PipeType
{
  UpAndDown, // Vertical Pipe
  LeftAndRight, // Horizontal Pipe
  UpAndRight, // North-East Pipe
  UpAndLeft, // North-West Pipe
  DownAndRight, // South-East Pipe
  DownAndLeft, // South-West Pipe
  Unknown, // Starting Pipe
}