namespace HauntedWasteland.Tests;

public class NodeTests
{
  [Theory]
  [MemberData(nameof(TestData.ParseNodeTestData), MemberType = typeof(TestData))]
  public void Parse_WhenGivenString_ItShouldReturnNode(string input, Node expected)
  {
    Node.Parse(input).Should().BeEquivalentTo(expected);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> ParseNodeTestData =>
      new List<object[]>
      {
        new object[]
        {
          "AAA = (BBB, CCC)",
          new Node("AAA", "BBB", "CCC")
        },
        new object[]
        {
          "BBB = (DDD, EEE)",
          new Node("BBB", "DDD", "EEE")
        },
        new object[]
        {
          "CCC = (ZZZ, GGG)",
          new Node("CCC", "ZZZ", "GGG")
        },
        new object[]
        {
          "DDD = (DDD, DDD)",
          new Node("DDD", "DDD", "DDD")
        },
        new object[]
        {
          "EEE = (EEE, EEE)",
          new Node("EEE", "EEE", "EEE")
        },
        new object[]
        {
          "GGG = (GGG, GGG)",
          new Node("GGG", "GGG", "GGG")
        },
        new object[]
        {
          "ZZZ = (ZZZ, ZZZ)",
          new Node("ZZZ", "ZZZ", "ZZZ")
        },
      };
  }
}