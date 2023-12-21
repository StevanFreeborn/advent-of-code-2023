namespace ParabolicReflectorDish.Tests;

public class DishTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseTestData), MemberType = typeof(TestData))]
  public void Parse_GivenInput_ItShouldReturnDishInstance(string[] input, Dish expected)
  {
    var result = Dish.Parse(input);
    result.Should().BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateTotalLoadTestData), MemberType = typeof(TestData))]
  public void CalculateTotalLoad_WhenCalled_ItShouldReturnExpectedLoad(Dish dish, long expected)
  {
    var result = dish.CalculateTotalLoad();
    result.Should().Be(expected);
  }

  public static class TestData
  {
    private static readonly Dish TestDish = new(
      [
        ['O', 'O', 'O', 'O', '.', '#', '.', 'O', '.', '.'],
        ['O', 'O', '.', '.', '#', '.', '.', '.', '.', '#'],
        ['O', 'O', '.', '.', 'O', '#', '#', '.', '.', 'O'],
        ['O', '.', '.', '#', '.', 'O', 'O', '.', '.', '.'],
        ['.', '.', '.', '.', '.', '.', '.', '.', '#', '.'],
        ['.', '.', '#', '.', '.', '.', '.', '#', '.', '#'],
        ['.', '.', 'O', '.', '.', '#', '.', 'O', '.', 'O'],
        ['.', '.', 'O', '.', '.', '.', '.', '.', '.', '.'],
        ['#', '.', '.', '.', '.', '#', '#', '#', '.', '.'],
        ['#', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
      ]
    );

    public static IEnumerable<object[]> CalculateTotalLoadTestData =>
      new List<object[]>
      {
        new object[]
        {
          new Dish(
            [
              ['O', 'O', 'O', 'O', '.', '#', '.', 'O', '.', '.'],
              ['O', 'O', '.', '.', '#', '.', '.', '.', '.', '#'],
              ['O', 'O', '.', '.', 'O', '#', '#', '.', '.', 'O'],
              ['O', '.', '.', '#', '.', 'O', 'O', '.', '.', '.'],
              ['.', '.', '.', '.', '.', '.', '.', '.', '#', '.'],
              ['.', '.', '#', '.', '.', '.', '.', '#', '.', '#'],
              ['.', '.', 'O', '.', '.', '#', '.', 'O', '.', 'O'],
              ['.', '.', 'O', '.', '.', '.', '.', '.', '.', '.'],
              ['#', '.', '.', '.', '.', '#', '#', '#', '.', '.'],
              ['#', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
            ]
          ),
          136
        }
      };

    public static IEnumerable<object[]> ParseTestData =>
      new List<object[]>
      {
        new object[]
        {
          new string[]
          {
            "O....#....",
            "O.OO#....#",
            ".....##...",
            "OO.#O....O",
            ".O.....O#.",
            "O.#..O.#.#",
            "..O..#O..O",
            ".......O..",
            "#....###..",
            "#OO..#....",
          },
          new Dish(
            [
              ['O', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
              ['O', '.', 'O', 'O', '#', '.', '.', '.', '.', '#'],
              ['.', '.', '.', '.', '.', '#', '#', '.', '.', '.'],
              ['O', 'O', '.', '#', 'O', '.', '.', '.', '.', 'O'],
              ['.', 'O', '.', '.', '.', '.', '.', 'O', '#', '.'],
              ['O', '.', '#', '.', '.', 'O', '.', '#', '.', '#'],
              ['.', '.', 'O', '.', '.', '#', 'O', '.', '.', 'O'],
              ['.', '.', '.', '.', '.', '.', '.', 'O', '.', '.'],
              ['#', '.', '.', '.', '.', '#', '#', '#', '.', '.'],
              ['#', 'O', 'O', '.', '.', '#', '.', '.', '.', '.'],
            ]
          ),
        }
      };
  }
}