namespace CamelCards.Tests;

public class CardTests
{
  [Theory]
  [InlineData('A', 13)]
  [InlineData('K', 12)]
  [InlineData('Q', 11)]
  [InlineData('J', 10)]
  [InlineData('T', 9)]
  [InlineData('9', 8)]
  [InlineData('8', 7)]
  [InlineData('7', 6)]
  [InlineData('6', 5)]
  [InlineData('5', 4)]
  [InlineData('4', 3)]
  [InlineData('3', 2)]
  [InlineData('2', 1)]
  [InlineData('W', 0)]
  public void Card_WhenGivenValidCharacter_ItShouldReturnCardWithExpectedStrength(char character, int expectedStrength)
  {
    var card = new Card(character);
    card.Strength.Should().Be(expectedStrength);
  }

  [Fact]
  public void Card_WhenGivenInvalidCardCharacter_ItShouldThrowArgumentException()
  {
    Action act = () => new Card('X');
    act.Should().Throw<ArgumentException>();
  }
}