namespace SharpVG

type Rect =
    {
        UpperLeft: Point
        Size: Area
    }

module Rect =
    let create upperLeft size =
        { UpperLeft = upperLeft; Size = size }

    let toTag rect =
        Tag.create "rect"
        |> Tag.withAttributes (Point.toAttributes rect.UpperLeft)
        |> Tag.addAttributes (Area.toAttributes rect.Size)

    let toString = toTag >> Tag.toString