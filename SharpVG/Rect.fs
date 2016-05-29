namespace SharpVG

type rect =
    {
        upperLeft: point
        size: area
    }

module Rect =

    let toTag rect =
        {
            name = "rect";
            attribute = Some(Point.toDescriptiveString rect.upperLeft + " " + Area.toDescriptiveString rect.size)
            body = None
        }

    let toString = toTag >> Tag.toString