module Triangle

open SharpVG
open SharpVG.Core

type Triangle =
    {
        a : point
        b : point
        c : point
    }

let midpoint a b =
    let ax = Size.toFloat a.x
    let ay = Size.toFloat a.y
    let bx = Size.toFloat b.x
    let by = Size.toFloat b.y

    { x = Ems((ax + bx) / 2.0); y = Ems((ay + by) / 2.0) }

let insideTriangles t =
    [
        {a = t.a; b = midpoint t.a t.b; c = midpoint t.a t.c}
        {a = midpoint t.a t.b; b = t.b;  c = midpoint t.b t.c}
        {a = midpoint t.a t.c; b = midpoint t.b t.c; c = t.c}
    ]

let triangleToPoints t =
    [ t.a; t.b; t.c; t.a]

let rec recursiveTriangles t iteration =
    let it = t |> List.map insideTriangles |> List.concat;
    if iteration > 1 then
        recursiveTriangles it (iteration - 1)
    else
        it

let triangleSize = 100.0
let iterations = 2
let startingTriangle =
        [{
            a = { x = Ems(-1.0 * triangleSize); y = Ems(0.0)};
            b = { x = Ems(0.0); y = Ems(triangleSize * sqrt triangleSize)};
            c = { x = Ems(triangleSize); y = Ems(0.0) }
        }]

let allTriangles = recursiveTriangles startingTriangle iterations |> List.map triangleToPoints |> List.concat

[<EntryPoint>]
let main argv = 
    let size = {height = Pixels 30; width = Pixels 30}
    let style = { stroke = Hex 0x00ff00; strokeWidth = Pixels 2; fill = Hex 0x000000}
    allTriangles |> Element.ofPolygon |> Element.withStyle style |>  Element.toString |> (svg size) |> html "SVG Demo" |> printfn "%A"
    0
