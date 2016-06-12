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
        {
            Name = "text";
            Attribute = 
                Some(Point.toDescriptiveString text.UpperLeft +
                    match text.FontFamily with
                        | Some(family) -> " font-family=\"" + family + "\""
                        | None -> ""
                    +
                    match text.FontSize with
                        | Some(size) -> " font-size=\"" + (string size) + "\""
                        | None -> ""
                )

            Body = Some(text.Body)
        }

    let toString text = text |> toTag |> Tag.toString