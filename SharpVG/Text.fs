namespace SharpVG

type TextAnchor =
    | Start
    | Middle
    | End
    | Inherit

type Text =
    {
        Position: Point
        Body: string
        FontFamily: string option
        FontSize: float option
        Anchor: TextAnchor option
        LetterSpacing: float option
    }
with
    static member ToTag text =
        Tag.create "text"
        |> Tag.withAttributes (Point.toAttributes text.Position)
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
        |> Tag.addAttributes
            (
                    match text.LetterSpacing with
                        | Some(letterSpacing) -> [Attribute.createXML "letter-spacing" (string letterSpacing)]
                        | None -> []
            )
        |> Tag.addBody text.Body

    override this.ToString() =
        this |> Text.ToTag |> Tag.toString

module Text =
    let create position body =
        { Position = position; Body = body; FontFamily = None; FontSize = None; Anchor = None; LetterSpacing = None; }

    let withFont family size text =
        { text with FontFamily = Option.ofObj family; FontSize = Some(size) }

    let withFontFamily family text =
        { text with FontFamily = Some(family) }

    let withFontSize size text =
        { text with FontSize = Some(size) }

    let withAnchor anchor text =
        { text with Anchor = Some(anchor) }

    let withLetterSpacing letterSpacing text =
        { text with LetterSpacing = Some(letterSpacing) }

    let toTag =
        Text.ToTag

    let toString (text : Text) =
        text.ToString()
