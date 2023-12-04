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
    var result = args.Length is 2 && args[1] == "part2"
      ? puzzleSolver.GetTotalCardCount(input)
      : puzzleSolver.SumCardValues(input);

    if (args.Length is 2 && args[1] == "part2")
    {
      Console.WriteLine($"The total number of cards is {result}.");
    }
    else
    {
      Console.WriteLine($"The total value of the cards is {result}.");
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
  /// Sums the value of the cards.
  /// </summary>
  /// <param name="cards">The cards.</param>
  /// <returns>The sum of the cards.</returns>
  public int SumCardValues(string[] cards) => cards
    .Select(Card.Parse)
    .Sum(c => c.GetCardValue());

  /// <summary>
  /// Gets the total number of cards.
  /// </summary>
  /// <param name="cards">The cards.</param>
  /// <returns>The total number of cards.</returns>
  public int GetTotalCardCount(string[] cards)
  {
    var cardStringsToProcess = new List<string>(cards);

    for (var i = 0; i < cardStringsToProcess.Count; i++)
    {
      var currentCardString = cardStringsToProcess[i];
      var currentCardStringIndex = Array.IndexOf(cards, currentCardString);
      var currentCard = Card.Parse(currentCardString);
      var numOfMatchingNumbersForCurrentCard = currentCard.MatchingNumbers.Count;
      var cardIndexesToCopy = Enumerable.Range(
        currentCardStringIndex + 1,
        Math.Min(
          numOfMatchingNumbersForCurrentCard,
          cards.Length - currentCardStringIndex - 1
        )
      );

      foreach (var index in cardIndexesToCopy)
      {
        cardStringsToProcess.Add(cards[index]);
      }
    }

    return cardStringsToProcess.Count;
  }
}

/// <summary>
/// Represents a card.
/// </summary>
/// <param name="id">The identifier.</param>
/// <param name="winningNumbers">The winning numbers.</param>
/// <param name="numbers">The numbers.</param>
/// <returns>An instance of <see cref="Card"/>.</returns>
public class Card(
  int id,
  List<int> winningNumbers,
  List<int> numbers
)
{
  /// <summary>
  /// Gets the identifier.
  /// </summary>
  public int Id { get; init; } = id;

  /// <summary>
  /// Gets the winning numbers.
  /// </summary>
  public List<int> WinningNumbers { get; init; } = winningNumbers;

  /// <summary>
  /// Gets the numbers.
  /// </summary>
  public List<int> Numbers { get; init; } = numbers;

  /// <summary>
  /// Gets the matching numbers.
  /// </summary>
  public List<int> MatchingNumbers => Numbers.Intersect(WinningNumbers).ToList();

  /// <summary>
  /// Gets the value of the card.
  /// </summary>
  /// <returns>The value of the card.</returns>
  public int GetCardValue() => MatchingNumbers.Count is 1
    ? 1
    : (int)Math.Pow(2, MatchingNumbers.Count - 1);

  /// <summary>
  /// Parses the specified input.
  /// </summary>
  /// <param name="input">The input.</param>
  /// <returns>An instance of <see cref="Card"/>.</returns>
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