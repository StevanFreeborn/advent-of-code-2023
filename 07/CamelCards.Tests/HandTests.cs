namespace CamelCards.Tests;

public class HandTests
{
  [Theory]
  [MemberData(nameof(TestData.HandTypeTestData), MemberType = typeof(TestData))]
  public void Type_WhenGivenListOfCards_ItShouldReturnExpectedHandType(List<Card> cards, HandType expectedHandType)
  {
    var hand = new Hand(cards);
    hand.Type.Should().Be(expectedHandType);
  }

  [Theory]
  [MemberData(nameof(TestData.CompareToTestData), MemberType = typeof(TestData))]
  public void CompareTo_WhenGivenHandWithHigherType_ItShouldReturnNegative1(Hand hand, Hand otherHand, int sortResult)
  {
    hand.CompareTo(otherHand).Should().Be(sortResult);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> CompareToTestData =>
      new List<object[]>
      {
        new object[]
        {
          new Hand(
            [
              new('A'),
              new('A'),
              new('A'),
              new('A'),
              new('A'),
            ]
          ),
          new Hand(
            [
              new('A'),
              new('A'),
              new('8'),
              new('A'),
              new('A'),
            ]
          ),
          1,
        },
        new object[]
        {
          new Hand(
            [
              new('A'),
              new('A'),
              new('8'),
              new('A'),
              new('A'),
            ]
          ),
          new Hand(
            [
              new('A'),
              new('A'),
              new('A'),
              new('A'),
              new('A'),
            ]
          ),
          -1,
        },
        new object[]
        {
          new Hand(
            [
              new('A'),
              new('A'),
              new('A'),
              new('A'),
              new('A'),
            ]
          ),
          new Hand(
            [
              new('A'),
              new('A'),
              new('A'),
              new('A'),
              new('A'),
            ]
          ),
          0,
        },
        new object[]
        {
          new Hand(
            [
              new('3'),
              new('3'),
              new('3'),
              new('3'),
              new('2'),
            ]
          ),
          new Hand(
            [
              new('2'),
              new('A'),
              new('A'),
              new('A'),
              new('A'),
            ]
          ),
          1,
        }
      };

    public static IEnumerable<object[]> HandTypeTestData =>
      new List<object[]>
      {
        new object[]
        {
          new List<Card>
          {
            new('A'),
            new('A'),
            new('A'),
            new('A'),
            new('A'),
          },
          HandType.FiveOfAKind,
        },
        new object[]
        {
          new List<Card>
          {
            new('A'),
            new('A'),
            new('8'),
            new('A'),
            new('A'),
          },
          HandType.FourOfAKind,
        },
        new object[]
        {
          new List<Card>
          {
            new('2'),
            new('3'),
            new('3'),
            new('3'),
            new('2'),
          },
          HandType.FullHouse,
        },
        new object[]
        {
          new List<Card>
          {
            new('T'),
            new('T'),
            new('T'),
            new('9'),
            new('8'),
          },
          HandType.ThreeOfAKind,
        },
        new object[]
        {
          new List<Card>
          {
            new('2'),
            new('3'),
            new('4'),
            new('3'),
            new('2'),
          },
          HandType.TwoPair,
        },
        new object[]
        {
          new List<Card>
          {
            new('A'),
            new('2'),
            new('3'),
            new('A'),
            new('4'),
          },
          HandType.OnePair,
        },
        new object[]
        {
          new List<Card>
          {
            new('2'),
            new('3'),
            new('4'),
            new('5'),
            new('6'),
          },
          HandType.HighCard,
        },
      };
  }
}