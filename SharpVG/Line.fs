namespace SharpVG

type Line =
    {
        Point1: Point
        Point2: Point
    }

module Line =
    let create point1 point2 =
        { Point1 = point1; Point2 = point2 }

    let toTag line =
        Tag.create "line"
        |> Tag.withAttributes (Point.toAttributesWithModifier "" "1" line.Point1)
        |> Tag.addAttributes (Point.toAttributesWithModifier "" "2" line.Point2)

    let toString = toTag >> Tag.toString