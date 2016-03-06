namespace SharpVG

type Polyline =
    {
        Points : seq<Point>
        Style : Style option
    }
    
    interface ElementBase with
        member __.name = "polyline"