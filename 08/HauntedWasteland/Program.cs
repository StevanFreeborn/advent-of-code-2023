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

    var isPart2 = args.Length > 1 && args[1] == "part2";
    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var map = Map.Parse(input);
    var result = isPart2
      ? map.CountStepsToAllZNodes()
      : map.CountStepsToZ();

    stopwatch.Stop();

    Console.WriteLine($"The number of steps is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return (int)result;
  }
}

/// <summary>
/// A map of the Haunted Wasteland.
/// </summary>
/// <param name="turns">The turns to take at each step.</param>
/// <param name="nodes">The nodes in the map.</param>
/// <returns>An instance of <see cref="Map"/>.</returns>
public class Map(
  List<char> turns,
  List<Node> nodes
)
{
  /// <summary>
  /// Gets the turns to take at each step.
  /// </summary>
  public List<char> Turns { get; init; } = turns;

  /// <summary>
  /// Gets the nodes in the map.
  /// </summary>
  public List<Node> Nodes { get; init; } = nodes;

  /// <summary>
  /// Parses a map from a string array.
  /// </summary>
  /// <param name="mapInput">The map input.</param>
  /// <returns>An instance of <see cref="Map"/>.</returns>
  public static Map Parse(string[] mapInput)
  {
    var turns = mapInput[0].ToList();
    var nodes = mapInput[2..]
      .Select(Node.Parse)
      .ToList();

    return new Map(turns, nodes);
  }

  /// <summary>
  /// Counts the number of steps to the Z node.
  /// </summary>
  /// <returns>The number of steps to the Z node.</returns>
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

  /// <summary>
  /// Finds the prime factors of a number.
  /// </summary>
  /// <param name="number">The number to find the prime factors of.</param>
  /// <returns>The prime factors of the number.</returns>
  public List<long> FindPrimeFactors(long number)
  {
    var factors = new List<long>();

    // Start with the smallest prime number, 2.
    var divisor = 2;

    // Continue until the number is reduced to 2 or less.
    while (number >= 2)
    {
      // If the number is divisible by the current divisor,
      if (number % divisor == 0)
      {
        // Add the divisor to the list of factors.
        factors.Add(divisor);
        // Divide the number by the divisor to reduce it.
        number /= divisor;
      }
      else
      {
        // If the number is not divisible by the current divisor, increment the divisor.
        divisor++;
      }
    }

    return factors;
  }

  /// <summary>
  /// Finds the least common multiple of a list of numbers.
  /// </summary>
  /// <param name="numbers">The numbers to find the least common multiple of.</param>
  /// <returns>The least common multiple of the numbers.</returns>
  public long FindLeastCommonMultiple(List<long> numbers)
  {
    var primeFactors = numbers.Select(FindPrimeFactors).ToList();

    var uniquePrimeFactors = primeFactors
      .SelectMany(pf => pf)
      .Distinct()
      .ToList();

    var maxPrimeFactors = uniquePrimeFactors
      .Select(upf => primeFactors.Max(pf => pf.Count(f => f == upf)))
      .ToList();

    var result = uniquePrimeFactors
      .Zip(maxPrimeFactors)
      .Aggregate(
        (long)1,
        (acc, b) =>
          // b.First is the prime factor
          // b.Second is the number of times it occurs
          acc * (long)Math.Pow(b.First, b.Second)
      );

    return result;
  }

  /// <summary>
  /// Counts the number of steps to all Z nodes.
  /// </summary>
  /// <returns>The number of steps to all Z nodes.</returns>
  public long CountStepsToAllZNodes()
  {
    var startNodes = Nodes.Where(n => n.Current.EndsWith('A')).ToList();
    var nodeSteps = new List<long>();

    foreach (var startNode in startNodes)
    {
      var current = startNode;
      var steps = 0;

      while (current.Current.EndsWith('Z') is false)
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

      nodeSteps.Add(steps);
    }

    return FindLeastCommonMultiple(nodeSteps);
  }
}

/// <summary>
/// A node in the map.
/// </summary>
/// <param name="current">The current node.</param>
/// <param name="left">The left node.</param>
/// <param name="right">The right node.</param>
/// <returns>An instance of <see cref="Node"/>.</returns>
public class Node(
  string current,
  string left,
  string right
)
{
  /// <summary>
  /// Gets the current node.
  /// </summary>
  public string Current { get; init; } = current;

  /// <summary>
  /// Gets the left node.
  /// </summary>
  public string Left { get; init; } = left;

  /// <summary>
  /// Gets the right node.
  /// </summary>
  public string Right { get; init; } = right;

  /// <summary>
  /// Casts the node to a string.
  /// </summary>
  public override string ToString()
  {
    return $"{Current} = ({Left},{Right})";
  }

  /// <summary>
  /// Parses a node from a string.
  /// </summary>
  /// <param name="nodeString">The node string.</param>
  /// <returns>An instance of <see cref="Node"/>.</returns>
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