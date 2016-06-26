namespace SharpVG

module Styles =
    let toTag styles =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.create "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (styles |> Seq.map NamedStyle.toCssString |> String.concat " ") + "]]>")

    let toString (styles:seq<NamedStyle>) =
        styles |> toTag |> Tag.toString
