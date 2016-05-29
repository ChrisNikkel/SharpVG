namespace SharpVG

type image =
    {
        UpperLeft: point
        Size: area
        Source: string
    }

module Image =

    let init upperLeft size source =
        { UpperLeft = upperLeft; Size = size; Source = source }

    let toTag image =
        {
            Name = "image";
            Attribute = Some((Point.toDescriptiveString image.UpperLeft) + " " + Area.toDescriptiveString image.Size);
            Body = None
        }

    let toString = toTag >> Tag.toString