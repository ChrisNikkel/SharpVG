namespace SharpVG

type TextAnchor =
    | Start
    | Middle
    | End
    | Inherit

type Text =
    {
        UpperLeft: Point
        Body: string
        FontFamily: string option
        FontSize: int option
        Anchor: TextAnchor option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Text =
    let create upperLeft body =
        { UpperLeft = upperLeft; Body = body; FontFamily = None; FontSize = None; Anchor = None }

    let withFont family size text =
        { text with FontFamily = Some(family); FontSize = Some(size) }

    let withFontFamily family text =
        { text with FontFamily = Some(family) }

    let withFontSize size text =
        { text with FontSize = Some(size) }

    let withAnchor anchor text =
        { text with Anchor = Some(anchor) }

    let toTag text =
        Tag.create "text"
        |> Tag.withAttributes (Point.toAttributes text.UpperLeft)
        |> Tag.addAttributes
            (
                    match text.FontFamily with
                        | Some(family) -> [Attribute.createXML "font-family" family]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.FontSize with
                        | Some(size) -> [Attribute.createXML "font-size" (string size)]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.Anchor with
                        | Some(Start) -> [Attribute.createXML "text-anchor" "start"]
                        | Some(Middle) -> [Attribute.createXML "text-anchor" "middle"]
                        | Some(End) -> [Attribute.createXML "text-anchor" "end"]
                        | Some(Inherit) -> [Attribute.createXML "text-anchor" "inherit"]
                        | None -> []
            )

    let toString text = text |> toTag |> Tag.toString