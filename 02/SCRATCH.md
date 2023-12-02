# Notes

## Constraints

### Total number of cubes: 39

The elf can't show me more than 39 dice at a time.

### Total number of each cube type: 12 red, 13 green, 14 blue

The elf can't show me more than 12 red cubes at a time.
The elf can't show me more than 13 green cubes at a time.
The elf can't show me more than 14 blue cubes at a time.

### Types of cubes: red, green, blue

The elf can only show me red, green, or blue cubes.
The elf though can show me any combination of red, green, or blue cubes including none of a particular color or all of a particular color.

## Structure

Each game is a line of text.
Each game is delimited by "Game N: " where N is the game id.
Each game consists of a semi-colon delimited list of sets of cubes.
Each set of cubes is a comma delimited list of the number of cubes and the color of the cubes.

```text
Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
```

❓ How would I model this?

```csharp
class Game
{
  public int Id { get; set; }
  public List<Results> Results { get; set; }
}

class Results
{
  public List<Cube> Cubes { get; set; }
}

class Cube
{
  public int Count { get; set; }
  public CubeColor Color { get; set; }
}

enum CubeColor
{
  Red,
  Green,
  Blue
}
```

## Problems

### Problem 1

🔴 Need to determine which results are possible given the known # of cubes and known # of cubes for each color.

### Problem 2

🔴 Need to determine if all the results for a given game are possible

### Problem 3

🔴 Need to determine the sum of the game ids for all the possible games

### Problem 4

🔴 Need to determine the sum of the game ids for all the possible games
