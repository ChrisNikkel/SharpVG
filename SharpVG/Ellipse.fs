namespace SharpVG

type ellipse =
    {
        center: point
        radius: point
    }

module Ellipse =
    let toString ellipse =
        {
            name = "ellipse";
            attribute = Some((Point.toDescriptiveStringWithModifier ellipse.center "c" "") + " r=" + Tag.quote (Point.toString ellipse.radius));
            body = None
        }
        |> Tag.toString
