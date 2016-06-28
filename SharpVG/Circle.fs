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
        |> Tag.withAttributes ((Point.toAttributesWithModifier circle.Center "c" "") |> Set.add (Attribute.createXML "r" (Length.toString circle.Radius)))

    let toString =
        toTag >> Tag.toString