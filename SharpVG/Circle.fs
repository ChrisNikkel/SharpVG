namespace SharpVG

type Circle =
    {
        Center : Point
        Radius : Size
        Style : Style option
    }
    
    interface ElementBase with
        member __.name = "circle"