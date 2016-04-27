namespace SharpVG

type line =
    {
        point1: point
        point2: point
    }

module Line =

    let toTag line =
        {
            name = "line";
            attribute = Some((Point.toDescriptiveStringWithModifier line.point1 "" "1") + " " + (Point.toDescriptiveStringWithModifier line.point2 "" "2"));
            body = None
        }

    let toString = toTag >> Tag.toString