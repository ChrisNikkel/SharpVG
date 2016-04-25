namespace SharpVG

type rect =
    {
        upperLeft: point
        size: area
    }

module Rect =
    let toString rect =
        {
            name = "rect";
            attribute = Some(Point.toDescriptiveString rect.upperLeft + " " + Area.toString rect.size)
            body = None
        }
        |> Tag.toString