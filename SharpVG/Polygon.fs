namespace SharpVG

module Polygon =
    let toString points =
        {
            name = "polygon";
            attribute = Some("points=" + Tag.quote (Points.toString points))
            body = None
        }
        |> Tag.toString