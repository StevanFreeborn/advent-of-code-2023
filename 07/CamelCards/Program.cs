using System.Diagnostics;

namespace CamelCards;


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

    var jokersWild = args.Length > 1 && args[1] == "part2";
    var input = await File.ReadAllLinesAsync(args[0]);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var result = input
      .Select(line => Turn.Parse(line, jokersWild))
      .OrderBy(t => t.Hand)
      .Select((turn, index) => turn.Bid * (index + 1))
      .Sum();

    stopWatch.Stop();

    Console.WriteLine($"The total winnings is {result}. ({stopWatch.ElapsedMilliseconds}ms)");

    return result;
  }
}

public class Turn(
  Hand hand,
  int bid
)
{
  public Hand Hand { get; init; } = hand;
  public int Bid { get; init; } = bid;

  public override string ToString()
  {
    var cards = Hand.Cards
      .Select(c =>
      {
        return c.Value == 'W'
          ? 'J'
          : c.Value;
      });

    return $"{string.Join("", cards)} {Bid}";
  }

  public static Turn Parse(string turnInput, bool jokersWild = false)
  {
    var parts = turnInput.Split(' ', StringSplitOptions.TrimEntries);
    var cards = parts[0]
      .Select(c => jokersWild && c == 'J' ? new Card('W') : new Card(c))
      .ToList();
    var bid = int.Parse(parts[1]);

    return new Turn(new(cards), bid);
  }
}

public class Hand : IComparable<Hand>
{
  public List<Card> Cards { get; init; }

  public Hand(List<Card> cards)
  {
    if (cards.Count != 5)
    {
      throw new ArgumentException("A hand must have 5 cards");
    }

    Cards = cards;
  }

  public HandType Type => Cards.Any(c => c.Value == 'W')
    ? Cards
      .GroupBy(c => c.Value)
      .Count() switch
    {
      5 => HandType.OnePair,
      4 => HandType.ThreeOfAKind,
      3 => Cards.GroupBy(c => c.Value).Any(g => g.Count() == 3)
        ? HandType.FourOfAKind
        : Cards.Count(c => c.Value == 'W') == 2
          ? HandType.FourOfAKind
          : HandType.FullHouse,
      2 or
      1 => HandType.FiveOfAKind,
      _ => throw new ApplicationException("Hand has no defined type"),
    }
    : Cards
      .GroupBy(c => c.Value)
      .Count() switch
    {
      5 => HandType.HighCard,
      4 => HandType.OnePair,
      3 => Cards.GroupBy(c => c.Value).Any(g => g.Count() == 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
      2 => Cards.GroupBy(c => c.Value).Any(g => g.Count() == 4) ? HandType.FourOfAKind : HandType.FullHouse,
      1 => HandType.FiveOfAKind,
      _ => throw new ApplicationException("Hand has no defined type"),
    };

  public int CompareTo(Hand? other)
  {
    int result;

    if (other == null)
    {
      return 1;
    }

    result = Type.CompareTo(other.Type);

    if (result != 0)
    {
      return result;
    }

    for (int i = 0; i < Cards.Count; i++)
    {
      var currentCard = Cards[i];
      var otherCard = other.Cards[i];

      var cardComparison = currentCard.Strength.CompareTo(otherCard.Strength);

      if (i == Cards.Count - 1)
      {
        result = cardComparison;
      }

      if (cardComparison != 0)
      {
        result = cardComparison;
        break;
      }
    }

    return result;
  }
}

public enum HandType
{
  HighCard,
  OnePair,
  TwoPair,
  ThreeOfAKind,
  FullHouse,
  FourOfAKind,
  FiveOfAKind,
}


public class Card
{
  private static readonly Dictionary<char, int> Cards = new()
  {
    ['A'] = 13,
    ['K'] = 12,
    ['Q'] = 11,
    ['J'] = 10,
    ['T'] = 9,
    ['9'] = 8,
    ['8'] = 7,
    ['7'] = 6,
    ['6'] = 5,
    ['5'] = 4,
    ['4'] = 3,
    ['3'] = 2,
    ['2'] = 1,
    ['W'] = 0,
  };

  public char Value { get; init; }
  public int Strength { get; init; }

  public Card(char value)
  {
    var isValidCardChar = Cards.TryGetValue(value, out var strength);

    if (!isValidCardChar)
    {
      throw new ArgumentException($"Invalid card value: {value}");
    }

    Value = value;
    Strength = strength;
  }
}