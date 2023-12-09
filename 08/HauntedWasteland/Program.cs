using System.Diagnostics;

namespace HauntedWasteland;

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

    var result = Map.Parse(input).CountStepsToZ();

    stopwatch.Stop();

    Console.WriteLine($"The number of steps to Z is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return result;
  }
}

public class Map(
  List<char> turns,
  List<Node> nodes
)
{
  public List<char> Turns { get; init; } = turns;
  public List<Node> Nodes { get; init; } = nodes;

  public static Map Parse(string[] mapInput)
  {
    var turns = mapInput[0].ToList();
    var nodes = mapInput[2..]
      .Select(Node.Parse)
      .ToList();

    return new Map(turns, nodes);
  }

  public int CountStepsToZ()
  {
    var current = Nodes.First(n => n.Current == "AAA");
    var steps = 0;

    while (current.Current != "ZZZ")
    {
      var next = Turns[steps % Turns.Count] switch
      {
        'R' => current.Right,
        'L' => current.Left,
        _ => throw new Exception("Invalid turn")
      };
      current = Nodes.First(n => n.Current == next);
      steps++;
    }

    return steps;
  }
}

public class Node(
  string current,
  string left,
  string right
)
{
  public string Current { get; init; } = current;
  public string Left { get; init; } = left;
  public string Right { get; init; } = right;

  public override string ToString()
  {
    return $"{Current} = ({Left},{Right})";
  }

  public static Node Parse(string nodeString)
  {
    var parts = nodeString.Split(
      '=',
      StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
    );
    var current = parts[0];
    var nextNodes = parts[1].Split(
      ',',
      StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
    );
    var left = nextNodes[0].Trim('(');
    var right = nextNodes[1].Trim(')');

    return new Node(current, left, right);
  }
}