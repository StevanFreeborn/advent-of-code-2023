namespace Scratchcards;

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

    var puzzleSolver = new PuzzleSolver();

    var input = await File.ReadAllLinesAsync(args[0]);
    var result = puzzleSolver.SumCardValues(input);

    Console.WriteLine($"The sum of all card values is {result}.");

    return result;
  }
}

public class PuzzleSolver
{
  public int SumCardValues(string[] cards) => cards
    .Select(Card.Parse)
    .Sum(c => c.GetCardValue());
}

public class Card(
  int id,
  List<int> winningNumbers,
  List<int> numbers
)
{
  public int Id { get; init; } = id;
  public List<int> WinningNumbers { get; init; } = winningNumbers;
  public List<int> Numbers { get; init; } = numbers;

  public int GetCardValue()
  {
    var commonNumbers = WinningNumbers.Intersect(Numbers).ToList();

    return commonNumbers.Count is 1
      ? 1
      : (int)Math.Pow(2, commonNumbers.Count - 1);
  }

  public static Card Parse(string input)
  {
    var parts = input.Split(':');
    var cardId = int.Parse(
      parts[0].Split(
        ' ',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
      )[1]
    );

    var numberStrings = parts[1].Trim();
    var numberParts = numberStrings
      .Split('|')
      .Select(s => s.Trim())
      .ToArray();

    var winningNumbersString = numberParts[0];
    var numbersString = numberParts[1];

    var winningNumbers = winningNumbersString
      .Split(
        ' ',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
      )
      .Select(s => s.Trim())
      .Select(int.Parse)
      .ToList();

    var numbers = numbersString
      .Split(
        ' ',
        StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
      )
      .Select(s => s.Trim())
      .Select(int.Parse)
      .ToList();

    return new Card(cardId, winningNumbers, numbers);
  }
}