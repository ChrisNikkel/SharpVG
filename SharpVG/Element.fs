namespace SharpVG

[<AbstractClass>]
type SvgElement(element : Element, style : Style option) =

    member __.Element = element

    member __.Style =
        match element with
            | StyledElement(_, style) -> Some(style)
            | _ -> None

    abstract member Name : string

    abstract member Body : string option
    default __.Body = None

    abstract member Attributes : string

    abstract member toString : string

    default __.toString =
        let name = __.Name
        let body = __.Body
        let attributes = __.Attributes
        let style = __.Style

        let styledAttributes =
            match style with
                | Some(s) -> attributes + " " + SvgStyle(s).toString
                | _ -> attributes

        match body with
            | Some(body) -> "<" + name + " " + styledAttributes + ">" + body + "</" + name + ">"
            | None -> "<" + name + " " + styledAttributes + ">"

    override __.ToString() = __.toString