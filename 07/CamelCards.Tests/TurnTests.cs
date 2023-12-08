namespace CamelCards.Tests;

public class TurnTests
{
  [Fact]
  public void Parse_WhenGivenTurnInput_ItShouldReturnExpectedTurnInstance()
  {
    var turn = Turn.Parse("32T3K 765");
    turn.Hand.Cards.Should().BeEquivalentTo(new List<Card>
    {
      new('3'),
      new('2'),
      new('T'),
      new('3'),
      new('K'),
    });

    turn.Bid.Should().Be(765);
  }

  [Fact]
  public void ToString_WhenCalled_ItShouldReturnStringRepresentationOfTurn()
  {
    var turn = "32T3K 765";
    Turn.Parse(turn).ToString().Should().Be(turn);
  }

  [Fact]
  public void OrderBy_WhenGivenListOfTurns_ItShouldBeAbleToOrderThemAccordingToHandStrength()
  {
    var turns = TestData.TestTurns
    .Select(t => Turn.Parse(t))
    .ToList();

    turns
      .OrderBy(t => t.Hand)
      .Select(t => t.ToString())
      .Should()
      .BeEquivalentTo(
        [
          "32T3K 765",
          "KTJJT 220",
          "KK677 28",
          "T55J5 684",
          "QQQJA 483",
        ]
      );
  }

  [Fact]
  public void OrderByDescending_WhenGivenListOfTurns_ItShouldBeAbleToOrderThemAccordingToHandStrength()
  {
    var turns = TestData.TestTurns
    .Select(t => Turn.Parse(t))
    .ToList();

    turns
      .OrderByDescending(t => t.Hand)
      .Select(t => t.ToString())
      .Should()
      .BeEquivalentTo(
        [
          "QQQJA 483",
          "T55J5 684",
          "KK677 28",
          "KTJJT 220",
          "32T3K 765",
        ]
      );
  }

  [Fact]
  public void OrderBy_WhenGivenListOfTurnsAndJokersAreTreatedAsWild_ItShouldBeAbleToOrderThemAccordingToHandStrength()
  {
    var turns = TestData.TestTurns
    .Select(t => Turn.Parse(t, true))
    .ToList();

    turns
      .OrderBy(t => t.Hand)
      .Select(t => t.ToString())
      .Should()
      .BeEquivalentTo(
        [
          "32T3K 765",
          "KK677 28",
          "T55J5 684",
          "QQQJA 483",
          "KTJJT 220",
        ]
      );
  }

  [Fact]
  public void Turn_GivenListOfTurns_ItShouldBeAbleToCalculateTotalWinnings()
  {
    TestData.TestTurns
      .Select(t => Turn.Parse(t))
      .OrderBy(t => t.Hand)
      .Select((turn, index) => turn.Bid * (index + 1))
      .Sum()
      .Should()
      .Be(6440);
  }

  [Fact]
  public void Turn_GivenListOfTurnsAndJokersAreWild_ItShouldBeAbleToCalculateTotalWinnings()
  {
    TestData.TestTurns
      .Select(t => Turn.Parse(t, true))
      .OrderBy(t => t.Hand)
      .Select((turn, index) => turn.Bid * (index + 1))
      .Sum()
      .Should()
      .Be(5905);
  }

  [Theory]
  [MemberData(nameof(TestData.TestTurnsWithJokersData), MemberType = typeof(TestData))]
  public void Turn_GivenListOfTurnsWithJokers_ItShouldHaveCorrectHandType(string turn, HandType expectedHandType)
  {
    Turn.Parse(turn, true).Hand.Type.Should().Be(expectedHandType);
  }

  public static class TestData
  {
    public static readonly string[] InputTurnsWithJokers = File.ReadAllLines("INPUT.txt").Where(l => l.Contains('J')).ToArray();

    public static IEnumerable<object[]> TestTurnsWithJokersData =>
      new List<object[]>
      {
        new object[]
        {
          "4446J 425",
          HandType.FourOfAKind,
        },
        new object[]
        {
          "26J93 60",
          HandType.OnePair,
        },
        new object[]
        {
          "TQ9JQ 554",
          HandType.ThreeOfAKind,
        },
        new object[]
        {
          "J373A 525",
          HandType.ThreeOfAKind,
        },
        new object[]
        {
          "44JJ4 738",
          HandType.FiveOfAKind,
        },
        new object[]
        {
          "JTK95 684",
          HandType.OnePair,
        },
        new object[]
        {
          "5J39Q 743",
          HandType.OnePair,
        },
        new object[]
        {
          "222J2 833",
          HandType.FiveOfAKind,
        },
        new object[]
        {
          "JJJ44 668",
          HandType.FiveOfAKind,
        },
        new object[]
        {
          "4JK47 317",
          HandType.ThreeOfAKind,
        },
        new object[]
        {
          "66J4J 253",
          HandType.FourOfAKind,
        }
      };

    public static readonly string[] TestTurns =
      [
        "32T3K 765",
        "T55J5 684",
        "KK677 28",
        "KTJJT 220",
        "QQQJA 483"
      ];
  }
}