# Creating a line

```fsharp
let start = Point.ofInts (5, 0)
let finish =  Point.ofInts (60, 9)

let style = Style.create (Name Colors.Cyan) (Name Colors.Blue) (Length.ofInt 3) 1.0

Line.create start finish
  |> Element.ofLine
  |> Element.withStyle style
  |> Element.toString
  |> printf "%A"
```
```html
<line stroke="blue" stroke-width="3" fill="cyan" opacity="1" x1="5" y1="0" x2="60" y2="9"/>
```
