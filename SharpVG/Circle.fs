namespace SharpVG

type circle =
    {
        center: point
        radius: size
    }

module Circle =
    let toString circle =
        {
            name = "circle";
            attribute = Some((Point.toDescriptiveStringWithModifier circle.center "c" "") + " r=" + Tag.quote (Size.toString circle.radius));
            body = None
        }
        |> Tag.toString
