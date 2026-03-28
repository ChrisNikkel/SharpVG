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

type TextBaseline =
    | AutoBaseline
    | HangingBaseline
    | MiddleBaseline
    | CentralBaseline
    | AlphabeticBaseline
    | IdeographicBaseline
    | TextBottomBaseline
    | TextTopBaseline
with
    override this.ToString() =
        match this with
        | AutoBaseline -> "auto"
        | HangingBaseline -> "hanging"
        | MiddleBaseline -> "middle"
        | CentralBaseline -> "central"
        | AlphabeticBaseline -> "alphabetic"
        | IdeographicBaseline -> "ideographic"
        | TextBottomBaseline -> "text-bottom"
        | TextTopBaseline -> "text-top"

type FontVariant =
    | NormalVariant
    | SmallCaps
with
    override this.ToString() =
        match this with
        | NormalVariant -> "normal"
        | SmallCaps -> "small-caps"

type LengthAdjust =
    | Spacing
    | SpacingAndGlyphs
with
    override this.ToString() =
        match this with
        | Spacing -> "spacing"
        | SpacingAndGlyphs -> "spacingAndGlyphs"

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

type TSpan =
    {
        Body: string
        Position: Point option
        Offset: Point option
        FontFamily: string option
        FontSize: float option
        FontWeight: FontWeight option
        FontStyle: FontStyle option
        FontVariant: FontVariant option
        Baseline: TextBaseline option
    }
with
    static member ToTag span =
        Tag.create "tspan"
        |> (match span.Position with Some p -> Tag.addAttributes (Point.toAttributes p) | None -> id)
        |> (match span.Offset with Some p -> Tag.addAttributes (Point.toAttributesWithModifier "d" "" p) | None -> id)
        |> (match span.FontFamily with Some f -> Tag.addAttributes [Attribute.createXML "font-family" f] | None -> id)
        |> (match span.FontSize with Some s -> Tag.addAttributes [Attribute.createXML "font-size" (string s)] | None -> id)
        |> (match span.FontWeight with Some fw -> Tag.addAttributes [Attribute.createXML "font-weight" (fw.ToString())] | None -> id)
        |> (match span.FontStyle with Some fs -> Tag.addAttributes [Attribute.createXML "font-style" (fs.ToString())] | None -> id)
        |> (match span.FontVariant with Some fv -> Tag.addAttributes [Attribute.createXML "font-variant" (fv.ToString())] | None -> id)
        |> (match span.Baseline with Some b -> Tag.addAttributes [Attribute.createXML "dominant-baseline" (b.ToString())] | None -> id)
        |> Tag.addBody span.Body
    override this.ToString() = this |> TSpan.ToTag |> Tag.toString

type TextPathMethod =
    | AlignMethod
    | StretchMethod
with
    override this.ToString() =
        match this with
        | AlignMethod -> "align"
        | StretchMethod -> "stretch"

type TextPathSpacing =
    | AutoSpacing
    | ExactSpacing
with
    override this.ToString() =
        match this with
        | AutoSpacing -> "auto"
        | ExactSpacing -> "exact"

type TextPath =
    {
        Href: ElementId
        Body: string
        StartOffset: Length option
        Method: TextPathMethod option
        Spacing: TextPathSpacing option
        TextLength: Length option
        LengthAdjust: LengthAdjust option
    }
with
    static member ToTag textPath =
        Tag.create "textPath"
        |> Tag.withAttribute (Attribute.createXML "href" ("#" + textPath.Href))
        |> (match textPath.StartOffset with Some o -> Tag.addAttributes [Attribute.createXML "startOffset" (Length.toString o)] | None -> id)
        |> (match textPath.Method with Some m -> Tag.addAttributes [Attribute.createXML "method" (m.ToString())] | None -> id)
        |> (match textPath.Spacing with Some s -> Tag.addAttributes [Attribute.createXML "spacing" (s.ToString())] | None -> id)
        |> (match textPath.TextLength with Some tl -> Tag.addAttributes [Attribute.createXML "textLength" (Length.toString tl)] | None -> id)
        |> (match textPath.LengthAdjust with Some la -> Tag.addAttributes [Attribute.createXML "lengthAdjust" (la.ToString())] | None -> id)
        |> Tag.addBody textPath.Body
    override this.ToString() = this |> TextPath.ToTag |> Tag.toString

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
        FontVariant: FontVariant option
        Baseline: TextBaseline option
        AlignmentBaseline: TextBaseline option
        WordSpacing: float option
        TextLength: Length option
        LengthAdjust: LengthAdjust option
        Spans: TSpan list
        TextPaths: TextPath list
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
                        | Some(VerticalLeftToRight) -> [Attribute.createXML "writing-mode" "vertical-lr"]
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
        |> Tag.addAttributes
            (
                    match text.FontVariant with
                        | Some(fv) -> [Attribute.createXML "font-variant" (fv.ToString())]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.Baseline with
                        | Some(b) -> [Attribute.createXML "dominant-baseline" (b.ToString())]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.AlignmentBaseline with
                        | Some(b) -> [Attribute.createXML "alignment-baseline" (b.ToString())]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.WordSpacing with
                        | Some(ws) -> [Attribute.createXML "word-spacing" (string ws)]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.TextLength with
                        | Some(tl) -> [Attribute.createXML "textLength" (Length.toString tl)]
                        | None -> []
            )
        |> Tag.addAttributes
            (
                    match text.LengthAdjust with
                        | Some(la) -> [Attribute.createXML "lengthAdjust" (la.ToString())]
                        | None -> []
            )
        |> Tag.addBody text.Body
        |> (if text.Spans |> List.isEmpty then id
            else Tag.addBody (text.Spans |> List.map (fun s -> s.ToString()) |> String.concat ""))
        |> (if text.TextPaths |> List.isEmpty then id
            else Tag.addBody (text.TextPaths |> List.map (fun tp -> tp.ToString()) |> String.concat ""))

    override this.ToString() =
        this |> Text.ToTag |> Tag.toString

module Text =
    let create position body =
        { Position = position; Body = body; FontFamily = None; FontSize = None; Anchor = None; LetterSpacing = None; Decoration = None; WritingMode = None; FontWeight = None; FontStyle = None; FontVariant = None; Baseline = None; AlignmentBaseline = None; WordSpacing = None; TextLength = None; LengthAdjust = None; Spans = []; TextPaths = [] }

    let withFont family size (text: Text) =
        { text with FontFamily = Option.ofObj family; FontSize = Some(size) }

    let withFontFamily family (text: Text) =
        { text with FontFamily = Option.ofObj family }

    let withFontSize size (text: Text) =
        { text with FontSize = Some(size) }

    let withAnchor anchor text =
        { text with Anchor = Some(anchor) }

    let withLetterSpacing letterSpacing text =
        { text with LetterSpacing = Some(letterSpacing) }

    let withWritingMode writingMode text =
        { text with WritingMode = Some(writingMode) }

    let withDecoration decoration text =
        { text with Decoration = Some(decoration) }

    let withFontWeight fontWeight (text: Text) =
        { text with FontWeight = Some(fontWeight) }

    let withFontStyle fontStyle (text: Text) =
        { text with FontStyle = Some(fontStyle) }

    let withFontVariant fontVariant (text: Text) =
        { text with FontVariant = Some(fontVariant) }

    let withBaseline baseline (text: Text) =
        { text with Baseline = Some(baseline) }

    let withAlignmentBaseline baseline text =
        { text with AlignmentBaseline = Some(baseline) }

    let withWordSpacing spacing text =
        { text with WordSpacing = Some(spacing) }

    let withTextLength length (text: Text) =
        { text with TextLength = Some(length) }

    let withLengthAdjust adjust (text: Text) =
        { text with LengthAdjust = Some(adjust) }

    let addSpan span text =
        { text with Spans = text.Spans @ [span] }

    let withSpans spans text =
        { text with Spans = spans }

    let addTextPath textPath (text: Text) =
        { text with TextPaths = text.TextPaths @ [textPath] }

    let withTextPaths textPaths (text: Text) =
        { text with TextPaths = textPaths }

    let toTag =
        Text.ToTag

    let toString (text : Text) =
        text.ToString()

module TSpan =
    let create body =
        { Body = body; Position = None; Offset = None; FontFamily = None; FontSize = None; FontWeight = None; FontStyle = None; FontVariant = None; Baseline = None }

    let withPosition position (span: TSpan) = { span with Position = Some position }
    let withOffset offset (span: TSpan) = { span with Offset = Some offset }
    let withFontFamily family (span: TSpan) = { span with FontFamily = Some family }
    let withFontSize size (span: TSpan) = { span with FontSize = Some size }
    let withFontWeight weight (span: TSpan) = { span with FontWeight = Some weight }
    let withFontStyle style (span: TSpan) = { span with FontStyle = Some style }
    let withFontVariant variant (span: TSpan) = { span with FontVariant = Some variant }
    let withBaseline baseline (span: TSpan) = { span with Baseline = Some baseline }
    let toTag = TSpan.ToTag
    let toString (span: TSpan) = span.ToString()

module TextPath =
    let create href body : TextPath =
        { Href = href; Body = body; StartOffset = None; Method = None; Spacing = None; TextLength = None; LengthAdjust = None }

    let withStartOffset offset (textPath: TextPath) =
        { textPath with StartOffset = Some offset }

    let withMethod method (textPath: TextPath) =
        { textPath with Method = Some method }

    let withSpacing spacing (textPath: TextPath) =
        { textPath with Spacing = Some spacing }

    let withTextLength length (textPath: TextPath) =
        { textPath with TextLength = Some length }

    let withLengthAdjust adjust (textPath: TextPath) =
        { textPath with LengthAdjust = Some adjust }

    let toTag = TextPath.ToTag

    let toString (textPath: TextPath) = textPath.ToString()
