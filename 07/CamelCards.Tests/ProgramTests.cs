namespace CamelCards.Tests;

public class ProgramTests
{
  [Fact]
  public async Task Main_WhenCalledWithInputFile_ItShouldReturnExpectedResult()
  {
    var result = await Program.Main(["INPUT.txt"]);
    result.Should().Be(251287184);
  }

  [Fact]
  public async Task Main_WhenCalledWithInputFileAsPart2_ItShouldReturnExpectedResult()
  {
    var result = await Program.Main(["INPUT.txt", "part2"]);
    result.Should().Be(250757288);
  }
}