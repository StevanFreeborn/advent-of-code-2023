namespace CubeConundrum.Tests;

public class ProgramTests
{
  [Fact]
  public async Task Main_GivenInput_ItShouldReturnExpectedSum()
  {
    var result = await Program.Main(["INPUT.txt"]);
    result.Should().Be(2879);
  }
}