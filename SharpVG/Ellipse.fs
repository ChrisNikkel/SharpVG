namespace SharpVG

type Ellipse =
    {
        Center: Point
        Radius: Point
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Ellipse =
    let create center radius =
        {
            Center = center
            Radius = radius
        }

    let toTag circle =
        Tag.create "ellipse"
        |> Tag.withAttributes ((Point.toAttributesWithModifier circle.Center "c" "") + (Point.toAttributesWithModifier circle.Radius "r" ""))

    let toString =
        toTag >> Tag.toString
