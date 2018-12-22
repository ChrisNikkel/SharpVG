namespace SharpVG

type Circle =
    {
        Center: Point
        Radius: Length
    }
with
    static member ToTag circle =
        Tag.create "circle"
        |> Tag.withAttributes ((Attribute.createXML "r" (Length.toString circle.Radius)) :: (Point.toAttributesWithModifier "c" "" circle.Center))


module Circle =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag =
        Circle.ToTag

    let toString =
        toTag >> Tag.toString