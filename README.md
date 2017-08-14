# SharpVG

A .NET Core library for F# to generate vector graphics in SVG format.

## Why SharpVG?

 - Allows you to emit SVG using simple F# commands so that you can create graphics and animations that are easy to distribute
 - Ability to render dynamically using [Fable](http://fable.io) to create interactive web pages
 - All basic SVG elements are supported: line, circle, ellipse, rect, text, polygon, polyline, path, image, and groups
 - No understanding of SVG is required and its as easy as using seq, list, or array
 - No external dependencies other than SharpVG are required
 - .NET Core cross platform support on Windows, Linux, and OSX
 - Limited support for SVG animations
 - Limited support for cartesian plotting
 - Reusable styles

## Example

```F#
Circle.create Point.origin (Length.ofInt 10)
  |> Circle.toString
  |> printf "%A"
```

```html
<circle r="10" cx="0" cy="0"/>
```

## Building SharpVG

Clone the repository:
```bash
git clone https://github.com/ChrisNikkel/SharpVG.git
cd SharpVG
```

Start the build:
```bash
dotnet build
```

Run the tests:
```bash
dotnet test Tests
```

Run the examples:
```bash
dotnet run -p Examples\Triangle\Triangle.fsproj
dotnet run -p Examples\Life\Life.fsproj
dotnet run -p Examples\Graph\Graph.fsproj
dotnet run -p Examples\Animate\Animate.fsproj
```

Explore interactive:
```bash
fsharpi -r:SharpVG/bin/Debug/netcoreapp2.0/SharpVG.dll
```
```F#
open SharpVG;;
Circle.create Point.origin (Length.ofInt 10) |> Circle.toString |> printf "%A";;
#quit;;
```

## Support

 - Please submit bugs and feature requests [here](https://github.com/ChrisNikkel/SharpVG/issues)

## Library License

The library is available under the MIT license. For more information see the [License file](https://github.com/ChrisNikkel/SharpVG/blob/master/LICENSE.md).

## Maintainer(s)

- [@ChrisNikkel](https://github.com/ChrisNikkel)
