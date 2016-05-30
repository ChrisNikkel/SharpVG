namespace SharpVG

type points = seq<point>

module Points =
    let toString (points:points) =
        points
        |> Seq.map Point.toString
        |> String.concat " "