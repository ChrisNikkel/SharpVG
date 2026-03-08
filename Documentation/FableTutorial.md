# Using SharpVG with Fable

[Fable](https://fable.io) compiles F# to JavaScript, enabling SharpVG to run directly in the browser. SharpVG requires no changes — it is a pure F# library with no .NET I/O or platform-specific APIs, so Fable can compile it as-is.

## How it works

The integration is one line:

```fsharp
document.getElementById("app").innerHTML <- svg |> Svg.toString
```

SharpVG generates SVG as a plain string. Fable gives you access to the browser DOM. You generate the string exactly as you would in a console or server app, then inject it into the page. For live updates (animations, clocks, data dashboards) wrap the render call in `setInterval`.

## Prerequisites

- .NET SDK 8+
- Node.js 18+ and npm
- Fable 4 installed as a global tool:
  ```bash
  dotnet tool install -g fable
  ```

## Running the example

```bash
cd Examples/Fable
npm install
npm start        # compiles F# → JS and starts Vite dev server
```

Open `http://localhost:5173` to see a live SVG clock that updates every second.

To produce a production build:

```bash
npm run build    # outputs to dist/
```

## Project layout

```
Examples/Fable/
├── index.html        # single-page shell — just a <div id="app">
├── package.json      # npm scripts + Vite dev dependency
└── src/
    ├── App.fsproj    # references SharpVG + Fable.Browser.Dom
    └── App.fs        # all application logic
```

`App.fsproj` carries the only Fable dependencies (`Fable.Core`, `Fable.Browser.Dom`). SharpVG itself has none.

## Code walkthrough

### Static elements

Elements that don't change with time are built once at module load:

```fsharp
let private center = Point.ofInts (100, 100)
let private pivot  = Length.ofInt 100

let private faceStyle =
    let strokeColor = Color.ofName Colors.Black
    let fillColor   = Color.ofName Colors.White
    let penWidth    = Length.ofInt 2
    let strokePen   = Pen.createWithOpacityAndWidth strokeColor 1.0 penWidth
    let fillPen     = Pen.create fillColor
    Style.createWithPen strokePen |> Style.withFillPen fillPen

let private hourMarkers =
    let strokeColor = Color.ofName Colors.Black
    let penWidth    = Length.ofInt 2
    let style       = Style.createWithStroke strokeColor |> Style.withStrokeWidth penWidth
    [ for i in 0 .. 11 do
        let angle = float i * 30.0
        let inner = Point.ofInts (100, 14)
        let outer = Point.ofInts (100, 24)
        Line.create inner outer
        |> Element.createWithStyle style
        |> Element.withTransform (Transform.createRotate angle pivot pivot) ]
```

`Transform.createRotate angle pivot pivot` rotates a shape around `(100, 100)` — the clock center. Each tick mark is an identical vertical line; rotating it produces the 12-hour positions.

### Dynamic elements

Clock hands are lines from the center, rotated to the current angle:

```fsharp
let private hand tipY width color angle =
    let strokeColor = Color.ofName color
    let penWidth    = Length.ofInt width
    let strokePen   = Pen.createWithWidth strokeColor penWidth
    let style       = Style.createWithPen strokePen
    let line    = Line.create center (Point.ofInts (100, tipY))
    let element = line |> Element.createWithStyle style
    element |> Element.withTransform (Transform.createRotate angle pivot pivot)
```

A smaller `tipY` value means a longer hand (SVG y-coordinates increase downward, so `y=28` is closer to the top than `y=50`).

### Render and update loop

```fsharp
let private render () =
    let now         = DateTime.Now
    let secondAngle = float now.Second * 6.0           // 360° / 60 steps
    let minuteAngle = float now.Minute * 6.0 + float now.Second * 0.1
    let hourAngle   = float (now.Hour % 12) * 30.0 + float now.Minute * 0.5

    let svg =
        [ face ] @ hourMarkers @ [ hourHand; minuteHand; secondHand; centerDot ]
        |> Svg.ofList
        |> Svg.withSize (Area.ofInts (200, 200))
        |> Svg.toString

    // Browser.Dom is qualified (not opened) to avoid shadowing SharpVG.Element.
    Browser.Dom.document.getElementById("app").innerHTML <- svg

do
    render ()
    Browser.Dom.window.setInterval((fun () -> render ()), 1000) |> ignore
```

> **Namespace note:** `open Browser.Dom` would shadow `SharpVG.Element` with the DOM `Element` type and cause compile errors. Use `Browser.Dom.document` and `Browser.Dom.window` as qualified references instead.

`Svg.ofList` collects a list of elements, `Svg.withSize` sets the `width`/`height` attributes, and `Svg.toString` produces the final SVG string. The `do` block runs at startup and schedules a refresh every second.

## Key patterns to take further

**Event-driven updates** — attach a `click` or `mousemove` handler and call `render` with new state instead of using `setInterval`.

**Stateful rendering** — hold mutable state (an F# `ref` or a mutable record) and update it in response to events before re-rendering.

**Reusable named styles** — call `Style.withName` to create a CSS class-based style and use `Svg.withDefinitions` to embed a `<style>` block once, avoiding repeated inline attributes across many elements.

**Server/browser code sharing** — because SharpVG has no platform dependency, the same shape-building functions can run on both the server (generating static HTML) and in the browser (via Fable) without any conditional compilation.
