namespace SharpVG

type Ellipse =
    {
        Center : Point
        Radius : Point
        Style : Style option
    }
    
    interface ElementBase with
        member __.name = "ellipse"