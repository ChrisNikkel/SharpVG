module TriangleExample

open SharpVG
open System.Diagnostics
open System.IO
open System

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
    let iterations, triangleLength = 8, 1000.0
    let offsetY = triangleLength - Math.Sqrt 3.0 * triangleLength / 2.0
    let startingTriangle =
            [{
                A = Point.ofFloats (0.0, offsetY)
                B = Point.ofFloats (triangleLength / 2.0, sqrt (0.75 * triangleLength * triangleLength) + offsetY)
                C = Point.ofFloats (triangleLength, offsetY)
            }]

    let center = triangleLength / 2.0 |> Length.ofFloat
    let centerCircle = Circle.create (Point.create center center) (Length.ofFloat 5.0) |> Element.ofCircle |> Element.withStyle style
    let frame = Rect.create Point.origin (Area.ofFloats (triangleLength, triangleLength)) |> Element.ofRect |> Element.withStyle style
    let rotationStart = Transform.createRotate 0.0 center center
    let rotationEnd = Transform.createRotate 360.0 center center
    let sizeStart = Transform.createScale (Length.ofInt 1)
    let sizeEnd = Transform.createScale (Length.ofInt 2)
    let offsetStart = Transform.createTranslate (Length.ofFloat 0.0) |> Transform.withY (Length.ofFloat 0.0)
    let offsetEnd = Transform.createTranslate (Length.ofFloat (triangleLength/(0.0-4.0))) |> (Transform.withY (Length.ofFloat (triangleLength/(0.0-4.0))))
    let timing = Timing.create (TimeSpan.FromSeconds(0.0)) |> Timing.withDuration (TimeSpan.FromSeconds(5.0)) |> Timing.withRepetition { RepeatCount = RepeatCountValue.Indefinite; RepeatDuration = None }
    let rotationAnimation = Animation.createTransform timing rotationStart rotationEnd |> Animation.withAdditive Additive.Sum |> Element.ofAnimation
    let resizeAnimation = Animation.createTransform timing sizeStart sizeEnd |> Animation.withAdditive Additive.Sum |> Element.ofAnimation
    let offsetAnimation = Animation.createTransform timing offsetStart offsetEnd |> Animation.withAdditive Additive.Sum |> Element.ofAnimation

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> (Element.withStyle style))
    |> List.append [resizeAnimation; rotationAnimation; offsetAnimation]
    |> List.append [frame; centerCircle]
    |> Group.ofList
    |> Svg.ofGroup
    |> Svg.withSize (Area.ofFloats (triangleLength, triangleLength))
    |> Svg.withViewbox Point.origin Area.full
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
