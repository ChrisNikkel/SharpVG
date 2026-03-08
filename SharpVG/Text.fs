namespace SharpVG

type FontWeight =
    | NormalWeight
    | BoldWeight
    | BolderWeight
    | LighterWeight
    | NumericWeight of int
with
    override this.ToString() =
        match this with
        | NormalWeight -> "normal"
        | BoldWeight -> "bold"
        | BolderWeight -> "bolder"
        | LighterWeight -> "lighter"
        | NumericWeight n -> string n

type FontStyle =
    | NormalStyle
    | ItalicStyle
    | ObliqueStyle
with
    override this.ToString() =
        match this with
        | NormalStyle -> "normal"
        | ItalicStyle -> "italic"
        | ObliqueStyle -> "oblique"

type TextAnchor =
    | Start
    | Middle
    | End
    | Inherit

type TextDecoration =
    | Underline
    | StrikeThrough

type WritingMode =
    | HorizontalTopToBottom
    | VerticalRightToLeft
    | VerticalLeftToRight

type Text =
    {
        Position: Point
        Body: string
        FontFamily: string option
        FontSize: float option
        Anchor: TextAnchor option
        LetterSpacing: float option
        Decoration: TextDecoration option
        WritingMode: WritingMode option
        FontWeight: FontWeight option
        FontStyle: FontStyle option
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
                    match text.Decoration with
                        | Some(Underline) -> [Attribute.createXML "text-decoration" "underline"]
                        | Some(StrikeThrough) -> [Attribute.createXML "text-decoration" "line-through"]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.LetterSpacing with
                        | Some(letterSpacing) -> [Attribute.createXML "letter-spacing" (string letterSpacing)]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.WritingMode with
                        | Some(HorizontalTopToBottom) -> [Attribute.createXML "writing-mode" "horizontal-tb"]
                        | Some(VerticalRightToLeft) -> [Attribute.createXML "writing-mode" "vertical-rl"]
                        | Some(VerticalLeftToRight) -> [Attribute.createXML "writing-mode" "vertical-left"]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.FontWeight with
                        | Some(fw) -> [Attribute.createXML "font-weight" (fw.ToString())]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.FontStyle with
                        | Some(fs) -> [Attribute.createXML "font-style" (fs.ToString())]
                        | None -> []
            )
        |> Tag.addBody text.Body

    override this.ToString() =
        this |> Text.ToTag |> Tag.toString

module Text =
    let create position body =
        { Position = position; Body = body; FontFamily = None; FontSize = None; Anchor = None; LetterSpacing = None; Decoration = None; WritingMode = None; FontWeight = None; FontStyle = None }

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

    let withWritingMode writingMode text =
        { text with WritingMode = Some(writingMode) }

    let withDecoration decoration text =
        { text with Decoration = Some(decoration) }

    let withFontWeight fontWeight text =
        { text with FontWeight = Some(fontWeight) }

    let withFontStyle fontStyle text =
        { text with FontStyle = Some(fontStyle) }

    let toTag =
        Text.ToTag

    let toString (text : Text) =
        text.ToString()
