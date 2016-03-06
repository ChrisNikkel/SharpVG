namespace SharpVG

type Line =
    {
        Point1 : Point
        Point2 : Point
        Style : Style option
    }
    
    interface ElementBase with
        member __.name = "line"