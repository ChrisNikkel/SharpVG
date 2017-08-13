module TriangleExample

open SharpVG
open Helpers.File
open Helpers.Random

//open System.Diagnostics
//open System.IO
//open System

[<EntryPoint>]
let main argv =

    let randomShapes =
        randomsBetween "shapes" 1 3
        |> Seq.take 10
        |> Seq.map
            (
                function
                    | 1 -> Circle.create Point.origin (Length.ofFloat 10.0) |> Element.ofCircle
                    | 2 -> Ellipse.create Point.origin Point.origin |> Element.ofEllipse
                    | 3 -> Rect.create Point.origin (Area.ofFloats (10.0, 10.0)) |> Element.ofRect
                    | _ -> failwith "inconceivable"
            )

    // Initialization
    let fileName = ".\\animate.html"
    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None; Name = Some("std") }
    (*
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

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> (Element.withStyle style))
    |> List.append animations
    |> Group.ofList |> Svg.ofGroup
    |> Svg.withSize (Area.ofFloats (triangleLength*5.0, triangleLength*3.0))
    |> Svg.withViewbox Point.origin Area.full
    |> Svg.toHtml "SVG Animate Example"
    |> saveToFile fileName

    openFile fileName *)
    0
