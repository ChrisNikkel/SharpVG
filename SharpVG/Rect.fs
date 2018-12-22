namespace SharpVG

type Rect =
    {
        UpperLeft: Point
        Size: Area
    }
with
    static member ToTag rect =
        Tag.create "rect"
        |> Tag.withAttributes (Point.toAttributes rect.UpperLeft)
        |> Tag.addAttributes (Area.toAttributes rect.Size)

module Rect =
    let create upperLeft size =
        { UpperLeft = upperLeft; Size = size }

    let toTag =
        Rect.ToTag

    let toString = toTag >> Tag.toString