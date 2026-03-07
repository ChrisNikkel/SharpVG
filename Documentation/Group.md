# Group

Groups collect multiple elements (and nested groups) so you can apply a single transform, name, or style set to them. The `<g>` element is used in the output.

## Creating groups

```fsharp
open SharpVG

// Empty group
let empty = Group.empty

// From a list of elements
let center = Point.ofInts (50, 50)
let radius = Length.ofInt 20
let position = Point.ofInts (10, 10)
let area = Area.ofInts (40, 30)
let circle = Circle.create center radius |> Element.create
let rect = Rect.create position area |> Element.create
let group = Group.ofList [ circle; rect ]
// Or: Group.ofSeq (seq { circle; rect }), Group.ofArray [| circle; rect |]
```

## Example: Named group with transform

Build a group, give it an id, and apply a transform. Then render as an element:

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
let group = [ circle; rect ] |> Group.ofList |> Group.withName "myGroup"
    |> Group.withTransform transform

let groupElement = group |> Element.create
groupElement |> Element.toString
```

## Adding elements and transforms

- **Group.addElement** / **Group.addElements**: Add elements to the group body.
- **Group.withBody**: Replace the body.
- **Group.addTransform** / **Group.withTransforms** / **Group.withTransform**: Manage transforms.
- **Group.withName**: Set the group's id.

## Cartesian coordinates

**Group.asCartesian** flips the y-axis and applies a translation so you can use cartesian (math) coordinates:

```fsharp
let originX = Length.ofInt 0
let originY = Length.ofInt 100
let group = Group.ofList [ circle; rect ]
    |> Group.asCartesian originX originY
```

## Styles from a group

**Group.toStyleSet** collects all styles used by elements (and nested groups) in the group. Useful for generating a `<style>` block that covers only those styles.
