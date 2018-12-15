namespace SharpVG

type Ellipse =
    {
        Center: Point
        Radius: Point
    }

module Ellipse =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag circle =
        Tag.create "ellipse"
        |> Tag.withAttributes ((Point.toAttributesWithModifier "c" "" circle.Center) @ (Point.toAttributesWithModifier "r" "" circle.Radius))

    let toString =
        toTag >> Tag.toString
