namespace SharpVG

module Styles =
    let toTag styles =
        {
            Name = "style";
            Attribute = Some("type=" + (Tag.quote "text/css"));
            Body = Some("<![CDATA[" + (styles |> Seq.map NamedStyle.toCssString |> String.concat " ") + "]]>")
        }

    let toString (styles:seq<NamedStyle>) =
        styles |> toTag |> Tag.toString
