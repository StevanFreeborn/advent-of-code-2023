namespace Trebuchet.Tests;

public class PartTwoPuzzleSolverTests
{
  private readonly PartTwoPuzzleSolver _sut = new();

  [Theory]
  [InlineData("two1nine", 29)]
  [InlineData("eightwothree", 83)]
  [InlineData("abcone2threexyz", 13)]
  [InlineData("xtwone3four", 24)]
  [InlineData("4nineeightseven2", 42)]
  [InlineData("zoneight234", 14)]
  [InlineData("7pqrstsixteen", 76)]
  [InlineData("99seven3vdcgvmvxtjtwodc5", 95)]
  [InlineData("gpmfhninexxgqr6", 96)]
  [InlineData("five3xjzlrmxvqznine", 59)]
  [InlineData("nine4vqxmzqxcvfhlm45", 95)]
  [InlineData("4nine7oneighthm", 48)]
  public void GetCalibrationValue_GivenAStringContainingDigits_ItShouldReturnSumOrFirstAndLastDigit(string input, int expected)
  {
    var result = _sut.GetCalibrationValue(input);
    result.Should().Be(expected);
  }

  [Theory]
  [InlineData(new string[] { "two1nine", "eightwothree", "abcone2threexyz", "xtwone3four", "4nineeightseven2", "zoneight234", "7pqrstsixteen" }, 281)]
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
    result.Should().BeLessThan(54249);
  }
}