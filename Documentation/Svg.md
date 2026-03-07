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

## Pipeline

Shape or Group → Element (with optional Style) → Svg (ofElement / ofSeq) → toString or toHtml.
