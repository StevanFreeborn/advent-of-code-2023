# Notes

## Contraints

- Periods don't count as special characters
- All lines have same length
- If not a number or period then it is a special character

Each line parsed need to collect all numbers.

Can't know whether a collected number is a part number until we
have also parsed the proceeding or following line.

```text
467..114..
...*......
..35..633.
......#...
617*......
```

After parsing first line we would have `467` with start index 0 and end index 2 and `114` with start index 5 and end index 7.

For 467 to be a part number it needs to have a special character at index 3 or it needs to have a special character at index 1, 2, 3, 4 on the next line.

For 114 to be a part number it needs to have a special character at index 4 or index 8 or it needs to have a special character at index 4, 5, 6, 7 on the next line.

Need to keep track of:

- Previous line - will start as empty
- Current line
- Next line

So you look at the current line and you parse all the numbers with their start and end index. You also parse all special characters with their index.

Then you end up with something like this: currentLine

```json
{
  "numbers": [
    "467": {
      "start": 0,
      "end": 2
    },
    "114": {
      "start": 5,
      "end": 7
    }
  ],
  "specialCharacters": []
}
```

```csharp
var currentLineString = "467..114..";
var currentLine = new Line(currentLineString);

class Line
{
  public List<Dictionary<int, Indexes>> Numbers { get; set; }
  public List<Dictionary<int, Indexes>> SpecialCharacters { get; set; }

  public Line(string line)
  {
    // parse line
  }
}

public class Indexes
{
  public int Start { get; set; }
  public int End { get; set; }

  public Indexes(int start, int length)
  {
    Start = start;
    End = start + length;
  }
}
```

Then you see if those numbers are part numbers just by checking if a special character proceeds or follows their start and end indexes.

```csharp

```

If not then you need to see if there is a previous line and whether there is a special character at on the previous line that has an index that is between 1 less than the start index and 1 more than the end index.

If not then you need to see if there is a next line and whether there is a special character at on the next line that has an index that is between 1 less than the start index and 1 more than the end index.

Basic approach:

- Parse the current, previous, and next lines into a model that keeps track of the numbers and special characters and their indexes for that line.
- Then each line should be able to evaluate what is a part number or not. Or what is a gear or not given its previous and next lines.
