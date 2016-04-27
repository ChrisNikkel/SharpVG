namespace SharpVG

type image =
    {
        upperLeft: point
        size: area
        source: string
    }

module Image =

    let toTag image =
        {
            name = "image";
            attribute = Some((Point.toDescriptiveString image.upperLeft) + " " + Area.toString image.size);
            body = None
        }

    let toString = toTag >> Tag.toString