namespace SharpVG

type Line =
    {
        Point1: Point
        Point2: Point
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Line =
    let create point1 point2 =
        { Point1 = point1; Point2 = point2 }

    let toTag line =
        {
            Name = "line";
            Attribute = Some((Point.toDescriptiveStringWithModifier line.Point1 "" "1") + " " + (Point.toDescriptiveStringWithModifier line.Point2 "" "2"));
            Body = None
        }

    let toString = toTag >> Tag.toString