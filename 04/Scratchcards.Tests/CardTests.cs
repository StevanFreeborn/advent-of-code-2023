namespace Scratchcards.Tests;

public class CardTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseCardData), MemberType = typeof(TestData))]
  public void Parse_GivenInput_ItShouldReturnExpectedCard(string input, Card expected)
  {
    var result = Card.Parse(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.GetCardValueData), MemberType = typeof(TestData))]
  public void GetCardValue_GivenCard_ItShouldReturnExpectedValue(Card card, int expected)
  {
    var result = card.GetCardValue();
    result.Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly Dictionary<string, Card> TestCards =
      new()
      {
        {
          "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
          new Card(
            1,
            [41, 48, 83, 86, 17],
            [83, 86, 6, 31, 17, 9, 48, 53]
          )
        },
        {
          "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
          new Card(
            2,
            [13, 32, 20, 16, 61],
            [61, 30, 68, 82, 17, 32, 24, 19]
          )
        },
        {
          "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
          new Card(
            3,
            [1, 21, 53, 59, 44],
            [69, 82, 63, 72, 16, 21, 14, 1]
          )
        },
        {
          "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
          new Card(
            4,
            [41, 92, 73, 84, 69],
            [59, 84, 76, 51, 58, 5, 54, 83]
          )
        },
        {
          "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
          new Card(
            5,
            [87, 83, 26, 28, 32],
            [88, 30, 70, 12, 93, 22, 82, 36]
          )
        },
        {
          "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11",
          new Card(
            6,
            [31, 18, 13, 56, 72],
            [74, 77, 10, 23, 35, 67, 36, 11]
          )
        },
      };

    public static IEnumerable<object[]> ParseCardData =>
      TestCards
        .Select(kvp => new object[] { kvp.Key, kvp.Value })
        .ToList();

    public static IEnumerable<object[]> GetCardValueData()
    {
      for (var i = 0; i < TestCards.Count; i++)
      {
        var currentCard = TestCards.ElementAt(i);
        var expectedValue = i switch
        {
          0 => 8,
          1 => 2,
          2 => 2,
          3 => 1,
          4 => 0,
          5 => 0,
          _ => throw new Exception("Unexpected card index"),
        };

        yield return new object[]
        {
          currentCard.Value,
          expectedValue
        };
      }
    }
  }
}