namespace SharpVG

type ellipse =
    {
        center: point
        radius: point
    }

module Ellipse =

    let toTag ellipse =
        {
            name = "ellipse";
            attribute = Some((Point.toDescriptiveStringWithModifier ellipse.center "c" "") + " r=" + Tag.quote (Point.toString ellipse.radius));
            body = None
        }

    let toString = toTag >> Tag.toString
