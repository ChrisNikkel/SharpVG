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

    // TODO: Add style into plot somehow
    let style = { Stroke = Some(Name Colors.Black); StrokeWidth = Some(Length.ofInt 1); Fill = Some(Name Colors.White); Opacity = None }
    let namedStyle = style |> NamedStyle.ofStyle "std"

    // TODO: Make plotting simpler
    Plot.line [ for i in 0.0 .. 0.02 .. 2.0 * Math.PI -> (1000.0 * sin i, 1000.0 * cos i * sin i) ]
    |> Svg.ofPlot
    |> Svg.withSize Area.full
    |> Svg.withViewbox {Minimums = Point.ofInts (-500, 0); Size = Area.ofInts (2000, 1000)}
    |> Svg.withStyle namedStyle
    |> Svg.toHtml "SVG Graph Example"
    |> saveToFile fileName

    openFile fileName
    0 // return an integer exit code
