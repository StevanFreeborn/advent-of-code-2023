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

  [Theory]
  [MemberData(nameof(TestData.CalculateTotalLoadWithSpinCycleData), MemberType = typeof(TestData))]
  public void CalculateTotalLoad_WhenCalledWithSpinCycle_ItShouldReturnExpectedLoad(Dish dish, long expected)
  {
    var result = dish.CalculateTotalLoad(true);
    result.Should().Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.TiltTestData), MemberType = typeof(TestData))]
  public void TiltDish_WhenCalled_ItShouldReturnExpectedDish(Dish dish, int times, Dish expected)
  {
    var result = dish.TiltDish(times);
    result.Should().BeEquivalentTo(expected);
  }

  [Fact]
  public void CalculateTotalLoad_WhenCalledWithExample_ItShouldReturnExpectedLoad()
  {
    var input = File.ReadAllLines("EXAMPLE.txt");
    var dish = Dish.Parse(input);
    var result = dish.CalculateTotalLoad();
    result.Should().Be(136);
  }

  [Fact]
  public void CalculateTotalLoad_WhenCalledWithInput_ItShouldReturnExpectedLoad()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var dish = Dish.Parse(input);
    var result = dish.CalculateTotalLoad();
    result.Should().Be(106997);
  }

  [Fact]
  public void CalculateTotalLoad_WhenCalledWithExampleAndSpinCycle_ItShouldReturnExpectedLoad()
  {
    var input = File.ReadAllLines("EXAMPLE.txt");
    var dish = Dish.Parse(input);
    var result = dish.CalculateTotalLoad(true);
    result.Should().Be(64);
  }

  [Fact]
  public void CalculateTotalLoad_WhenCalledWithInputAndSpinCycle_ItShouldReturnExpectedLoad()
  {
    var input = File.ReadAllLines("INPUT.txt");
    var dish = Dish.Parse(input);
    var result = dish.CalculateTotalLoad(true);
    result.Should().Be(99641);
  }

  public static class TestData
  {
    private static readonly Dish TestDish = new(
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
    );

    public static IEnumerable<object[]> TiltTestData =>
      new List<object[]>
      {
        new object[]
        {
          TestDish,
          1,
          new Dish(
            [
              ['.', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '#', '.', '.', '.', 'O', '#'],
              ['.', '.', '.', 'O', 'O', '#', '#', '.', '.', '.'],
              ['.', 'O', 'O', '#', '.', '.', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '.', 'O', 'O', 'O', '#', '.'],
              ['.', 'O', '#', '.', '.', '.', 'O', '#', '.', '#'],
              ['.', '.', '.', '.', 'O', '#', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '.', '.', 'O', 'O', 'O', 'O'],
              ['#', '.', '.', '.', 'O', '#', '#', '#', '.', '.'],
              ['#', '.', '.', 'O', 'O', '#', '.', '.', '.', '.'],
            ]
          )
        },
        new object[]
        {
          TestDish,
          2,
          new Dish(
            [
              ['.', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '#', '.', '.', '.', 'O', '#'],
              ['.', '.', '.', '.', '.', '#', '#', '.', '.', '.'],
              ['.', '.', 'O', '#', '.', '.', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '.', 'O', 'O', 'O', '#', '.'],
              ['.', 'O', '#', '.', '.', '.', 'O', '#', '.', '#'],
              ['.', '.', '.', '.', 'O', '#', '.', '.', '.', 'O'],
              ['.', '.', '.', '.', '.', '.', '.', 'O', 'O', 'O'],
              ['#', '.', '.', 'O', 'O', '#', '#', '#', '.', '.'],
              ['#', '.', 'O', 'O', 'O', '#', '.', '.', '.', 'O'],
            ]
          ),
        },
        new object[]
        {
          TestDish,
          3,
          new Dish(
            [
              ['.', '.', '.', '.', '.', '#', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '#', '.', '.', '.', 'O', '#'],
              ['.', '.', '.', '.', '.', '#', '#', '.', '.', '.'],
              ['.', '.', 'O', '#', '.', '.', '.', '.', '.', '.'],
              ['.', '.', '.', '.', '.', 'O', 'O', 'O', '#', '.'],
              ['.', 'O', '#', '.', '.', '.', 'O', '#', '.', '#'],
              ['.', '.', '.', '.', 'O', '#', '.', '.', '.', 'O'],
              ['.', '.', '.', '.', '.', '.', '.', 'O', 'O', 'O'],
              ['#', '.', '.', '.', 'O', '#', '#', '#', '.', 'O'],
              ['#', '.', 'O', 'O', 'O', '#', '.', '.', '.', 'O'],
            ]
          ),
        }
      };

    public static IEnumerable<object[]> CalculateTotalLoadWithSpinCycleData =>
      new List<object[]>
      {
        new object[]
        {
          TestDish,
          64,
        }
      };

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