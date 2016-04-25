namespace SharpVG

module Polyline =
    let toString points =
        {
            name = "polyline";
            attribute = Some("points=" + Tag.quote (Points.toString points))
            body = None
        }
        |> Tag.toString