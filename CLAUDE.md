# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

```bash
# Build
dotnet build
dotnet build --configuration Release

# Test
dotnet test Tests
dotnet test Tests/Tests.fsproj --configuration Release --no-build

# Test with coverage (output in TestResults/)
dotnet test Tests --collect:"XPlat Code Coverage" --results-directory ./TestResults

# Run examples
dotnet run -p Examples/Triangle/Triangle.fsproj
dotnet run -p Examples/Life/Life.fsproj
```

## Architecture

SharpVG is an F# library (netstandard2.1 + net7/8/9) that produces SVG output. The compilation order in `SharpVG/SharpVG.fsproj` reflects the dependency layers:

**Primitives** (no dependencies on other SharpVG modules):
`Attribute` → `Tag` → `ElementId`, `Length`, `Point`, `Area`, `Color`

**Shapes** (depend on primitives): `Path`, `Polygon`, `Polyline`, `Image`, `Circle`, `Ellipse`, `Line`, `Rect`, `Text`, `Script`

**Styling/behavior**: `Pen`, `Style`, `Timing`, `Transform`, `Animation`, `FilterEffect`, `Filter`, `ViewBox`

**Composition layer**: `Element`, `Anchor`, `Symbol`, `Use`, `Group`, `SvgDefinitions`, `Graphic`, `Svg`

### Key rendering pattern

Shapes implement `static member ToTag: ^T -> Tag`. `Element.create` uses an F# SRTP constraint to call `ToTag` on any shape, wrapping it into an `Element` record. `Element.ToFullTag` then injects style attributes, transforms, and animations as children/attributes before final `Tag.toString` renders the SVG string.

`Graphic` is a DU that unifies all shapes so they can be stored in heterogeneous collections and converted to `Element` uniformly.

### Typical usage pipeline

```
Shape.create → Element.createWithStyle style → Svg.ofElement → Svg.toHtml "Title"
```

`Svg` is the top-level renderer; it wraps elements/groups into `<svg>` tags or full HTML pages.

### Tests

Tests live in `Tests/` and use xUnit + FsCheck (property-based testing). `BasicChecks.fs` provides shared helpers (`checkBodylessTag`, `checkTag`) that verify structural correctness of SVG output. Individual test files follow the pattern `Test<Module>.fs`.

## Code standards

See [`Documentation/standards.md`](Documentation/standards.md) for coding conventions, example style, and test standards.

When editing or adding documentation in `Documentation/`, follow **Documentation/standards.md**: use named `let` bindings for all points, areas, lengths, times, colors, and pens; use the standard names (center, radius, position, area, strokeColor, fillColor, penWidth, strokePen, fillPen, beginTime, duration, etc.); ensure examples produce valid SVG.

When adding or changing tests, follow **Documentation/standards.md**: add or update doc-proof tests for doc examples; use the same naming as the doc so examples and tests stay in sync.

When writing F# code examples (README, notebooks, docs), apply the same binding and naming rules from **Documentation/standards.md**.
