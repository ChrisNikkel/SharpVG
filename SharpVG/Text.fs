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
                        | Some(family) -> set [Attribute.create "font-family" family]
                        | None -> set []
            )
        |> Tag.addAttributes
            (
                    match text.FontSize with
                        | Some(size) -> set [Attribute.create "font-size" (string size)]
                        | None -> set []
            )

    let toString text = text |> toTag |> Tag.toString