namespace SharpVG

type rect =
    {
        UpperLeft: point
        Size: area
    }

module Rect =
    let create upperLeft size =
        { UpperLeft = upperLeft; Size = size }

    let toTag rect =
        {
            Name = "rect";
            Attribute = Some(Point.toDescriptiveString rect.UpperLeft + " " + Area.toDescriptiveString rect.Size)
            Body = None
        }

    let toString = toTag >> Tag.toString