using System.Diagnostics;

namespace LensLibrary;

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

    var input = await File.ReadAllTextAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = LaunchSequence
      .Parse(input)
      .Steps
      .Sum(step => step.HashLabel());

    stopwatch.Stop();

    Console.WriteLine($"The total is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return result;
  }
}

public class LaunchSequence(
  List<Step> steps
)
{
  public List<Step> Steps { get; init; } = steps;

  public long Initialize()
  {
    var boxes = new Dictionary<int, Dictionary<string, int>>();

    foreach (var num in Enumerable.Range(0, 255))
    {
      boxes.Add(num, []);
    }

    return 0;
  }

  public static LaunchSequence Parse(string input, bool part2 = false)
  {
    var steps = input
      .Split(
        ',',
        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
      )
      .Select(step => Step.Parse(step, part2))
      .ToList();

    return new LaunchSequence(steps);
  }
}

public class Step(
  string label,
  Operation operation,
  int? lensFocalLength
)
{
  public string Label { get; init; } = label;
  public Operation Operation { get; init; } = operation;
  public int? LensFocalLength { get; init; } = lensFocalLength;

  public int HashLabel() => Label
    .Aggregate(0, (hash, character) =>
    {
      var ascii = (int)character;
      hash += ascii;
      hash *= 17;
      hash %= 256;
      return hash;
    });

  public static Step Parse(string input, bool part2 = false)
  {

    if (input.Contains('-'))
    {
      var label = part2 ? input.Trim('-') : input;
      return new Step(label, Operation.Removal, null);
    }

    if (input.Contains('='))
    {

      var parts = input.Split('=');
      var label = part2 ? parts[0] : parts[0] + "=" + parts[1];
      var focalLength = int.Parse(parts[1]);
      return new Step(label, Operation.Insertion, focalLength);
    }

    throw new ArgumentException("Invalid input.", nameof(input));
  }
}

public enum Operation
{
  Removal,
  Insertion,
}