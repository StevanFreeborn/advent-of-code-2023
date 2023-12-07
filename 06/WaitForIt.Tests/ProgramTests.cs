namespace WaitForIt.Tests;

public class ProgramTests
{
  public void Main_WhenGivenInput_ItShouldReturnTheProductOfTheMarginOfErrors()
  {
    var result = Program.Main(["INPUT.txt"]);
    result.Should().Be(1710720);
  }

  public void Main_WhenGivenInputAndPart2_ItShouldReturnTheNumberOfWaysToWin()
  {
    var result = Program.Main(["INPUT.txt", "part2"]);
    result.Should().Be(35349468);
  }
}