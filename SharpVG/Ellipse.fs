namespace SharpVG

type Ellipse =
    {
        Center: Point
        Radius: Point
    }
with
    static member ToTag circle =
        Tag.create "ellipse"
        |> Tag.withAttributes ((Point.toAttributesWithModifier "c" "" circle.Center) @ (Point.toAttributesWithModifier "r" "" circle.Radius))

module Ellipse =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag  =
        Ellipse.ToTag

    let toString =
        toTag >> Tag.toString
