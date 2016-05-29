module Triangle

open SharpVG
open System
open System.Diagnostics
open System.IO

type Triangle =
    {
        a : point
        b : point
        c : point
    }

let midpoint a b =
    let ax = Length.toFloat a.x
    let ay = Length.toFloat a.y
    let bx = Length.toFloat b.x
    let by = Length.toFloat b.y

    {x = Pixels((ax + bx) / 2.0); y = Pixels((ay + by) / 2.0)}

let insideTriangles t =
    [
        {a = t.a; b = midpoint t.a t.b; c = midpoint t.a t.c}
        {a = midpoint t.a t.b; b = t.b;  c = midpoint t.b t.c}
        {a = midpoint t.a t.c; b = midpoint t.b t.c; c = t.c}
    ]

let triangleToPoints t =
    [ t.a; t.b; t.c]

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
    let iterations = 7
    let triangleLength = 1000.0
    let startingTriangle =
            [{
                a = {x = Pixels(0.0); y = Pixels(0.0)};
                b = {x = Pixels(triangleLength / 2.0); y = Pixels(sqrt (0.75 * triangleLength * triangleLength))};
                c = {x = Pixels(triangleLength); y = Pixels(0.0)}
            }]
    let style = { stroke = Name colors.Green; strokeWidth = Pixels 2.0; fill = Name colors.White; opacity = 1.0 }

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPoints >> Element.ofPolygon >> Element.withStyle style)
    |> Svg.ofList
    |> Svg.withSize {height = Pixels 1000.0; width = Pixels 1000.0}
    |> Svg.withViewbox {minimums = {x = Pixels 0.0; y = Pixels 0.0}; size = {height = Pixels 1000.0; width = Pixels 1000.0}}
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
