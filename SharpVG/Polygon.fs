namespace SharpVG

module Polygon =
    let toTag points =
        {
            Name = "polygon";
            Attribute = Some("points=" + Tag.quote (Points.toString points))
            Body = None
        }

    let toString points = points |> toTag |> Tag.toString