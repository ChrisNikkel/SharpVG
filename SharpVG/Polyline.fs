namespace SharpVG

module Polyline =

    let toTag points =
        {
            name = "polyline";
            attribute = Some("points=" + Tag.quote (Points.toString points))
            body = None
        }

    let toString points = points |> toTag |> Tag.toString