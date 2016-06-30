module GraphExample

open SharpVG
open System
open System.Diagnostics
open System.IO

[<EntryPoint>]
let main argv = 
    // Helper Functions
    let saveToFile name lines =
        File.WriteAllLines(name, [lines]);

    let openFile (name:string) =
        Process.Start(name) |> ignore

    // Initalize
    let fileName = ".\\Graph.html"

    Plot.line [ for i in 0.0 .. 0.02 .. 2.0 * Math.PI -> (1000.0 * sin i, 1000.0 * cos i * sin i) ]
    |> Svg.ofPlot
    |> Svg.withSize (Area.ofInts (1000, 1000))
    |> Svg.withViewbox {Minimums = Point.ofInts (0, 0); Size = Area.ofInts (1000, 1000)}
    |> Svg.toHtml "SVG Graph Example"
    |> saveToFile fileName

    openFile fileName
    0 // return an integer exit code
