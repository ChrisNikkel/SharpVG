namespace SharpVG

type Image =
    {
        UpperLeft: Point
        Size: Area
        Source: string
    }
    
    interface ElementBase with
        member __.name = "image"