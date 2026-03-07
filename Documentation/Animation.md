# Animation

SharpVG supports SVG animation via the `Animation` module. Animations can change attributes over time, animate transforms, or follow a motion path. Timing controls when and how long each animation runs.

## Types

- **AnimationType**: `Set` (discrete value change), `Animate` (from/to), `Transform` (transform from/to), or `Motion` (path-based).
- **Timing**: Controls begin, duration, repeat, and fill (see Timing section below).
- **Additive**: `Replace` (default) or `Sum` (additive with other animations).
- **CalculationMode** (for motion): `Discrete`, `Linear`, `Paced`, or `Spline`.

## Timing

Timing is created with `Timing.create` (pass a `TimeSpan` for when the animation begins), then optionally configured:

- **withDuration**: How long the animation runs.
- **withMediaDuration**: Use media duration (e.g. for media elements).
- **withEnd**: End time.
- **withMinimum** / **withMaximum**: Min/max duration.
- **withResart**: `Always`, `WhenNotActive`, or `Never`.
- **withRepetition**: Repeat count and/or repeat duration.
- **withFinalState**: `Freeze` (hold final state) or `Remove` (revert).

```fsharp
open SharpVG
open System

let beginTime = TimeSpan.FromSeconds 1.0
let timing = Timing.create beginTime
```

### Duration and repeat

```fsharp
let beginTime = TimeSpan.FromSeconds 0.0
let duration = TimeSpan.FromSeconds 3.0
let timing = Timing.create beginTime
    |> Timing.withDuration duration
    |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
```

### Restart and fill

```fsharp
let beginTime = TimeSpan.FromSeconds 2.0
let duration = TimeSpan.FromSeconds 1.0
let timing = Timing.create beginTime
    |> Timing.withDuration duration
    |> Timing.withResart Always
    |> Timing.withFinalState Freeze
```

### Repetition with count and duration

```fsharp
let beginTime = TimeSpan.FromSeconds 0.0
let duration = TimeSpan.FromSeconds 2.0
let repeatDuration = TimeSpan.FromSeconds 6.0
let timing = Timing.create beginTime
    |> Timing.withDuration duration
    |> Timing.withRepetition
        { RepeatCount = RepeatCount 3.0
          RepeatDuration = Some repeatDuration }
```

## Creating animations

### Transform animation

Animate an element's transform from one value to another (e.g. rotate, scale, translate):

```fsharp
open SharpVG
open System

let pivot = Length.ofInt 20
let fromTransform = Transform.createRotate 0.0 pivot pivot
let toTransform = Transform.createRotate 360.0 pivot pivot
let beginTime = TimeSpan.FromSeconds 0.0
let duration = TimeSpan.FromSeconds 2.0
let timing = Timing.create beginTime
    |> Timing.withDuration duration
    |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }

let animation = Animation.createTransform timing fromTransform toTransform
let position = Point.ofInts (10, 10)
let area = Area.ofInts (40, 40)
let rect = Rect.create position area
    |> Element.create
    |> Element.withAnimation animation

rect |> Element.toString
```

### Set (discrete attribute change)

`Set` changes an attribute to a value at a specific time (no interpolation). For example, hide an element after 1 second by setting `visibility` to `"hidden"`:

```fsharp
let position = Point.ofInts (10, 10)
let area = Area.ofInts (40, 40)
let rect = Rect.create position area |> Element.create

let beginTime = TimeSpan.FromSeconds 1.0
let timing = Timing.create beginTime
let animation = Animation.createSet timing AttributeType.XML "visibility" "hidden"
let rectWithSet = rect |> Element.withAnimation animation

rectWithSet |> Element.toString
```
This produces a `<rect>` with a child `<set>`; when the animation begins (after 1s), the rect becomes hidden.

### Animate (from/to)

Animate an attribute from one value to another:

```fsharp
let beginTime = TimeSpan.FromSeconds 0.0
let duration = TimeSpan.FromSeconds 3.0
let timing = Timing.create beginTime |> Timing.withDuration duration
let animation = Animation.createAnimation timing AttributeType.XML "cy" "50" "250"
```
Use with `Element.withAnimation`; for linked animation use `Element.withHref "#targetId"`.

### Motion (path-based)

Animate an element along a path:

```fsharp
let startingPoint = Point.ofInts (50, 50)
let pathPoint1 = Point.ofInts (100, 100)
let pathPoint2 = Point.ofInts (200, 50)
let path = Path.empty
    |> Path.addMoveTo Absolute startingPoint
    |> Path.addLinesTo Absolute (seq { pathPoint1; pathPoint2 })
let beginTime = TimeSpan.FromSeconds 0.0
let duration = TimeSpan.FromSeconds 2.0
let timing = Timing.create beginTime
    |> Timing.withDuration duration
    |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
let animation = Animation.createMotion timing path (Some CalculationMode.Paced)

let radius = Length.ofInt 10
let circle = Circle.create startingPoint radius
    |> Element.create
    |> Element.withAnimation animation
```

## Options

- **withAdditive**: `Animation.withAdditive Additive.Sum` for additive animation.
- **withKeyTimes**: `Animation.withKeyTimes [0.0; 0.25; 0.5; 0.75; 1.0]` for key times.

## Attaching to elements

- **Element.withAnimation**: add one animation to an element.
- **Element.withAnimations**: add multiple animations (e.g. scale + rotate + translate).
- **Element.withHref**: use with a standalone animation element to target another element by id (`#"id"`).
