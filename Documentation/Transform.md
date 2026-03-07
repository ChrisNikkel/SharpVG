# Transform

Transforms modify the coordinate system of an element (position, scale, rotation, skew). They can be applied to elements or groups.

## Types

- **Matrix**: Full 6-parameter matrix.
- **Translate**: Move by (x, y).
- **Scale**: Scale by (x, y).
- **Rotate**: Rotate by an angle, optionally about a point.
- **SkewX** / **SkewY**: Skew by an angle.

## Creating transforms

```fsharp
open SharpVG

let translateX = Length.ofInt 10
let translateY = Length.ofInt 20
let move = Transform.createTranslate translateX |> Transform.withY translateY

let scaleX = Length.ofFloat 2.0
let scaleY = Length.ofFloat 2.0
let scale = Transform.createScale scaleX |> Transform.withY scaleY

let pivot = Length.ofInt 50
let rotate = Transform.createRotate 90.0 pivot pivot

let skewX = Transform.createSkewX 15.0
let skewY = Transform.createSkewY 10.0
```

## Example: Group with transform

Apply a transform to a group so all children are translated and scaled together:

```fsharp
let center = Point.ofInts (25, 25)
let radius = Length.ofInt 20
let position = Point.ofInts (0, 0)
let area = Area.ofInts (30, 30)
let circle = Circle.create center radius |> Element.create
let rect = Rect.create position area |> Element.create

let translateX = Length.ofInt 100
let translateY = Length.ofInt 50
let transform = Transform.createTranslate translateX |> Transform.withY translateY
let group = [ circle; rect ]
    |> Group.ofList
    |> Group.withTransform transform
    |> Element.create

group |> Element.toString
```

## Multiple transforms

Use a sequence of transforms; they are combined into one `transform` attribute (applied in order):

```fsharp
let translateX = Length.ofInt 10
let scaleAmount = Length.ofInt 2
let pivot = Length.ofInt 0
let transforms = seq {
    Transform.createTranslate translateX
    Transform.createScale scaleAmount
    Transform.createRotate 45.0 pivot pivot
}
let attr = Transforms.toAttribute transforms
```
