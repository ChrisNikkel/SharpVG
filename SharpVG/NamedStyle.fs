namespace SharpVG

type NamedStyle = {
    Name : string;
    Style : Style;
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module NamedStyle =
    let ofStyle name style =
        { Name = name; Style = style }

    let toCssString namedStyle =
        "." + namedStyle.Name + "{" + (namedStyle.Style |> Style.toString) + "}"

    let toTag namedStyle =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.create "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (toCssString namedStyle) + "]]>")

    let toString = toTag >> Tag.toString
