namespace WaitForIt.Tests;

public class RaceTests
{
  [Theory]
  [InlineData(7, 9, 4)]
  [InlineData(15, 40, 8)]
  [InlineData(30, 200, 9)]
  public void CalculateNumberOfWaysToWin_WhenCalled_ItShouldReturnNumberOfWaysToWin(int raceDuration, int distanceRecord, int expected)
  {
    var result = new Race(raceDuration, distanceRecord).CalculateNumberOfWaysToWin();
    result.Should().Be(expected);
  }
}