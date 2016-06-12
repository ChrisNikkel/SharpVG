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
            Center = center;
            Radius = radius
        }

    let toTag ellipse =
        {
            Name = "ellipse";
            Attribute = Some((Point.toDescriptiveStringWithModifier ellipse.Center "c" "") + " r=" + Tag.quote (Point.toString ellipse.Radius));
            Body = None
        }

    let toString = toTag >> Tag.toString
