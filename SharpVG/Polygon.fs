namespace SharpVG

type Polygon =
    {
        Points : seq<Point>
        Style : Style option
    }
    
    interface ElementBase with
        member __.name = "polygon"