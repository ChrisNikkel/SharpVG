namespace SharpVG

module Polygon =
    let toTag points =
        {
            name = "polygon";
            attribute = Some("points=" + Tag.quote (Points.toString points))
            body = None
        }

    let toString points = points |> toTag |> Tag.toString