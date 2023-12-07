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

    var input = await File.ReadAllLinesAsync(args[0]);

    var stopWatch = new Stopwatch();
    stopWatch.Start();

    var result = input
      .Select(Turn.Parse)
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
    return $"{string.Join("", Hand.Cards.Select(c => c.Value))} {Bid}";
  }

  public static Turn Parse(string turnInput)
  {
    var parts = turnInput.Split(' ', StringSplitOptions.TrimEntries);
    var cards = parts[0].Select(c => new Card(c)).ToList();
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

  public HandType Type => Cards.GroupBy(c => c.Value).Count() switch
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
    ['A'] = 12,
    ['K'] = 11,
    ['Q'] = 10,
    ['J'] = 9,
    ['T'] = 8,
    ['9'] = 7,
    ['8'] = 6,
    ['7'] = 5,
    ['6'] = 4,
    ['5'] = 3,
    ['4'] = 2,
    ['3'] = 1,
    ['2'] = 0,
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