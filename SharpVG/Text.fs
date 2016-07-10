namespace SharpVG

type Text =
    {
        UpperLeft: Point
        Body: string
        FontFamily: string option
        FontSize: int option
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Text =
    let create upperLeft body =
        { UpperLeft = upperLeft; Body = body; FontFamily = None; FontSize = None }

    let withFont family size text =
        { text with FontFamily = Some(family); FontSize = Some(size) }

    let withFontFamily family text =
        { text with FontFamily = Some(family) }

    let withFontSize size text =
        { text with FontSize = Some(size) }

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

    let toString text = text |> toTag |> Tag.toString