namespace SharpVG

type circle =
    {
        Center: point
        Radius: length
    }

module Circle =

    let toTag circle =
        {
            Name = "circle";
            Attribute = Some((Point.toDescriptiveStringWithModifier circle.Center "c" "") + " r=" + Tag.quote (Length.toString circle.Radius));
            Body = None
        }

    let toString = toTag >> Tag.toString