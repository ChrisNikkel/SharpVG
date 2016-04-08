module Triangle

open LogHelpers
open SharpVG
open SharpVG.Core
open Xunit
open FsCheck
open FsCheck.Xunit
open SizeHelpers

type triangle =
    {
        a : Point
        b : Point
        c : Point
    }

let midpoint a b =
    let ax = sizeToFloat a.X
    let ay = sizeToFloat a.Y
    let bx = sizeToFloat b.X
    let by = sizeToFloat b.Y
    
    { X = Ems((ax + bx) / 2.0); Y = Ems((ay + by) / 2.0) }

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
            a = { X = Ems(-1.0 * triangleSize); Y = Ems(0.0)}; 
            b = { X = Ems(0.0); Y = Ems(triangleSize * sqrt triangleSize)}; 
            c = { X = Ems(triangleSize); Y = Ems(0.0) }
        }]

let allTriangles = recursiveTriangles startingTriangle iterations |> List.map (fun t -> triangleToPoints t) |> List.concat
[<Property>]
let ``draw triangles`` =
    allTriangles