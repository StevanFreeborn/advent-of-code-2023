# Advent of Code 2023

This repository contains my solutions for the [Advent of Code 2023](https://adventofcode.com/2023) challenge.

## Language

I am using C# for this year's challenge. I'd consider myself proficient in C#, but on a day to day basis I don't use it as much and would just like to get more practice with it.

## Structure

Each day's solution is contained in a separate folder. Each folder contains:

- A dotnet console application containing the solution.
- An xUnit test project containing unit tests for the solution.

### Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

### Running the Solutions

To run the solutions, navigate to the solution folder and run the following command:

```bash
dotnet run -- <path-to-input-file>
```

Or for part 2 solutions

```bash
dotnet run -- <path-to-input-file> part2
```

You can also build the projects and run the executables directly.

```bash
dotnet build
./bin/Debug/net8.0/<project-name> <path-to-input-file>
```

## Challenges

| Day | Problem                                         |                 Solution                 | Status | Notes                                                                                                                                            |
|-----|-------------------------------------------------|:----------------------------------------:|:------:|--------------------------------------------------------------------------------------------------------------------------------------------------|
| 01  | [Problem](https://adventofcode.com/2023/day/1)  |       [Solution](./01/Trebuchet/)        |   ✅    | The trickiest part here was accounting for overlapping digit words.                                                                              |
| 02  | [Problem](https://adventofcode.com/2023/day/2)  |     [Solution](./02/CubeConundrum/)      |   ✅    | The key to me here was to parse the input into a useful model.                                                                                   |
| 03  | [Problem](https://adventofcode.com/2023/day/3)  |       [Solution](./03/GearRatios/)       |   ✅    | The edge case that got me here was lines ending with a part number.                                                                              |
| 04  | [Problem](https://adventofcode.com/2023/day/4)  |      [Solution](./04/Scratchcards/)      |   ✅    | Part 2 gets out of hand quickly with just 200 cards.                                                                                             |
| 05  | [Problem](https://adventofcode.com/2023/day/5)  |        [Solution](./05/IYGASAF/)         |   ✅    | I brute forced part 2 using parallelism. I know shame.                                                                                           |
| 06  | [Problem](https://adventofcode.com/2023/day/6)  |       [Solution](./06/WaitForIt/)        |   ✅    | Thank goodness part 2 was not like 5's part 2. 😅                                                                                                |
| 07  | [Problem](https://adventofcode.com/2023/day/7)  |       [Solution](./07/CamelCards/)       |   ✅    | What took me longest here was I missed a case when jokers are wild and there are three groups of cards.                                          |
| 08  | [Problem](https://adventofcode.com/2023/day/8)  |    [Solution](./08/HauntedWasteland/)    |   ✅    | Got to implement a least common multiple algorithm for part 2.                                                                                   |
| 09  | [Problem](https://adventofcode.com/2023/day/9)  |   [Solution](./09/MirageMaintenance/)    |   ✅    | Part 1 took me unnecessarily long. The bug was summing a line to check for all zeros is bad idea when negative numbers are involved.             |
| 10  | [Problem](https://adventofcode.com/2023/day/10) |        [Solution](./10/PipeMaze/)        |   ✅    | So...my solutions are rather slow, but they work. Part 2 appears to have a mathematical solution, but scanning is what I came up with on my own. |
| 11  | [Problem](https://adventofcode.com/2023/day/11) |    [Solution](./11/CosmicExpansion/)     |   ✅    | Expanding the universe was fine in part 1, but part 2 showed I actually needed to calculate the expansion instead of expanding the input.        |
| 12  | [Problem](https://adventofcode.com/2023/day/12) |       [Solution](./12/HotSprings/)       |   ✅    | This one stretched my skills. I needed lots of help from the interwebz.                                                                          |
| 13  | [Problem](https://adventofcode.com/2023/day/13) |    [Solution](./13/PointOfIncidence/)    |   ✅    | This solution is the one I think I'm most proud of so far.                                                                                       |
| 14  | [Problem](https://adventofcode.com/2023/day/14) | [Solution](./14/ParabolicReflectorDish/) |   ✅    | Cycles and a cache...that's all I'm going to say.                                                                                                |
| 15  | [Problem](https://adventofcode.com/2023/day/15) |            [Solution](./15/)             |   ⌛    |
| 16  | [Problem](https://adventofcode.com/2023/day/16) |            [Solution](./16/)             |   ⌛    |
| 17  | [Problem](https://adventofcode.com/2023/day/17) |            [Solution](./17/)             |   ⌛    |
| 18  | [Problem](https://adventofcode.com/2023/day/18) |            [Solution](./18/)             |   ⌛    |
| 19  | [Problem](https://adventofcode.com/2023/day/19) |            [Solution](./19/)             |   ⌛    |
| 20  | [Problem](https://adventofcode.com/2023/day/20) |            [Solution](./20/)             |   ⌛    |
| 21  | [Problem](https://adventofcode.com/2023/day/21) |            [Solution](./21/)             |   ⌛    |
| 22  | [Problem](https://adventofcode.com/2023/day/22) |            [Solution](./22/)             |   ⌛    |
| 23  | [Problem](https://adventofcode.com/2023/day/23) |            [Solution](./23/)             |   ⌛    |
| 24  | [Problem](https://adventofcode.com/2023/day/24) |            [Solution](./24/)             |   ⌛    |
| 25  | [Problem](https://adventofcode.com/2023/day/25) |            [Solution](./25/)             |   ⌛    |
