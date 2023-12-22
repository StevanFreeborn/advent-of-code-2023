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
    var steps = input.Split(
      ',',
      StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries
    );

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var total = 0;

    foreach (var step in steps)
    {
      var stepValue = 0;

      foreach (var character in step)
      {
        var ascii = (int)character;
        stepValue += ascii;
        stepValue *= 17;
        stepValue %= 256;
      }

      total += stepValue;
    }

    stopwatch.Stop();

    Console.WriteLine($"The total is {total}. ({stopwatch.ElapsedMilliseconds}ms)");

    return total;
  }
}