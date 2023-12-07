namespace CamelCards.Tests;

public class CardTests
{
  [Theory]
  [InlineData('A', 12)]
  [InlineData('K', 11)]
  [InlineData('Q', 10)]
  [InlineData('J', 9)]
  [InlineData('T', 8)]
  [InlineData('9', 7)]
  [InlineData('8', 6)]
  [InlineData('7', 5)]
  [InlineData('6', 4)]
  [InlineData('5', 3)]
  [InlineData('4', 2)]
  [InlineData('3', 1)]
  [InlineData('2', 0)]
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