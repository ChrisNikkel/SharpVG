module Triangle

open SharpVG
open System
open System.Diagnostics
open System.IO

type Triangle =
    {
        A : point
        B : point
        C : point
    }

let midpoint a b =
    let ax = Length.toFloat a.x
    let ay = Length.toFloat a.y
    let bx = Length.toFloat b.x
    let by = Length.toFloat b.y

    {x = Pixels((ax + bx) / 2.0); y = Pixels((ay + by) / 2.0)}

let insideTriangles t =
    [
        {A = t.A; B = midpoint t.A t.B; C = midpoint t.A t.C}
        {A = midpoint t.A t.B; B = t.B;  C = midpoint t.B t.C}
        {A = midpoint t.A t.C; B = midpoint t.B t.C; C = t.C}
    ]

let triangleToPolygon t =
    [ t.A; t.B; t.C] |> Element.ofPolygon

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
                A = {x = Pixels(0.0); y = Pixels(0.0)};
                B = {x = Pixels(triangleLength / 2.0); y = Pixels(sqrt (0.75 * triangleLength * triangleLength))};
                C = {x = Pixels(triangleLength); y = Pixels(0.0)}
            }]
    let style = { stroke = Name colors.Green; strokeWidth = Pixels 1.0; fill = Name colors.White; opacity = 1.0 }

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> Element.withStyle style)
    |> Svg.ofList
    |> Svg.withSize {height = Pixels 1000.0; width = Pixels 1000.0}
    |> Svg.withViewbox {Minimums = {x = Pixels 0.0; y = Pixels 0.0}; Size = {height = Pixels 1000.0; width = Pixels 1000.0}}
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
