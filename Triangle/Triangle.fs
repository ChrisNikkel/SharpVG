module TriangleExample

open SharpVG
open System.Diagnostics
open System.IO

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
    let it = t |> List.map insideTriangles |> List.concat;
    if iteration > 1 then
        recursiveTriangles it (iteration - 1)
    else
        it

[<EntryPoint>]
let main argv =

    // Helper Functions
    let saveToFile name lines =
        File.WriteAllLines(name, [lines]);

    let openFile (name:string) =
        Process.Start(name) |> ignore

    // Initialization
    let fileName = ".\\triangle.html"
    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None; Name = Some("std") }
    let iterations, triangleLength, margin = 7, 1000.0, 100.0
    let startingTriangle =
            [{
                A = Point.origin
                B = Point.ofFloats (triangleLength / 2.0, sqrt (0.75 * triangleLength * triangleLength))
                C = Point.ofFloats (triangleLength, 0.0)
            }]

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> (Element.withStyle style))
    |> Group.ofList
    |> Group.asCartesian (Length.ofFloat margin) (Length.ofFloat triangleLength)
    |> Svg.ofGroup
    |> Svg.withSize (Area.ofFloats (triangleLength + margin, triangleLength + margin))
    |> Svg.withViewbox Point.origin Area.full
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
