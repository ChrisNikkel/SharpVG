namespace SharpVG

type Script =
    {
        Body: string
    }
    member __.toString =
        "<script type=\"application/ecmascript\"><![CDATA[" +
        __.Body +
        "]]></script>"

    override __.ToString() = __.toString

