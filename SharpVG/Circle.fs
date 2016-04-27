namespace SharpVG

type circle =
    {
        center: point
        radius: size
    }

module Circle =

    let toTag circle =
        {
            name = "circle";
            attribute = Some((Point.toDescriptiveStringWithModifier circle.center "c" "") + " r=" + Tag.quote (Size.toString circle.radius));
            body = None
        }

    let toString = toTag >> Tag.toString