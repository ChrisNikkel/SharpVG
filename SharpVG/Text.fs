namespace SharpVG

type Text =
    {
        UpperLeft : Point
        Body : string
        Style : Style option
    }
    interface ElementBase with
        member __.name = "text"