using System.Diagnostics;

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

    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = Maze.Parse(input).FarthestStepFromStart;

    stopwatch.Stop();

    Console.WriteLine($"The farthest step from the starting pipe is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return result;
  }
}

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

  public int Width { get; init; } = width;
  public int Height { get; init; } = height;
  public List<Pipe> Pipes { get; init; } = pipes;

  public Pipe? StartingPipe => Pipes.FirstOrDefault(p => p.Type is PipeType.Unknown);

  public int FarthestStepFromStart => GetLoop().Count / 2;

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

  public List<Pipe?> GetConnectingPipes(Pipe pipe)
  {
    var connectingPipes = new List<Pipe?>();
    var isNotOnFirstRow = pipe.Coordinate.Row > 0;
    var isNotOnFirstColumn = pipe.Coordinate.Column > 0;
    var isNotOnLastRow = pipe.Coordinate.Row < Height - 1;
    var isNotOnLastColumn = pipe.Coordinate.Column < Width - 1;

    switch (pipe.Type)
    {
      case PipeType.UpAndDown:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row - 1)
            );
          }

          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row + 1)
            );
          }

          break;
        }
      case PipeType.LeftAndRight:
        {
          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column - 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column + 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      case PipeType.UpAndRight:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row - 1)
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column + 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      case PipeType.UpAndLeft:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row - 1)
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column - 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      case PipeType.DownAndRight:
        {
          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row + 1)
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column + 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      case PipeType.DownAndLeft:
        {
          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row + 1)
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column - 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      case PipeType.Unknown:
        {
          if (isNotOnFirstRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row - 1)
            );
          }

          if (isNotOnLastColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column + 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          if (isNotOnLastRow)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column && p.Coordinate.Row == pipe.Coordinate.Row + 1)
            );
          }

          if (isNotOnFirstColumn)
          {
            connectingPipes.Add(
              Pipes.FirstOrDefault(p => p.Coordinate.Column == pipe.Coordinate.Column - 1 && p.Coordinate.Row == pipe.Coordinate.Row)
            );
          }

          break;
        }
      default:
        break;
    }

    return connectingPipes;
  }

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

public class Pipe(
  char symbol,
  PipeType type,
  int row,
  int column
)
{
  public char Symbol { get; init; } = symbol;
  public PipeType Type { get; init; } = type;
  public PipeCoordinate Coordinate { get; init; } = new(column, row);
}

public class PipeCoordinate(
  int column,
  int row
) : IEquatable<PipeCoordinate>
{
  public int Column { get; init; } = column;
  public int Row { get; init; } = row;

  public bool Equals(PipeCoordinate? other) =>
    other is not null &&
    Column == other.Column &&
    Row == other.Row;
}

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