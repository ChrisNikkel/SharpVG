namespace SharpVG

module Polyline =

    let toTag points =
        Tag.create "polyline" |> Tag.withAttribute (Attribute.createXML "points" <| Points.toString points)

    let toString points =
        points |> toTag |> Tag.toString