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
    .Select(Turn.Parse)
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
    .Select(Turn.Parse)
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
  public void Turn_GivenListOfTurns_ItShouldBeAbleToCalculateTotalWinnings()
  {
    TestData.TestTurns
      .Select(Turn.Parse)
      .OrderBy(t => t.Hand)
      .Select((turn, index) => turn.Bid * (index + 1))
      .Sum()
      .Should()
      .Be(6440);
  }

  public static class TestData
  {
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