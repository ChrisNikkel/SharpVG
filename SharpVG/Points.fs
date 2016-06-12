namespace SharpVG

type Points = seq<Point>

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Points =
    let toString (points:Points) =
        points
        |> Seq.map Point.toString
        |> String.concat " "