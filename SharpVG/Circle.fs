namespace SharpVG

type Circle =
    {
        Center: Point
        Radius: Length
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Circle =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag circle =
        Tag.create "circle"
        |> Tag.withAttributes ((Attribute.createXML "r" (Length.toString circle.Radius)) :: (Point.toAttributesWithModifier circle.Center "c" ""))

    let toString =
        toTag >> Tag.toString