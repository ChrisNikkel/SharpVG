namespace SharpVG

module Polygon =

    let toTag points =
        Tag.create "polygon" |> Tag.withAttribute (Attribute.create "points" <| Points.toString points)

    let toString points =
        points |> toTag |> Tag.toString