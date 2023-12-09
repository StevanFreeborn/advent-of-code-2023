using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Trebuchet;

public class Program
{
  public async static Task<int> Main(string[] args)
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

    IPuzzleSolver puzzleSolver = args.Length > 1 && args[1] == "part2"
      ? new PartTwoPuzzleSolver()
      : new PartOnePuzzleSolver();

    var input = await File.ReadAllLinesAsync(args[0]);

    var stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = puzzleSolver.SumCalibrationValues(input);

    stopwatch.Stop();

    Console.WriteLine($"The sum of all calibration values is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

    return result;
  }
}

/// <summary>
/// Interface for puzzle solvers.
/// </summary>
public interface IPuzzleSolver
{
  /// <summary>
  /// Sums all calibration values.
  /// </summary>
  /// <param name="lines">The lines of the input file.</param>
  /// <returns>The sum of all calibration values.</returns>
  public int SumCalibrationValues(string[] lines);

  /// <summary>
  /// Gets the calibration value for a given line.
  /// </summary>
  /// <param name="line">The line to get the calibration value for.</param>
  /// <returns>The calibration value for the given line.</returns>
  public int GetCalibrationValue(string line);
}

/// <summary>
/// Puzzle solver for part one.
/// </summary>
/// <inheritdoc cref="IPuzzleSolver"/>
public class PartOnePuzzleSolver : IPuzzleSolver
{
  public int SumCalibrationValues(string[] lines) => lines.Sum(GetCalibrationValue);

  public int GetCalibrationValue(string line)
  {
    if (line.Length is 0)
    {
      return 0;
    }

    var numbers = line
      .Where(char.IsDigit)
      .Select(char.ToString)
      .ToList();

    var firstNumber = numbers.First();
    var lastNumber = numbers.Last();

    return int.Parse(firstNumber + lastNumber);
  }
}

/// <summary>
/// Puzzle solver for part two.
/// </summary>
/// <inheritdoc cref="IPuzzleSolver"/>
public class PartTwoPuzzleSolver : IPuzzleSolver
{
  private readonly Dictionary<string, string> _validDigitsAsWords = new() {
    { "one", "1" },
    { "two", "2" },
    { "three", "3" },
    { "four", "4" },
    { "five", "5" },
    { "six", "6" },
    { "seven", "7" },
    { "eight", "8" },
    { "nine", "9" },
  };

  private bool HasValidDigitAsWord(string word, out string match)
  {
    var matchResult = Regex.Match(word, _validDigitsAsWords.Keys.Aggregate((a, b) => @$"{a}|{b}"));
    match = matchResult.Value;
    return matchResult.Success;
  }

  public int SumCalibrationValues(string[] lines) => lines.Sum(GetCalibrationValue);

  public int GetCalibrationValue(string line)
  {
    if (line.Length is 0)
    {
      return 0;
    }

    var currentWord = string.Empty;
    var numbers = new List<string>();

    foreach (var character in line)
    {
      currentWord += character;

      if (char.IsDigit(character))
      {
        numbers.Add(character.ToString());
        currentWord = string.Empty;
      }

      if (HasValidDigitAsWord(currentWord, out var match))
      {
        // The last character of the match could be the
        // first character of the next valid digit as word
        // so we want to preserve it for the next iteration.
        numbers.Add(_validDigitsAsWords[match]);
        currentWord = currentWord.Replace(match[..^1], string.Empty);
      }
    }

    var firstNumber = numbers.First();
    var lastNumber = numbers.Last();

    return int.Parse(firstNumber + lastNumber);
  }
}