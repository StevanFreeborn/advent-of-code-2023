namespace MirageMaintenance.Tests;

public class ValueHistoryTests
{
  [Theory]
  [MemberData(nameof(TestData.CalculateDifferencesTestData), MemberType = typeof(TestData))]
  public void CalculateDifferences_GivenAListOfValues_ItShouldReturnAListOfDifferences(List<long> values, List<long> expected)
  {
    new ValueHistory([])
      .CalculateDifferences(values)
      .Should()
      .BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateHistoryTestData), MemberType = typeof(TestData))]
  public void CalculateHistory_GivenAListOfValues_ItShouldReturnAListOfHistoricalValues(List<long> values, List<List<long>> expected)
  {
    new ValueHistory(values)
      .CalculateHistory()
      .Should()
      .BeEquivalentTo(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateNextValueTestData), MemberType = typeof(TestData))]
  public void CalculateNextValue_GivenAListOfValues_ItShouldReturnTheNextValue(List<long> values, long expected)
  {
    new ValueHistory(values)
      .CalculateNextValue()
      .Should()
      .Be(expected);
  }

  [Theory]
  [MemberData(nameof(TestData.CalculateNextValueTestDataBackwards), MemberType = typeof(TestData))]
  public void CalculateNextValue_GivenAListOfValuesAndBackwardsIsTrue_ItShouldReturnTheNextValue(List<long> values, long expected)
  {
    new ValueHistory(values)
      .CalculateNextValue(true)
      .Should()
      .Be(expected);
  }

  public static class TestData
  {
    public static IEnumerable<object[]> CalculateNextValueTestDataBackwards =>
      new List<object[]>
      {
        new object[]
        {
          new List<long> { 10, 13, 16, 21, 30, 45 },
          5,
        }
      };

    public static IEnumerable<object[]> CalculateNextValueTestData =>
      new List<object[]>
      {
        new object[]
        {
          new List<long> { 0, 3, 6, 9, 12, 15 },
          18,
        },
        new object[]
        {
          new List<long> { 1, 3, 6, 10, 15, 21 },
          28,
        },
        new object[]
        {
          new List<long> { 10, 13, 16, 21, 30, 45 },
          68,
        },
        new object[]
        {
          new List<long> { -7, 6, 44, 130, 309, 672, 1404, 2864, 5712, 11113, 21082, 39112, 71386, 129156, 233312, 422773, 769075, 1400300, 2538072, 4551386, 8029995 },
          13877188,
        },
        new object[]
        {
          new List<long> { 5, 13, 45, 115, 234, 420, 731, 1329, 2591, 5314, 11131, 23388, 48974, 102035, 211293, 434077, 882526, 1771333, 3502944, 6817585, 13056927 },
          24632652
        },
        new object[]
        {
          new List<long> { 12, 34, 63, 97, 134, 172, 209, 243, 272, 294, 307, 309, 298, 272, 229, 167, 84, -22, -153, -311, -498 },
          -716,
        },
        new object[]
        {
          new List<long> { 0, 8, 26, 57, 105, 173, 258, 346, 429, 605, 1397, 4561, 14876, 43758, 116059, 282152, 638422, 1360646, 2756524, 5345897, 9980043 },
          18014971,
        },
        new object[]
        {
          new List<long> { 8, 29, 72, 144, 248, 390, 605, 1026, 2039, 4596, 10807, 25025, 55832, 119755, 248445, 502919, 1002134, 1980015, 3897262, 7655070, 14992036 },
          29199652,
        },
        new object[]
        {
          new List<long> { 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0, -1, -2, -3, -4, -5, -6, -7 },
          -8,
        },
        new object[]
        {
          new List<long> { -6, -13, -22, -33, -46, -61, -78, -97, -118, -141, -166, -193, -222, -253, -286, -321, -358, -397, -438, -481, -526 },
          -573
        },
        new object[]
        {
          new List<long> { 9, 27, 56, 112, 226, 457, 930, 1921, 4030, 8514, 17905, 37139, 75621, 151030, 296349, 572758, 1092879, 2061701, 3846702, 7094666, 12920996 },
          23208575,
        },
        new object[]
        {
          new List<long> { -3, -1, 14, 61, 171, 389, 774, 1400, 2372, 3888, 6401, 10974, 20014, 38812, 78893, 165423, 353359, 761461, 1640856, 3507131, 7384045 },
          15235639,
        },
        new object[]
        {
          new List<long> { 11, 29, 63, 133, 274, 553, 1096, 2121, 3987, 7294, 13115, 23530, 42796, 79765, 152604, 297574, 584852, 1146848, 2229936, 4292852, 8201987 },
          15629938,
        },
        new object[]
        {
          new List<long> { 4, 16, 45, 98, 187, 334, 576, 970, 1598, 2572, 4039, 6186, 9245, 13498, 19282, 26994, 37096, 50120, 66673, 87442, 113199 },
          144806,
        },
        new object[]
        {
          new List<long> { 8, 20, 57, 137, 294, 600, 1198, 2360, 4606, 8946, 17345, 33579, 64776, 124157, 235847, 443145, 822344, 1505117, 2713796, 4816071, 8409952 },
          14458829,
        },
        new object[]
        {
          new List<long> { 13, 16, 31, 72, 171, 391, 853, 1788, 3635, 7233, 14219, 27894, 55158, 110850, 227371, 475612, 1009429, 2157809, 4610859, 9784975, 20520225 },
          42376168
        }
      };

    public static IEnumerable<object[]> CalculateHistoryTestData =>
      new List<object[]>
      {
          new object[]
          {
            new List<long> { 0, 3, 6, 9, 12, 15 },
            new List<List<long>>
            {
              new() { 3, 3, 3, 3, 3 },
              new() { 0, 0, 0, 0 },
            },
          },
          new object[]
          {
            new List<long> { 1, 3, 6, 10, 15, 21 },
            new List<List<long>>
            {
              new() { 2, 3, 4, 5, 6 },
              new() { 1, 1, 1, 1 },
              new() { 0, 0, 0 },
            },
          },
          new object[]
          {
            new List<long> { 10, 13, 16, 21, 30, 45 },
            new List<List<long>>
            {
              new() { 3, 3, 5, 9, 15 },
              new() { 0, 2, 4, 6 },
              new() { 2, 2, 2 },
              new() { 0, 0 },
            },
          },
          new object[]
          {
            new List<long> { -7, 6, 44, 130, 309, 672, 1404, 2864, 5712, 11113, 21082, 39112, 71386, 129156, 233312, 422773, 769075, 1400300, 2538072, 4551386, 8029995 },
            new List<List<long>>
            {
              new() { 13, 38, 86, 179, 363, 732, 1460, 2848, 5401, 9969, 18030, 32274, 57770, 104156, 189461, 346302, 631225, 1137772, 2013314, 3478609 },
              new() { 25, 48, 93, 184, 369, 728, 1388, 2553, 4568, 8061, 14244, 25496, 46386, 85305, 156841, 284923, 506547, 875542, 1465295 },
              new() { 23, 45, 91, 185, 359, 660, 1165, 2015, 3493, 6183, 11252, 20890, 38919, 71536, 128082, 221624, 368995, 589753 },
              new() { 22, 46, 94, 174, 301, 505, 850, 1478, 2690, 5069, 9638, 18029, 32617, 56546, 93542, 147371, 220758 },
              new() { 24, 48, 80, 127, 204, 345, 628, 1212, 2379, 4569, 8391, 14588, 23929, 36996, 53829, 73387 },
              new() { 24, 32, 47, 77, 141, 283, 584, 1167, 2190, 3822, 6197, 9341, 13067, 16833, 19558 },
              new() { 8, 15, 30, 64, 142, 301, 583, 1023, 1632, 2375, 3144, 3726, 3766, 2725 },
              new() { 7, 15, 34, 78, 159, 282, 440, 609, 743, 769, 582, 40, -1041 },
              new() { 8, 19, 44, 81, 123, 158, 169, 134, 26, -187, -542, -1081 },
              new() { 11, 25, 37, 42, 35, 11, -35, -108, -213, -355, -539 },
              new() { 14, 12, 5, -7, -24, -46, -73, -105, -142, -184 },
              new() { -2, -7, -12, -17, -22, -27, -32, -37, -42 },
              new() { -5, -5, -5, -5, -5, -5, -5, -5 },
              new() { 0, 0, 0, 0, 0, 0, 0 },
            }
          },
          new object[]
          {
            new List<long> { 8, 20, 57, 137, 294, 600, 1198, 2360, 4606, 8946, 17345, 33579, 64776, 124157, 235847, 443145, 822344, 1505117, 2713796, 4816071, 8409952 },
            new List<List<long>>
            {
              new() { 12, 37, 80, 157, 306, 598, 1162, 2246, 4340, 8399, 16234, 31197, 59381, 111690, 207298, 379199, 682773, 1208679, 2102275, 3593881 },
              new() { 25, 43, 77, 149, 292, 564, 1084, 2094, 4059, 7835, 14963, 28184, 52309, 95608, 171901, 303574, 525906, 893596, 1491606 },
              new() { 18, 34, 72, 143, 272, 520, 1010, 1965, 3776, 7128, 13221, 24125, 43299, 76293, 131673, 222332, 367690, 598010 },
              new() { 16, 38, 71, 129, 248, 490, 955, 1811, 3352, 6093, 10904, 19174, 32994, 55380, 90659, 145358, 230320 },
              new() { 22, 33, 58, 119, 242, 465, 856, 1541, 2741, 4811, 8270, 13820, 22386, 35279, 54699, 84962 },
              new() { 11, 25, 61, 123, 223, 391, 685, 1200, 2070, 3459, 5550, 8566, 12893, 19420, 30263 },
              new() { 14, 36, 62, 100, 168, 294, 515, 870, 1389, 2091, 3016, 4327, 6527, 10843 },
              new() { 22, 26, 38, 68, 126, 221, 355, 519, 702, 925, 1311, 2200, 4316 },
              new() { 4, 12, 30, 58, 95, 134, 164, 183, 223, 386, 889, 2116 },
              new() { 8, 18, 28, 37, 39, 30, 19, 40, 163, 503, 1227 },
              new() { 10, 10, 9, 2, -9, -11, 21, 123, 340, 724 },
              new() { 0, -1, -7, -11, -2, 32, 102, 217, 384 },
              new() { -1, -6, -4, 9, 34, 70, 115, 167 },
              new() { -5, 2, 13, 25, 36, 45, 52 },
              new() { 7, 11, 12, 11, 9, 7 },
              new() { 4, 1, -1, -2, -2 },
              new() { -3, -2, -1, 0 },
              new() { 1, 1, 1 },
              new() { 0, 0 },
            }
          }
      };

    public static IEnumerable<object[]> CalculateDifferencesTestData =>
      new List<object[]>
      {
          new object[]
          {
            new List<long> { 0, 3, 6, 9, 12, 15 },
            new List<long> { 3, 3, 3, 3, 3 },
          },
          new object[]
          {
            new List<long> { 1, 3, 6, 10, 15, 21 },
            new List<long> { 2, 3, 4, 5, 6 },
          },
          new object[]
          {
            new List<long> { 10, 13, 16, 21, 30, 45 },
            new List<long> { 3, 3, 5, 9, 15 },
          },
          new object[]
          {
            new List<long> { 10, 13, 16 },
            new List<long> { 3, 3 },
          }
      };
  }
}