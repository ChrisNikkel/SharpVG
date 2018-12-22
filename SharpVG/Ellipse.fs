namespace SharpVG

type Ellipse =
    {
        Center: Point
        Radius: Point
    }
with
    static member ToTag ellipse =
        Tag.create "ellipse"
        |> Tag.withAttributes ((Point.toAttributesWithModifier "c" "" ellipse.Center) @ (Point.toAttributesWithModifier "r" "" ellipse.Radius))

    override this.ToString() =
        Ellipse.ToTag this |> Tag.toString

module Ellipse =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag  =
        Ellipse.ToTag

    let toString (ellipse : Ellipse) =
        ellipse.ToString()
