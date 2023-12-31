﻿namespace GearRatios;

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

    var puzzleSolver = new PuzzleSolver();

    var input = await File.ReadAllLinesAsync(args[0]);
    var result = args.Length > 1 && args[1] == "part2"
      ? puzzleSolver.SumGearRatios(input)
      : puzzleSolver.SumPartNumbers(input);

    if (args.Length > 1 && args[1] == "part2")
    {
      Console.WriteLine($"The sum of all gear ratios is {result}.");
    }
    else
    {
      Console.WriteLine($"The sum of all part numbers is {result}.");
    }

    return result;
  }
}

/// <summary>
/// Solves the puzzle.
/// </summary>
public class PuzzleSolver
{
  /// <summary>
  /// Sums all part numbers in the schematic.
  /// </summary>
  /// <param name="schematic">The schematic to parse.</param>
  /// <returns>The sum of all part numbers.</returns>
  public int SumPartNumbers(string[] schematic)
  {
    var allPartNumbers = new List<int>();

    for (var i = 0; i < schematic.Length; i++)
    {
      var currentLine = Line.Parse(schematic[i]);
      var prevLine = i > 0 ? Line.Parse(schematic[i - 1]) : null;
      var nextLine = i < schematic.Length - 1 ? Line.Parse(schematic[i + 1]) : null;
      var linePartNumbers = currentLine.GetPartNumbers(prevLine, nextLine);
      allPartNumbers.AddRange(linePartNumbers);
    }

    return allPartNumbers.Sum();
  }

  /// <summary>
  /// Sums all gear ratios in the schematic.
  /// </summary>
  /// <param name="schematic">The schematic to parse.</param>
  /// <returns>The sum of all gear ratios.</returns>
  public int SumGearRatios(string[] schematic)
  {
    var allGearRatios = new List<int>();

    for (var i = 0; i < schematic.Length; i++)
    {
      var currentLine = Line.Parse(schematic[i]);
      var prevLine = i > 0 ? Line.Parse(schematic[i - 1]) : null;
      var nextLine = i < schematic.Length - 1 ? Line.Parse(schematic[i + 1]) : null;
      var lineGearRatios = currentLine.GetGearRatios(prevLine, nextLine);
      allGearRatios.AddRange(lineGearRatios);
    }

    return allGearRatios.Sum();
  }
}

/// <summary>
/// Represents a line in the schematic.
/// </summary>
/// <param name="numbers">The numbers in the line.</param>
/// <param name="specialCharacters">The special characters in the line.</param>
/// <returns>A line in the schematic.</returns>
public class Line(
  List<Dictionary<int, Indexes>> numbers,
  List<Dictionary<string, Indexes>> specialCharacters
)
{
  /// <summary>
  /// Gets the numbers in the line.
  /// </summary>
  public List<Dictionary<int, Indexes>> Numbers { get; init; } = numbers;

  /// <summary>
  /// Gets the special characters in the line.
  /// </summary>
  public List<Dictionary<string, Indexes>> SpecialCharacters { get; init; } = specialCharacters;

  /// <summary>
  /// Gets the indexes of all special characters in the line.
  /// </summary>
  public List<int> SpecialCharacterIndexes => SpecialCharacters.SelectMany(dict => dict.Values).Select(v => v.Start).ToList();

  private bool IsGearCharacter(string character) => character == "*";

  /// <summary>
  /// Gets the gear ratios in the line.
  /// </summary>
  /// <param name="previousLine">The previous line in the schematic.</param>
  /// <param name="nextLine">The next line in the schematic.</param>
  /// <returns>A list of gear ratios.</returns>
  public List<int> GetGearRatios(Line? previousLine = null, Line? nextLine = null)
  {
    var gearRatios = new List<int>();
    var gearCharacters = SpecialCharacters.Where(dict => dict.Keys.Any(IsGearCharacter)).ToList();

    if (previousLine is not null)
    {
      Numbers.AddRange(previousLine.Numbers);
    }

    if (nextLine is not null)
    {
      Numbers.AddRange(nextLine.Numbers);
    }

    foreach (var gear in gearCharacters)
    {
      foreach (var (key, value) in gear)
      {
        var beforeGear = value.Start - 1;
        var afterGear = value.End + 1;

        var gearNumbers = Numbers
          .Where(
            n => n.Values.Any(
              v =>
                v.Start >= beforeGear && v.Start <= afterGear ||
                v.End >= beforeGear && v.End <= afterGear
            )
          )
          .SelectMany(n => n.Keys)
          .ToList();

        if (gearNumbers.Count == 2)
        {
          gearRatios.Add(gearNumbers.First() * gearNumbers.Last());
        }
      }
    }

    return gearRatios;
  }

  /// <summary>
  /// Gets the part numbers in the line.
  /// </summary>
  /// <param name="previousLine">The previous line in the schematic.</param>
  /// <param name="nextLine">The next line in the schematic.</param>
  /// <returns>A list of part numbers.</returns>
  public List<int> GetPartNumbers(Line? previousLine = null, Line? nextLine = null)
  {
    var partNumbers = new List<int>();

    if (previousLine is not null)
    {
      SpecialCharacters.AddRange(previousLine.SpecialCharacters);
    }

    if (nextLine is not null)
    {
      SpecialCharacters.AddRange(nextLine.SpecialCharacters);
    }

    foreach (var number in Numbers)
    {
      foreach (var (key, value) in number)
      {
        var beforeNumber = value.Start - 1;
        var afterNumber = value.End + 1;

        if (SpecialCharacterIndexes.Any(idx => idx >= beforeNumber && idx <= afterNumber))
        {
          partNumbers.Add(key);
        }
      }
    }

    return partNumbers;
  }

  /// <summary>
  /// Parses a line in the schematic.
  /// </summary>
  /// <param name="line">The line to parse.</param>
  /// <returns>An instance of <see cref="Line"/>.</returns>
  public static Line Parse(string line)
  {
    var numbers = new List<Dictionary<int, Indexes>>();
    var specialCharacters = new List<Dictionary<string, Indexes>>();

    var number = string.Empty;
    int? numberStart = null;
    int? numberEnd;

    for (var i = 0; i < line.Length; i++)
    {
      var currentCharacter = line[i];

      if (char.IsDigit(currentCharacter))
      {
        number += line[i];
        numberStart ??= i;

        if (i == line.Length - 1)
        {
          numberEnd = i;
          numbers.Add(new()
          {
            {
              int.Parse(number),
              new Indexes(numberStart.Value, numberEnd.Value)
            }
          });
        }
      }
      else
      {
        if (numberStart.HasValue)
        {
          numberEnd = i - 1;
          numbers.Add(new()
          {
            {
              int.Parse(number),
              new Indexes(numberStart.Value, numberEnd.Value)
            }
          });
          number = string.Empty;
          numberStart = null;
        }

        if (currentCharacter == '.')
        {
          continue;
        }

        specialCharacters.Add(new()
        {
          {
            currentCharacter.ToString(),
            new Indexes(i, i)
          }
        });
      }
    }

    return new Line(numbers, specialCharacters);
  }
}

/// <summary>
/// Represents the start and end indexes of a number or special character.
/// </summary>
public class Indexes(int start, int end)
{
  /// <summary>
  /// Gets the start index. The start index is the position of the first character in the number or special character.
  /// </summary>
  public int Start { get; init; } = start;

  /// <summary>
  /// Gets the end index. The end index is the position of the last character in the number or special character.
  /// </summary>
  public int End { get; init; } = end;
}