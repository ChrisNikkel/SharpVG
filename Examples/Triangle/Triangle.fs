module TriangleExample

open SharpVG
open System
open Helpers.File

type Triangle =
    {
        A : Point
        B : Point
        C : Point
    }

let midpoint a b =
    let (ax, ay), (bx, by) = Point.toFloats a, Point.toFloats b
    Point.ofFloats ((ax + bx) / 2.0, (ay + by) / 2.0)

let insideTriangles t =
    [
        { A = t.A; B = midpoint t.A t.B; C = midpoint t.A t.C }
        { A = midpoint t.A t.B; B = t.B;  C = midpoint t.B t.C }
        { A = midpoint t.A t.C; B = midpoint t.B t.C; C = t.C }
    ]

let triangleToPolygon t =
    [t.A; t.B; t.C] |> Element.ofPolygon

let rec recursiveTriangles t iteration =
    let it = t |> List.collect insideTriangles;
    if iteration > 1 then
        recursiveTriangles it (iteration - 1)
    else
        it

[<EntryPoint>]
let main argv =

    let fileName = "Triangle.html"
    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None; Name = Some("std") }
    let iterations, triangleLength = 8, 1000.0
    let offsetY = triangleLength - Math.Sqrt 3.0 * triangleLength / 2.0
    let startingTriangle =
            [{
                A = Point.ofFloats (0.0, offsetY)
                B = Point.ofFloats (triangleLength / 2.0, sqrt (0.75 * triangleLength * triangleLength) + offsetY)
                C = Point.ofFloats (triangleLength, offsetY)
            }]

    let center = triangleLength / 2.0 |> Length.ofFloat

    let rotate degrees = Transform.createRotate degrees center center
    let rotationStart, rotationEnd = rotate 0.0, rotate 360.0

    let scale = Length.ofFloat >> Transform.createScale
    let sizeStart, sizeEnd = scale 1.0, scale 2.0

    let offset x y =  Transform.createTranslate (Length.ofFloat x) |> Transform.withY (Length.ofFloat y)
    let offsetStart = offset 0.0 0.0
    let offsetEnd = offset (triangleLength/(-4.0)) (triangleLength/(2.0))

    let timing = Timing.create (TimeSpan.FromSeconds(0.0)) |> Timing.withDuration (TimeSpan.FromSeconds(5.0)) |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
    let animations =
        List.map2 (Animation.createTransform timing)
            [sizeStart; rotationStart; offsetStart]
            [sizeEnd; rotationEnd; offsetEnd]
        |> List.map (Animation.withAdditive Additive.Sum >> Element.ofAnimation)

    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> (Element.withStyle style))
    |> List.append animations
    |> Group.ofList |> Svg.ofGroup
    |> Svg.withSize (Area.ofFloats (triangleLength*5.0, triangleLength*3.0))
    |> Svg.withViewbox Point.origin Area.full
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
