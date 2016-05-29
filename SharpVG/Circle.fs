namespace SharpVG

type circle =
    {
        center: point
        radius: length
    }

module Circle =

    let toTag circle =
        {
            name = "circle";
            attribute = Some((Point.toDescriptiveStringWithModifier circle.center "c" "") + " r=" + Tag.quote (Length.toString circle.radius));
            body = None
        }

    let toString = toTag >> Tag.toString