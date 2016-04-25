module Triangle

open LogHelpers
open SharpVG
open SharpVG.Core
open Xunit
open FsCheck
open FsCheck.Xunit

type triangle =
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
    let it = t |> List.map (fun st -> insideTriangles st) |> List.concat;
    if iteration > 1 then
        recursiveTriangles it (iteration - 1)
    else
        it

let triangleSize = 100.0
let iterations = 11
let startingTriangle =
        [{
            a = { x = Ems(-1.0 * triangleSize); y = Ems(0.0)};
            b = { x = Ems(0.0); y = Ems(triangleSize * sqrt triangleSize)};
            c = { x = Ems(triangleSize); y = Ems(0.0) }
        }]

let allTriangles = recursiveTriangles startingTriangle iterations |> List.map (fun t -> triangleToPoints t) |> List.concat
[<Property>]
let ``draw triangles`` =
    let size = {height = Pixels 30; width = Pixels 30}
    allTriangles |> Points.toString |> (svg size) |> html "SVG Demo"
