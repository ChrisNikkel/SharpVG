namespace SharpVG

type Rect =
    {
        UpperLeft : Point
        Size : Area
        Style : Style option
    }
    interface ElementBase with
        member __.name = "rect"