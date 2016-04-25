namespace SharpVG

type points = seq<point>

module Points =
    let toString points =
        points
        |> Seq.fold
            (
                fun acc point ->
                acc + Tag.addSpace (acc <> "") + Point.toString point
            ) ""