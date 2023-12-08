# Advent of Code 2023

This repository contains my solutions for the [Advent of Code 2023](https://adventofcode.com/2023) challenge.

## Language

I am using C# for this year's challenge. I'd consider myself proficient in C#, but on a day to day basis I don't use it as much and would just like to get more practice with it.

## Structure

Each day's solution is contained in a separate folder. Each folder contains:

- A `PROBLEM.md` file containing the problem description.
- A `INPUT.txt` file containing the input for the problem.
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

| Day | Problem                    |            Solution             | Status | Notes                                                                                                   |
| --- | -------------------------- | :-----------------------------: | :----: | ------------------------------------------------------------------------------------------------------- |
| 01  | [Problem](./01/PROBLEM.md) |   [Solution](./01/Trebuchet/)   |   ✅   | The trickiest part here was accounting for overlapping digit words.                                     |
| 02  | [Problem](./02/PROBLEM.md) | [Solution](./02/CubeConundrum/) |   ✅   | The key to me here was to parse the input into a useful model.                                          |
| 03  | [Problem](./03/PROBLEM.md) |  [Solution](./03/GearRatios/)   |   ✅   | The edge case that got me here was lines ending with a part number.                                     |
| 04  | [Problem](./04/PROBLEM.md) | [Solution](./04/Scratchcards/)  |   ✅   | Part 2 gets out of hand quickly with just 200 cards.                                                    |
| 05  | [Problem](./05/PROBLEM.md) |    [Solution](./05/IYGASAF/)    |   ✅   | I brute forced part 2 using parallelism. I know shame.                                                  |
| 06  | [Problem](./06/PROBLEM.md) |   [Solution](./06/WaitForIt/)   |   ✅   | Thank goodness part 2 was not like 5's part 2. 😅                                                       |
| 07  | [Problem](./07/PROBLEM.md) |  [Solution](./07/CamelCards/)   |   ✅   | What took me longest here was I missed a case when jokers are wild and there are three groups of cards. |
| 08  | [Problem](./08/PROBLEM.md) |        [Solution](./08/)        |   ⌛   |
| 09  | [Problem](./09/PROBLEM.md) |        [Solution](./09/)        |   ⌛   |
| 10  | [Problem](./10/PROBLEM.md) |        [Solution](./10/)        |   ⌛   |
| 11  | [Problem](./11/PROBLEM.md) |        [Solution](./11/)        |   ⌛   |
| 12  | [Problem](./12/PROBLEM.md) |        [Solution](./12/)        |   ⌛   |
| 13  | [Problem](./13/PROBLEM.md) |        [Solution](./13/)        |   ⌛   |
| 14  | [Problem](./14/PROBLEM.md) |        [Solution](./14/)        |   ⌛   |
| 15  | [Problem](./15/PROBLEM.md) |        [Solution](./15/)        |   ⌛   |
| 16  | [Problem](./16/PROBLEM.md) |        [Solution](./16/)        |   ⌛   |
| 17  | [Problem](./17/PROBLEM.md) |        [Solution](./17/)        |   ⌛   |
| 18  | [Problem](./18/PROBLEM.md) |        [Solution](./18/)        |   ⌛   |
| 19  | [Problem](./19/PROBLEM.md) |        [Solution](./19/)        |   ⌛   |
| 20  | [Problem](./20/PROBLEM.md) |        [Solution](./20/)        |   ⌛   |
| 21  | [Problem](./21/PROBLEM.md) |        [Solution](./21/)        |   ⌛   |
| 22  | [Problem](./22/PROBLEM.md) |        [Solution](./22/)        |   ⌛   |
| 23  | [Problem](./23/PROBLEM.md) |        [Solution](./23/)        |   ⌛   |
| 24  | [Problem](./24/PROBLEM.md) |        [Solution](./24/)        |   ⌛   |
| 25  | [Problem](./25/PROBLEM.md) |        [Solution](./25/)        |   ⌛   |
