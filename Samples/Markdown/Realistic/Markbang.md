# Markbang

Markbang is a high-performance and lightweight solution to object-orient the Markdown markup language in .NET (written in C#).
It was created to help with the [GBX.NET wiki class reference](https://github.com/BigBang1112/gbx-net/wiki) generation,
while also making it possible to add manual information that wouldn't be affected by the automation.

## Compatibility

Modern LTS .NET Core projects, due to heavy usage of `Span`:
- .NET 6
- .NET Standard 2.1

.NET Framework is not planned.

## Goals

Two main priorities are: 
- Understanding flaws while reading
- Proper formatting while writing

Of course, the performance (speed vs memory allocation) is also an important goal - while keeping the library small (currently ~30kB).

## Features

Currently supports:
- Paragraph
- Heading (all levels, even above 6)
- Horizontal rule
- List (ordered and unordered)
- Table
- Code block

Planned features:
- Blockquote
- Inline elements, such as font style, links, or images
  - These are currently presented as Paragraph block
- Better code readability + documentation
- More testing
- Async stuff
- Conversion to HTML (somewhere in the distance)

## Technical details

- Interfaces are preferred over abstract classes.
- Immutability is preferred where there's no list involved inside the class.
- `Span<char>` should be always used in places where you need to store a temporary string from the line being read.
- Writing should be done *per line*, using the `TextWriter.WriteLine(char[])`.
- Allocate only one `char[]` per written line, or if possible, use an already allocated string as a parameter of `TextWriter.WriteLine(string)`.
- New line should be always defined as `Environment.NewLine`.
- Library should have only managed code - no usage of `unsafe`, but depends where the future will go.

## Benchmarks

Benchmarks were done using a `StringReader` and `StringWriter`, to reduce the IO randomness.

```
BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1526 (21H1/May2021Update)
AMD Ryzen 7 3700X, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.102
  [Host]        : .NET 6.0.2 (6.0.222.6406), X64 RyuJIT  [AttachedDebugger]
  .NET 5.0      : .NET 5.0.14 (5.0.1422.5710), X64 RyuJIT
  .NET Core 3.1 : .NET Core 3.1.22 (CoreCLR 4.700.21.56803, CoreFX 4.700.21.57101), X64 RyuJIT
```


|                     Method |           Job |       Runtime |      Mean |    Error |   StdDev |   Gen 0 |  Gen 1 | Allocated |
|--------------------------- |-------------- |-------------- |----------:|---------:|---------:|--------:|-------:|----------:|
|            Parse_GBXNET_Md |      .NET 5.0 |      .NET 5.0 |  45.49 us | 0.855 us | 0.878 us | 10.0708 | 2.2583 |     82 KB |
|             Save_GBXNET_Md |      .NET 5.0 |      .NET 5.0 |  26.45 us | 0.522 us | 0.901 us |  6.7749 | 0.7324 |     56 KB |
| Parse_CGameCtnChallenge_Md |      .NET 5.0 |      .NET 5.0 | 141.63 us | 1.659 us | 1.552 us | 22.7051 | 8.0566 |    186 KB |
|  Save_CGameCtnChallenge_Md |      .NET 5.0 |      .NET 5.0 |  71.02 us | 1.348 us | 1.552 us | 13.7939 | 3.4180 |    114 KB |
|            Parse_GBXNET_Md | .NET Core 3.1 | .NET Core 3.1 |  48.46 us | 0.943 us | 0.968 us | 10.0708 | 2.2583 |     82 KB |
|             Save_GBXNET_Md | .NET Core 3.1 | .NET Core 3.1 |  25.31 us | 0.309 us | 0.289 us |  6.7749 | 0.7324 |     56 KB |
| Parse_CGameCtnChallenge_Md | .NET Core 3.1 | .NET Core 3.1 | 145.52 us | 1.558 us | 1.457 us | 22.7051 | 8.0566 |    186 KB |
|  Save_CGameCtnChallenge_Md | .NET Core 3.1 | .NET Core 3.1 |  76.44 us | 0.817 us | 0.724 us | 13.7939 | 3.2959 |    114 KB |

Tables are the slowest part of reading, as each cell is being allocated as its own string. It is still questionable if to change this or not.

## Usage

### To parse a markdown:

- `Markdown.Parse(string fileName)`
- `Markdown.Parse(Stream stream)`
- `Markdown.Parse(TextReader reader)` - can be `StreamReader`, `StringReader`, or any custom one
- `Markdown.ParseText(string text)`

Code:

```cs
using Markbang;

var md = Markdown.Parse("Markdown1.md");
```

```cs
using Markbang;

var markdown2AsString = "# Test markdown" + Environment.NewLine +
Environment.NewLine +
"This is a test paragraph" + Environment.NewLine +
"1. one" + Environment.NewLine +
"2. two" + Environment.NewLine +
"3. three";

var md = Markdown.ParseText(markdown2AsString);
```

Or any case where you have a `Stream` object! (I can't think of any simple one now)

### To save a markdown:

- `md.Save(string fileName)`
- `md.Save(Stream stream)`
- `md.Save(TextWriter writer)` - can be `StreamWriter`, `StringWriter`, or any custom one

Code:

```cs
using Markbang;

var md = Markdown.Parse("Markdown1.md");

using var writer = new StreamWriter("Markdown1New.md");

md.Save(writer);
```

## License

Forever MIT.

## Special thanks

Goes to Markdown creators for such a simple and readable documentation format!