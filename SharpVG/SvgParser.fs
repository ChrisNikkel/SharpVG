namespace SharpVG

open System
open System.Xml.Linq
open System.Globalization

// --- Public parser types ---

type ParseWarning =
    {
        Message: string
        ElementName: string option
    }

type ParseError =
    {
        Message: string
        ElementName: string option
    }

type ParseResult<'T> =
    {
        Value: 'T
        Warnings: ParseWarning list
    }

// --- Private implementation ---

module private SvgParserHelpers =

    // System.Text.RegularExpressions opened only here — prevents Group type
    // from shadowing SharpVG.Group in downstream modules.
    open System.Text.RegularExpressions

    type ParseState =
        {
            Warnings: ParseWarning list
        }

    let emptyState : ParseState = { Warnings = [] }

    let warnState msg elementName (state: ParseState) : ParseState =
        { state with Warnings = state.Warnings @ [{ Message = msg; ElementName = elementName }] }

    // Active patterns — El returns unit so | El "circle" -> works without a binding variable
    let (|El|_|) name (xel: XElement) =
        if xel.Name.LocalName = name then Some () else None

    let (|FloatAttr|_|) name (xel: XElement) =
        match xel.Attribute(XName.Get name) |> Option.ofObj with
        | Some a ->
            match Double.TryParse(a.Value, NumberStyles.Any, CultureInfo.InvariantCulture) with
            | true, v -> Some v
            | _ -> None
        | None -> None

    // Parsing helpers
    let tryParseFloat (s: string) =
        match Double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture) with
        | true, v -> Some v
        | _ -> None

    let parseFloat (s: string) =
        tryParseFloat s |> Option.defaultValue 0.0

    let tryParseLength (s: string) =
        let s = s.Trim()
        if s.EndsWith("px", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map (int >> Length.ofPixels)
        elif s.EndsWith("%") then
            tryParseFloat (s.[..s.Length - 2]) |> Option.map Length.ofPercent
        elif s.EndsWith("em", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map Length.ofEm
        elif s.EndsWith("cm", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map Length.ofCm
        elif s.EndsWith("mm", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map Length.ofMm
        elif s.EndsWith("in", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map Length.ofIn
        elif s.EndsWith("pt", StringComparison.OrdinalIgnoreCase) then
            tryParseFloat (s.[..s.Length - 3]) |> Option.map Length.ofPt
        else
            tryParseFloat s |> Option.map Length.ofFloat

    let private namedColors =
        dict [
            "aliceblue", Colors.AliceBlue; "antiquewhite", Colors.AntiqueWhite
            "aqua", Colors.Aqua; "aquamarine", Colors.Aquamarine; "azure", Colors.Azure
            "beige", Colors.Beige; "bisque", Colors.Bisque; "black", Colors.Black
            "blanchedalmond", Colors.BlanchedAlmond; "blue", Colors.Blue
            "blueviolet", Colors.BlueViolet; "brown", Colors.Brown; "burlywood", Colors.BurlyWood
            "cadetblue", Colors.CadetBlue; "chartreuse", Colors.Chartreuse
            "chocolate", Colors.Chocolate; "coral", Colors.Coral
            "cornflowerblue", Colors.CornflowerBlue; "cornsilk", Colors.CornSilk
            "crimson", Colors.Crimson; "cyan", Colors.Cyan; "darkblue", Colors.DarkBlue
            "darkcyan", Colors.DarkCyan; "darkgoldenrod", Colors.DarkGoldenRod
            "darkgray", Colors.DarkGray; "darkgreen", Colors.DarkGreen
            "darkgrey", Colors.DarkGrey; "darkkhaki", Colors.DarkKhaki
            "darkmagenta", Colors.DarkMagenta; "darkolivegreen", Colors.DarkOliveGreen
            "darkorange", Colors.DarkOrange; "darkorchid", Colors.DarkOrchid
            "darkred", Colors.DarkRed; "darksalmon", Colors.DarkSalmon
            "darkseagreen", Colors.DarkSeaGreen; "darkslateblue", Colors.DarkSlateBlue
            "darkslategray", Colors.DarkSlateGray; "darkslategrey", Colors.DarkSlateGrey
            "darkturquoise", Colors.DarkTurquoise; "darkviolet", Colors.DarkViolet
            "deeppink", Colors.DeepPink; "deepskyblue", Colors.DeepSkyBlue
            "dimgray", Colors.DimGray; "dimgrey", Colors.DimGrey; "dodgerblue", Colors.DodgerBlue
            "firebrick", Colors.FireBrick; "floralwhite", Colors.FloralWhite
            "forestgreen", Colors.ForestGreen; "fuchsia", Colors.Fuchsia
            "gainsboro", Colors.Gainsboro; "ghostwhite", Colors.GhostWhite; "gold", Colors.Gold
            "goldenrod", Colors.GoldenRod; "gray", Colors.Gray; "grey", Colors.Grey
            "green", Colors.Green; "greenyellow", Colors.GreenYellow; "honeydew", Colors.HoneyDew
            "hotpink", Colors.HotPink; "indianred", Colors.IndianRed; "indigo", Colors.Indigo
            "ivory", Colors.Ivory; "khaki", Colors.Khaki; "lavender", Colors.Lavender
            "lavenderblush", Colors.LavenderBlush; "lawngreen", Colors.LawnGreen
            "lemonchiffon", Colors.LemonChiffon; "lightblue", Colors.LightBlue
            "lightcoral", Colors.LightCoral; "lightcyan", Colors.LightCyan
            "lightgoldenrodyellow", Colors.LightGoldenRodYellow; "lightgray", Colors.LightGray
            "lightgreen", Colors.LightGreen; "lightgrey", Colors.LightGrey
            "lightpink", Colors.LightPink; "lightsalmon", Colors.LightSalmon
            "lightseagreen", Colors.LightSeaGreen; "lightskyblue", Colors.LightSkyBlue
            "lightslategray", Colors.LightSlateGray; "lightslategrey", Colors.LightSlateGrey
            "lightsteelblue", Colors.LightSteelBlue; "lightyellow", Colors.LightYellow
            "lime", Colors.Lime; "limegreen", Colors.LimeGreen; "linen", Colors.Linen
            "magenta", Colors.Magenta; "maroon", Colors.Maroon
            "mediumaquamarine", Colors.MediumAquamarine; "mediumblue", Colors.MediumBlue
            "mediumorchid", Colors.MediumOrchid; "mediumpurple", Colors.MediumPurple
            "mediumseagreen", Colors.MediumSeaGreen; "mediumslateblue", Colors.MediumSlateBlue
            "mediumspringgreen", Colors.MediumSpringGreen; "mediumturquoise", Colors.MediumTurquoise
            "mediumvioletred", Colors.MediumVioletRed; "midnightblue", Colors.MidnightBlue
            "mintcream", Colors.MintCream; "mistyrose", Colors.MistyRose; "moccasin", Colors.Moccasin
            "navajowhite", Colors.NavajoWhite; "navy", Colors.Navy; "oldlace", Colors.OldLace
            "olive", Colors.Olive; "olivedrab", Colors.OliveDrab; "orange", Colors.Orange
            "orangered", Colors.OrangeRed; "orchid", Colors.Orchid
            "palegoldenrod", Colors.PaleGoldenRod; "palegreen", Colors.PaleGreen
            "paleturquoise", Colors.PaleTurquoise; "palevioletred", Colors.PaleVioletRed
            "papayawhip", Colors.PapayaWhip; "peachpuff", Colors.PeachPuff; "peru", Colors.Peru
            "pink", Colors.Pink; "plum", Colors.Plum; "powderblue", Colors.PowderBlue
            "purple", Colors.Purple; "red", Colors.Red; "rosybrown", Colors.RosyBrown
            "royalblue", Colors.RoyalBlue; "saddlebrown", Colors.SaddleBrown; "salmon", Colors.Salmon
            "sandybrown", Colors.SandyBrown; "seagreen", Colors.SeaGreen
            "seashell", Colors.SeaShell; "sienna", Colors.Sienna; "silver", Colors.Silver
            "skyblue", Colors.SkyBlue; "slateblue", Colors.SlateBlue; "slategray", Colors.SlateGray
            "slategrey", Colors.SlateGrey; "snow", Colors.Snow; "springgreen", Colors.SpringGreen
            "steelblue", Colors.SteelBlue; "tan", Colors.Tan; "teal", Colors.Teal
            "thistle", Colors.Thistle; "tomato", Colors.Tomato; "turquoise", Colors.Turquoise
            "violet", Colors.Violet; "wheat", Colors.Wheat; "white", Colors.White
            "whitesmoke", Colors.WhiteSmoke; "yellow", Colors.Yellow; "yellowgreen", Colors.YellowGreen
        ]

    let tryParseColor (s: string) : Color option =
        let s = s.Trim()
        if s = "none" || s = "transparent" then
            None
        elif s = "currentColor" then
            Some Color.currentColor
        elif s.StartsWith("url(#") && s.EndsWith(")") then
            Some (Color.ofUrl (s.[5..s.Length - 2]))
        elif s.StartsWith("#") then
            let hex = s.[1..]
            match hex.Length with
            | 3 ->
                match Int32.TryParse(hex, NumberStyles.HexNumber, null) with
                | true, v -> Some (Color.ofSmallHex (int16 v))
                | _ -> None
            | 6 ->
                match Int32.TryParse(hex, NumberStyles.HexNumber, null) with
                | true, v -> Some (Color.ofHex v)
                | _ -> None
            | _ -> None
        elif s.StartsWith("rgb(") && s.EndsWith(")") then
            let inner = s.[4..s.Length - 2]
            let parts = inner.Split(',') |> Array.map (fun p -> p.Trim())
            match parts with
            | [|r; g; b|] ->
                match Byte.TryParse(r), Byte.TryParse(g), Byte.TryParse(b) with
                | (true, rv), (true, gv), (true, bv) -> Some (Color.ofValues (rv, gv, bv))
                | _ -> None
            | _ -> None
        else
            let key = s.ToLowerInvariant()
            match namedColors.TryGetValue(key) with
            | true, c -> Some (Color.ofName c)
            | _ -> None

    let splitNumbers (s: string) : float list =
        s.Split([|','; ' '; '\t'; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.choose tryParseFloat
        |> Array.toList

    let private tryParseCssProperty (name: string) (value: string) (style: Style) : Style option =
        match name.Trim() with
        | "fill" ->
            match tryParseColor value with
            | Some c -> Some { style with Fill = Some c }
            | None -> Some { style with Fill = None }
        | "stroke" ->
            match tryParseColor value with
            | Some c -> Some { style with Stroke = Some c }
            | None -> Some { style with Stroke = None }
        | "stroke-width" ->
            tryParseLength value |> Option.map (fun l -> { style with StrokeWidth = Some l })
        | "opacity" ->
            tryParseFloat value |> Option.map (fun f -> { style with Opacity = Some f })
        | "fill-opacity" ->
            tryParseFloat value |> Option.map (fun f -> { style with FillOpacity = Some f })
        | "fill-rule" ->
            match value.Trim() with
            | "nonzero" -> Some { style with FillRule = Some NonZero }
            | "evenodd" -> Some { style with FillRule = Some EvenOdd }
            | _ -> None
        | "stroke-linecap" ->
            match value.Trim() with
            | "butt" -> Some { style with StrokeLinecap = Some ButtLinecap }
            | "round" -> Some { style with StrokeLinecap = Some RoundLinecap }
            | "square" -> Some { style with StrokeLinecap = Some SquareLinecap }
            | _ -> None
        | "stroke-linejoin" ->
            match value.Trim() with
            | "miter" -> Some { style with StrokeLinejoin = Some MiterLinejoin }
            | "round" -> Some { style with StrokeLinejoin = Some RoundLinejoin }
            | "bevel" -> Some { style with StrokeLinejoin = Some BevelLinejoin }
            | _ -> None
        | "stroke-dasharray" ->
            let nums = splitNumbers value
            if nums.IsEmpty then None
            else Some { style with StrokeDashArray = Some nums }
        | "stroke-dashoffset" ->
            tryParseFloat value |> Option.map (fun f -> { style with StrokeDashOffset = Some f })
        | "visibility" ->
            match value.Trim() with
            | "visible" -> Some { style with Visibility = Some Visible }
            | "hidden" -> Some { style with Visibility = Some Hidden }
            | "collapse" -> Some { style with Visibility = Some Collapse }
            | _ -> None
        | "display" ->
            match value.Trim() with
            | "none" -> Some { style with Display = Some DisplayNone }
            | _ -> Some { style with Display = Some Inline }
        | _ -> None

    let tryStyle (xel: XElement) : Style option =
        let presentationAttrs =
            [ "fill"; "stroke"; "stroke-width"; "opacity"; "fill-opacity"; "fill-rule"
              "stroke-linecap"; "stroke-linejoin"; "stroke-dasharray"; "stroke-dashoffset"
              "visibility"; "display" ]

        let fromPresentation =
            presentationAttrs
            |> List.fold (fun s name ->
                match xel.Attribute(XName.Get name) |> Option.ofObj with
                | Some attr ->
                    match tryParseCssProperty name attr.Value s with
                    | Some updated -> updated
                    | None -> s
                | None -> s) Style.empty

        let merged =
            match xel.Attribute(XName.Get "style") |> Option.ofObj with
            | Some styleAttr ->
                styleAttr.Value.Split(';')
                |> Array.fold (fun s decl ->
                    match decl.Split([|':'|], 2) with
                    | [|name; value|] ->
                        match tryParseCssProperty name value s with
                        | Some updated -> updated
                        | None -> s
                    | _ -> s) fromPresentation
            | None -> fromPresentation

        if merged = Style.empty then None else Some merged

    let tryTransforms (xel: XElement) : Transform seq =
        match xel.Attribute(XName.Get "transform") |> Option.ofObj with
        | None -> Seq.empty
        | Some attr ->
            Regex.Matches(attr.Value, @"(\w+)\(([^)]*)\)")
            |> Seq.cast<Match>
            |> Seq.choose (fun m ->
                let name = m.Groups.[1].Value
                let args =
                    m.Groups.[2].Value.Split([|','; ' '; '\t'|], StringSplitOptions.RemoveEmptyEntries)
                    |> Array.choose tryParseFloat
                match name, args with
                | "translate", [|x|] ->
                    Some (Transform.createTranslate (Length.ofFloat x))
                | "translate", [|x; y|] ->
                    Some (Transform.createTranslate (Length.ofFloat x) |> Transform.withY (Length.ofFloat y))
                | "scale", [|x|] ->
                    Some (Transform.createScale (Length.ofFloat x))
                | "scale", [|x; y|] ->
                    Some (Transform.createScale (Length.ofFloat x) |> Transform.withY (Length.ofFloat y))
                | "rotate", [|a|] ->
                    Some (Transform.Rotate (a, None))
                | "rotate", [|a; cx; cy|] ->
                    Some (Transform.createRotate a (Length.ofFloat cx) (Length.ofFloat cy))
                | "skewX", [|a|] ->
                    Some (Transform.createSkewX a)
                | "skewY", [|a|] ->
                    Some (Transform.createSkewY a)
                | "matrix", [|a; b; c; d; e; f|] ->
                    Some (Transform.Matrix (Length.ofFloat a, Length.ofFloat b, Length.ofFloat c, Length.ofFloat d, Length.ofFloat e, Length.ofFloat f))
                | _ -> None)

    let applyCommon (xel: XElement) (element: Element) : Element =
        let withId =
            match xel.Attribute(XName.Get "id") |> Option.ofObj with
            | Some attr -> element |> Element.withName attr.Value
            | None -> element
        let withClass =
            match xel.Attribute(XName.Get "class") |> Option.ofObj with
            | Some attr ->
                let classes = attr.Value.Split(' ') |> Array.filter (fun s -> s <> "") |> Array.toSeq
                { withId with Classes = classes }
            | None -> withId
        let withStyled =
            match tryStyle xel with
            | Some s -> { withClass with Style = Some s }
            | None -> withClass
        let transforms = tryTransforms xel
        if Seq.isEmpty transforms then withStyled
        else { withStyled with Transforms = transforms }

    let tokenizePathData (d: string) : string list =
        Regex.Matches(d, @"[MmZzLlHhVvCcSsQqTtAa]|[-+]?(?:[0-9]+\.?[0-9]*|\.?[0-9]+)(?:[eE][-+]?[0-9]+)?")
        |> Seq.cast<Match>
        |> Seq.map _.Value
        |> Seq.toList

    let parsePathData (d: string) : Path =
        let tokens = tokenizePathData d
        let asLength s = Length.ofFloat (parseFloat s)
        let asPoint sx sy = Point.ofFloats (parseFloat sx, parseFloat sy)
        let isCmd (s: string) = Char.IsLetter s.[0]

        let rec takeNumbers acc remaining =
            match remaining with
            | t :: rest when not (isCmd t) -> takeNumbers (acc @ [t]) rest
            | _ -> (acc, remaining)

        let rec consume (tokens: string list) (path: Path) : Path =
            match tokens with
            | [] -> path
            | cmd :: rest when isCmd cmd ->
                let positioning = if Char.IsUpper cmd.[0] then Absolute else Relative
                let (nums, remaining) = takeNumbers [] rest
                let updated =
                    match cmd.ToUpperInvariant() with
                    | "Z" ->
                        Path.addClosePath path
                    | "M" ->
                        nums |> List.chunkBySize 2 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x; y] -> Path.addMoveTo positioning (asPoint x y) p
                            | _ -> p) path
                    | "L" ->
                        nums |> List.chunkBySize 2 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x; y] -> Path.addLineTo positioning (asPoint x y) p
                            | _ -> p) path
                    | "H" ->
                        nums |> List.fold (fun p n -> Path.addHorizontalLineTo positioning (asLength n) p) path
                    | "V" ->
                        nums |> List.fold (fun p n -> Path.addVerticalLineTo positioning (asLength n) p) path
                    | "C" ->
                        nums |> List.chunkBySize 6 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x1; y1; x2; y2; x; y] ->
                                Path.addCubicBezierCurveTo positioning (asPoint x1 y1) (asPoint x2 y2) (asPoint x y) p
                            | _ -> p) path
                    | "S" ->
                        nums |> List.chunkBySize 4 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x2; y2; x; y] ->
                                Path.addSmoothCubicBezierCurveTo positioning (asPoint x2 y2) (asPoint x y) p
                            | _ -> p) path
                    | "Q" ->
                        nums |> List.chunkBySize 4 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x1; y1; x; y] ->
                                Path.addQuadraticBezierCurveTo positioning (asPoint x1 y1) (asPoint x y) p
                            | _ -> p) path
                    | "T" ->
                        nums |> List.chunkBySize 2 |> List.fold (fun p chunk ->
                            match chunk with
                            | [x; y] -> Path.addSmoothQuadraticBezierCurveTo positioning (asPoint x y) p
                            | _ -> p) path
                    | "A" ->
                        nums |> List.chunkBySize 7 |> List.fold (fun p chunk ->
                            match chunk with
                            | [rx; ry; rot; largeArc; sweep; x; y] ->
                                Path.addEllipticalArcCurveTo positioning
                                    (asPoint rx ry) (parseFloat rot)
                                    (largeArc = "1") (sweep = "1")
                                    (asPoint x y) p
                            | _ -> p) path
                    | _ -> path
                consume remaining updated
            | _ :: rest -> consume rest path

        consume tokens Path.empty


module private SvgElementParsers =

    open SvgParserHelpers

    let private xhref (xel: XElement) =
        let href = xel.Attribute(XName.Get "href") |> Option.ofObj
        let xlinkHref = xel.Attribute(XName.Get("{http://www.w3.org/1999/xlink}href")) |> Option.ofObj
        (href |> Option.orElse xlinkHref) |> Option.map _.Value

    let parseCircle (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        match xel with
        | FloatAttr "r" r ->
            let cx = match xel with FloatAttr "cx" v -> v | _ -> 0.0
            let cy = match xel with FloatAttr "cy" v -> v | _ -> 0.0
            let center = Point.ofFloats (cx, cy)
            let radius = Length.ofFloat r
            let element = Circle.create center radius |> Element.create |> applyCommon xel
            GroupElement.Element element, state
        | _ ->
            let warned = warnState "circle missing required r attribute" (Some "circle") state
            let element = Circle.create Point.origin Length.empty |> Element.create |> applyCommon xel
            GroupElement.Element element, warned

    let parseEllipse (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let cx = match xel with FloatAttr "cx" v -> v | _ -> 0.0
        let cy = match xel with FloatAttr "cy" v -> v | _ -> 0.0
        let rx = match xel with FloatAttr "rx" v -> v | _ -> 0.0
        let ry = match xel with FloatAttr "ry" v -> v | _ -> 0.0
        let center = Point.ofFloats (cx, cy)
        let element = Ellipse.create center (Point.ofFloats (rx, ry)) |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parseRect (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let x = match xel with FloatAttr "x" v -> v | _ -> 0.0
        let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
        let w = match xel with FloatAttr "width" v -> v | _ -> 0.0
        let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
        let position = Point.ofFloats (x, y)
        let area = Area.ofFloats (w, h)
        let element = Rect.create position area |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parseLine (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let x1 = match xel with FloatAttr "x1" v -> v | _ -> 0.0
        let y1 = match xel with FloatAttr "y1" v -> v | _ -> 0.0
        let x2 = match xel with FloatAttr "x2" v -> v | _ -> 0.0
        let y2 = match xel with FloatAttr "y2" v -> v | _ -> 0.0
        let startPoint = Point.ofFloats (x1, y1)
        let endPoint = Point.ofFloats (x2, y2)
        let element = Line.create startPoint endPoint |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parsePath (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        match xel.Attribute(XName.Get "d") |> Option.ofObj with
        | Some dAttr ->
            let path = parsePathData dAttr.Value
            let element = path |> Element.create |> applyCommon xel
            GroupElement.Element element, state
        | None ->
            let warned = warnState "path missing required d attribute" (Some "path") state
            let element = Path.empty |> Element.create |> applyCommon xel
            GroupElement.Element element, warned

    let parsePolygon (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        match xel.Attribute(XName.Get "points") |> Option.ofObj with
        | Some pAttr ->
            let nums = splitNumbers pAttr.Value
            let points =
                nums |> List.chunkBySize 2
                |> List.choose (function
                    | [x; y] -> Some (Point.ofFloats (x, y))
                    | _ -> None)
            let element = Polygon.create points |> Element.create |> applyCommon xel
            GroupElement.Element element, state
        | None ->
            let warned = warnState "polygon missing points attribute" (Some "polygon") state
            let element = Polygon.create [] |> Element.create |> applyCommon xel
            GroupElement.Element element, warned

    let parsePolyline (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        match xel.Attribute(XName.Get "points") |> Option.ofObj with
        | Some pAttr ->
            let nums = splitNumbers pAttr.Value
            let points =
                nums |> List.chunkBySize 2
                |> List.choose (function
                    | [x; y] -> Some (Point.ofFloats (x, y))
                    | _ -> None)
            let element = Polyline.create points |> Element.create |> applyCommon xel
            GroupElement.Element element, state
        | None ->
            let warned = warnState "polyline missing points attribute" (Some "polyline") state
            let element = Polyline.create [] |> Element.create |> applyCommon xel
            GroupElement.Element element, warned

    let parseText (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let x = match xel with FloatAttr "x" v -> v | _ -> 0.0
        let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
        let position = Point.ofFloats (x, y)
        let body = xel.Value
        let element = Text.create position body |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parseImage (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let x = match xel with FloatAttr "x" v -> v | _ -> 0.0
        let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
        let w = match xel with FloatAttr "width" v -> v | _ -> 0.0
        let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
        let position = Point.ofFloats (x, y)
        let area = Area.ofFloats (w, h)
        let href = xhref xel |> Option.defaultValue ""
        let element = Image.create position area href |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parseUse (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let href = xhref xel |> Option.defaultValue ""
        let id = if href.StartsWith("#") then href.[1..] else href
        let x = match xel with FloatAttr "x" v -> v | _ -> 0.0
        let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
        let position = Point.ofFloats (x, y)
        let useObj : Use = { Name = id; Position = position; Size = None }
        let element = useObj |> Element.create |> applyCommon xel
        GroupElement.Element element, state

    let parseRaw (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let attributes =
            xel.Attributes()
            |> Seq.map (fun a -> Attribute.createXML a.Name.LocalName a.Value)
            |> Seq.toList
        let children =
            xel.Nodes()
            |> Seq.choose (fun node ->
                match node with
                | :? XElement as child ->
                    let childAttrs =
                        child.Attributes()
                        |> Seq.map (fun a -> Attribute.createXML a.Name.LocalName a.Value)
                        |> Seq.toList
                    Some (RawChild (RawElement.create child.Name.LocalName childAttrs []))
                | :? XText as text ->
                    let trimmed = text.Value.Trim()
                    if trimmed = "" then None else Some (RawText trimmed)
                | _ -> None)
            |> Seq.toList
        let raw = RawElement.create xel.Name.LocalName attributes children
        GroupElement.Raw raw, state

    let rec parseGroup (xel: XElement) (state: ParseState) : SharpVG.Group * ParseState =
        let (children, finalState) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)

        let name =
            xel.Attribute(XName.Get "id") |> Option.ofObj |> Option.map _.Value

        let transforms = tryTransforms xel

        let group =
            {
                Group.empty with
                    Name = name
                    Body = children |> Seq.ofList
                    Transforms = transforms
            }

        group, finalState

    and parseElement (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        match xel with
        | El "circle"   -> parseCircle xel state
        | El "ellipse"  -> parseEllipse xel state
        | El "rect"     -> parseRect xel state
        | El "line"     -> parseLine xel state
        | El "path"     -> parsePath xel state
        | El "polygon"  -> parsePolygon xel state
        | El "polyline" -> parsePolyline xel state
        | El "text"     -> parseText xel state
        | El "image"    -> parseImage xel state
        | El "use"      -> parseUse xel state
        | El "g"        ->
            let (group, s2) = parseGroup xel state
            GroupElement.Group group, s2
        | _             -> parseRaw xel state

    let parseDefs (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState =
        xel.Elements()
        |> Seq.fold (fun (defs, s) child ->
            match child with
            | El "g" ->
                let (group, s2) = parseGroup child s
                SvgDefinitions.addGroup group defs, s2
            | El "circle" | El "ellipse" | El "rect" | El "line"
            | El "path"   | El "polygon" | El "polyline" | El "text"
            | El "image"  | El "use" ->
                let (ge, s2) = parseElement child s
                match ge with
                | GroupElement.Element e -> SvgDefinitions.addElement e defs, s2
                | _ -> defs, s2
            | _ ->
                let attrs =
                    child.Attributes()
                    |> Seq.map (fun a -> Attribute.createXML a.Name.LocalName a.Value)
                    |> Seq.toList
                let raw = RawElement.create child.Name.LocalName attrs []
                let contents = Seq.append defs.Contents (Seq.singleton (RawDef raw))
                { defs with Contents = contents }, s)
            (SvgDefinitions.create, state)


module private SvgRootParser =

    open SvgParserHelpers

    let private tryParseViewBox (s: string) : ViewBox option =
        let nums =
            s.Split([|','; ' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.choose (fun t ->
                match Double.TryParse(t, NumberStyles.Any, CultureInfo.InvariantCulture) with
                | true, v -> Some v
                | _ -> None)
        match nums with
        | [|minX; minY; width; height|] ->
            let origin = Point.ofFloats (minX, minY)
            let size = Area.ofFloats (width, height)
            Some (ViewBox.create origin size)
        | _ -> None

    let parseSvgRoot (root: XElement) : Result<ParseResult<Svg>, ParseError> =
        let state = emptyState

        // Separate <defs> from body elements — defs go into SvgDefinitions, rest into Body
        let defsElements = root.Elements() |> Seq.filter (fun e -> e.Name.LocalName = "defs") |> Seq.toList
        let bodyElements = root.Elements() |> Seq.filter (fun e -> e.Name.LocalName <> "defs") |> Seq.toList

        let (definitions, stateAfterDefs) =
            defsElements
            |> List.fold (fun (defs, s) defsEl ->
                let (parsed, s2) = SvgElementParsers.parseDefs defsEl s
                let merged =
                    { defs with Contents = Seq.append defs.Contents parsed.Contents }
                merged, s2) (SvgDefinitions.create, state)

        let (children, finalState) =
            bodyElements
            |> List.fold (fun (acc, s) child ->
                let (ge, s2) = SvgElementParsers.parseElement child s
                (acc @ [ge], s2)) ([], stateAfterDefs)

        let width =
            root.Attribute(XName.Get "width") |> Option.ofObj
            |> Option.bind (fun a -> tryParseLength a.Value)

        let height =
            root.Attribute(XName.Get "height") |> Option.ofObj
            |> Option.bind (fun a -> tryParseLength a.Value)

        let size =
            match width, height with
            | Some w, Some h -> Some (Area.create w h)
            | _ -> None

        let viewBox =
            root.Attribute(XName.Get "viewBox") |> Option.ofObj
            |> Option.bind (fun a -> tryParseViewBox a.Value)

        let title =
            root.Elements()
            |> Seq.tryFind (fun e -> e.Name.LocalName = "title")
            |> Option.map _.Value

        let description =
            root.Elements()
            |> Seq.tryFind (fun e -> e.Name.LocalName = "desc")
            |> Option.map _.Value

        let defsOption =
            if Seq.isEmpty definitions.Contents then None else Some definitions

        let svg =
            {
                Body = children |> Seq.ofList
                Definitions = defsOption
                Size = size
                ViewBox = viewBox
                PreserveAspectRatio = None
                Title = title
                Description = description
            }

        Ok { Value = svg; Warnings = finalState.Warnings }


module SvgParser =

    let ofString (svgString: string) : Result<ParseResult<Svg>, ParseError> =
        try
            let doc = XDocument.Parse(svgString)
            match doc.Root with
            | null ->
                Error { Message = "SVG document has no root element"; ElementName = None }
            | root when root.Name.LocalName <> "svg" ->
                Error { Message = sprintf "Root element is '%s', expected 'svg'" root.Name.LocalName; ElementName = Some root.Name.LocalName }
            | root ->
                SvgRootParser.parseSvgRoot root
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    let ofFile (path: string) : Result<ParseResult<Svg>, ParseError> =
        try
            let content = System.IO.File.ReadAllText(path)
            ofString content
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    let stripUnknown (svg: Svg) : Svg =
        let rec stripBody (body: Body) : Body =
            body
            |> Seq.choose (function
                | GroupElement.Raw _ -> None
                | GroupElement.Element e -> Some (GroupElement.Element e)
                | GroupElement.Group g ->
                    let cleaned = { g with Body = stripBody g.Body }
                    Some (GroupElement.Group cleaned))
        { svg with Body = stripBody svg.Body }
