# Svg

**Svg** is the top-level container that turns elements (and groups) into an SVG document or a full HTML page.

## From elements to output

```fsharp
open SharpVG

// One element (name colors, pen width, and pens to simplify; see Styling.md)
let strokeColor = Color.ofName Colors.Blue
let fillColor = Color.ofName Colors.Cyan
let penWidth = Length.ofInt 3
let strokePen = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
let fillPen = Pen.create fillColor
let style = Style.createWithPen strokePen |> Style.withFillPen fillPen
let position = Point.ofInts (10, 10)
let area = Area.ofInts (50, 50)
let rect = Rect.create position area
  |> Element.createWithStyle style
let svg = rect |> Svg.ofElement
let html = svg |> Svg.toHtml "My Page"
// html is a full <!DOCTYPE html>... string

// Or just the SVG string (no HTML wrapper)
let svgString = svg |> Svg.toString
```

## API

- **Svg.ofElement** — build an Svg from a single element.
- **Svg.ofSeq** / **Svg.ofList** / **Svg.ofArray** — build an Svg from a sequence of elements.
- **Svg.ofGroup** — build an Svg from a single group (with default size).
- **Svg.ofElementsWithDefinitions** definitions elements — build an Svg with a definitions block and a sequence of elements as the body.
- **Svg.withDefinitions** definitions — attach a SvgDefinitions block to an Svg (rendered as `<defs>...</defs>` before the body).
- **Svg.toString** — serialize the Svg to an `<svg>...</svg>` string.
- **Svg.toHtml** title — wrap that string in a minimal HTML document with the given title.

## Optional: size and viewBox

- **Svg.withSize** — set the SVG width/height (e.g. `Area.ofInts (400, 300)`).
- **Svg.withViewBox** — set the viewBox attribute.

Example:

```fsharp
let size = Area.ofInts (400, 300)
let viewBoxMin = Point.ofInts (0, 0)
let viewBoxSize = Area.ofInts (100, 100)
let svg =
    rect |> Svg.ofElement
    |> Svg.withSize size
    |> Svg.withViewBox (ViewBox.create viewBoxMin viewBoxSize)
```

## Definitions (defs)

Reusable content (symbols, filters, gradients, etc.) can be placed in a **definitions** block so it is not rendered directly; reference it by id (e.g. with `<use href="#id">`). Build a **SvgDefinitions** and attach it to the Svg.

- **SvgDefinitions.create** — start an empty definitions block.
- **SvgDefinitions.addElement** / **SvgDefinitions.addGroup** — add elements or groups to the block (each referenced item must have an id, e.g. via `Element.withName`).

Example: symbol in definitions, `<use>` in body:

```fsharp
let viewBox = ViewBox.create Point.origin Area.full
let symbolContent = [ Circle.create Point.origin (Length.ofInt 10) |> Element.create ]
let circleIconSymbol = Symbol.create viewBox |> Symbol.withBody symbolContent |> Element.createWithName "icon"
let useEl = Use.create circleIconSymbol Point.origin |> Element.create

let definitions = SvgDefinitions.create |> SvgDefinitions.addElement circleIconSymbol
let svg = [ useEl ] |> Svg.ofElementsWithDefinitions definitions
// Or: let svg = [ useEl ] |> Svg.ofSeq |> Svg.withDefinitions definitions
```

The output will contain `<defs><symbol id="icon">...</symbol></defs>` followed by `<use href="#icon" .../>`.

## Pipeline

Shape or Group → Element (with optional Style) → Svg (ofElement / ofSeq) → toString or toHtml.
