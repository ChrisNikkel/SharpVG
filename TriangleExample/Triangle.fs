module Triangle

open SharpVG
open System.Diagnostics
open System.IO

type Triangle =
    {
        A : point
        B : point
        C : point
    }

let midpoint a b =
    let (ax, ay), (bx, by) = Point.toDoubles a, Point.toDoubles b
    { X = Pixels((ax + bx) / 2.0); Y = Pixels((ay + by) / 2.0) }

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
    let style = { Stroke = Some(Name colors.Black); StrokeWidth = Some(Pixels 1.0); Fill = Some(Name colors.White); Opacity = None }
    let (iterations, triangleLength) = (7, 1000.0)
    let startingTriangle =
            [{
                A = { X = Pixels(0.0); Y = Pixels(0.0) };
                B = { X = Pixels(triangleLength / 2.0); Y = Pixels(sqrt (0.75 * triangleLength * triangleLength)) };
                C = { X = Pixels(triangleLength); Y = Pixels(0.0) }
            }]

    // Execute
    recursiveTriangles startingTriangle iterations
    |> List.map (triangleToPolygon >> Element.withStyle style)
    |> Svg.ofList
    |> Svg.withSize {Height = Pixels 1000.0; Width = Pixels 1000.0}
    |> Svg.withViewbox {Minimums = { X = Pixels 0.0; Y = Pixels 0.0}; Size = {Height = Pixels 1000.0; Width = Pixels 1000.0 }}
    |> Svg.toHtml "SVG Triangle Example"
    |> saveToFile fileName

    openFile fileName
    0
