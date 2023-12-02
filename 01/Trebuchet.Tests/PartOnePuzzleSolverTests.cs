namespace Trebuchet.Tests;

public class PartOnePuzzleSolverTests
{
  private readonly PartOnePuzzleSolver _sut = new();

  [Theory]
  [InlineData("1abc2", 12)]
  [InlineData("pqr3stu8vwx", 38)]
  [InlineData("a1b2c3d4e5f", 15)]
  [InlineData("treb7uchet", 77)]
  public void GetCalibrationValue_GivenAStringContainingDigits_ItShouldReturnSumOrFirstAndLastDigit(string input, int expected)
  {
    var result = _sut.GetCalibrationValue(input);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData(new string[] { "1abc2", "pqr3stu8vwx", "a1b2c3d4e5f", "treb7uchet" }, 142)]
  public void SumCalibrationValues_GivenAnArrayOfStrings_ItShouldReturnTheSumOfAllCalibrationValues(string[] input, int expected)
  {
    var result = _sut.SumCalibrationValues(input);
    result.Should().Be(expected);
  }

  [Fact]
  public async Task SumCalibrationValues_GivenAFile_ItShouldReturnTheSumOfAllCalibrationValues()
  {
    var input = await File.ReadAllLinesAsync("INPUT.txt");
    var result = _sut.SumCalibrationValues(input);
    result.Should().Be(53194);
  }
}