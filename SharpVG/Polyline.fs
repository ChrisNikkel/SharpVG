namespace SharpVG

module Polyline =

    let toTag points =
        {
            Name = "polyline";
            Attribute = Some("points=" + Tag.quote (Points.toString points))
            Body = None
        }

    let toString points = points |> toTag |> Tag.toString