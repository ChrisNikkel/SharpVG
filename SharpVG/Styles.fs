namespace SharpVG

module Styles =
    let toTag styles =
        {
            Name = "style";
            Attribute = Some("type=" + (Tag.quote "text/css"));
            Body = Some("<![CDATA[" + (styles |> Seq.map Style.toString |> String.concat "") + "]]>")
        }

    let toString (styles:seq<style>) =
        styles |> toTag |> Tag.toString
