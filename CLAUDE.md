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

### Documentation (code examples)

**Named bindings:** Do not use inline construction inside API calls in examples (e.g. no `Point.ofInts (x, y)` or `Length.ofInt n` inside the call). Define values with `let` first, then use the variable. Tuples of named bindings are fine when appropriate (e.g. `(startPoint, endPoint)` for a line).

**Standard names** (use consistently):

- **Points**: `center` (circle/ellipse center), `position` (rect/shape position), `pathPoint1`, `pathPoint2`, `startingPoint` for path motion. For lines: `startPoint`, `endPoint` (or `fromPoint`, `toPoint`).
- **Areas/sizes**: `area` (rect size), `size` when referring to dimensions.
- **Lengths**: `radius` (circle), `penWidth` (stroke), `pivot` (rotate center when a single length is reused for x/y).
- **Time**: `beginTime`, `duration`, `repeatDuration`.
- **Style**: `strokeColor`, `fillColor`, `strokePen`, `fillPen`, `style`.
- **Transform**: `translateX`, `translateY` (or `offsetX`, `offsetY`); `transform` for the composed value.

**API:** Use tuple form for `Point.ofInts` and `Area.ofInts`: `(x, y)` and `(w, h)`.

**Valid SVG:** Every doc example must be runnable and produce valid SVG (no orphan animations or incomplete snippets).

### Testing

**Doc-proof tests:** For each non-trivial code example in the user documentation (the [SharpVG wiki](https://github.com/ChrisNikkel/SharpVG.wiki), typically checked out at `~/Code/SharpVG.wiki`), there should be a test that runs the same logic and asserts on output (e.g. `Assert.Contains` for expected tags/attributes).

**Naming:** Name facts like `"Module wiki - short description"` or `"Wiki: Group — Creating groups example"`. Keep a brief comment above the fact pointing to the wiki page and section.

**Coverage:** Use the same variable names (`center`, `radius`, `position`, `area`, etc.) as the docs so examples and tests stay in sync.

### Coding (library / tests)

- **F# style**: Match existing style (modules, `let` bindings, pipelines).
- **SharpVG API**: Use `Point.ofInts (x, y)` and `Area.ofInts (w, h)` (tuple form), not the curried form.
