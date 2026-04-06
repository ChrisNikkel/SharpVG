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

type ParseMode =
    /// Unknown elements are silently preserved as raw passthrough values. (Default)
    | Lenient
    /// Unknown elements produce a ParseWarning for each occurrence.
    | Strict

// --- Private implementation ---

module private SvgParserHelpers =

    // System.Text.RegularExpressions opened only here — prevents Group type
    // from shadowing SharpVG.Group in downstream modules.
    open System.Text.RegularExpressions

    type ParseState =
        {
            Warnings: ParseWarning list
            Mode: ParseMode
            CssRules: (string * Style) list
        }

    let stateFor (mode: ParseMode) : ParseState = { Warnings = []; Mode = mode; CssRules = [] }
    let emptyState : ParseState = stateFor Lenient

    let addCssRules (rules: (string * Style) list) (state: ParseState) : ParseState =
        { state with CssRules = state.CssRules @ rules }

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
        | "clip-path" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with ClipPath = Some i })
        | "filter" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with Filter = Some i })
        | "marker-start" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with MarkerStart = Some i })
        | "marker-mid" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with MarkerMid = Some i })
        | "marker-end" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with MarkerEnd = Some i })
        | "mask" ->
            let v = value.Trim()
            let id = if v.StartsWith("url(#") && v.EndsWith(")") then Some v.[5..v.Length - 2] else None
            id |> Option.map (fun i -> { style with Mask = Some i })
        | "stroke-miterlimit" ->
            tryParseFloat value |> Option.map (fun f -> { style with StrokeMiterLimit = Some f })
        | "paint-order" ->
            let layers =
                value.Trim().Split(' ')
                |> Array.choose (fun s ->
                    match s.Trim() with
                    | "fill"    -> Some FillLayer
                    | "stroke"  -> Some StrokeLayer
                    | "markers" -> Some MarkersLayer
                    | _ -> None)
                |> Array.toList
            if layers.IsEmpty then None else Some { style with PaintOrder = Some layers }
        | "vector-effect" ->
            match value.Trim() with
            | "none"               -> Some { style with VectorEffect = Some VectorEffectNone }
            | "non-scaling-stroke" -> Some { style with VectorEffect = Some NonScalingStroke }
            | "non-scaling-size"   -> Some { style with VectorEffect = Some NonScalingSize }
            | "non-rotation"       -> Some { style with VectorEffect = Some NonRotation }
            | "fixed-position"     -> Some { style with VectorEffect = Some FixedPosition }
            | _ -> None
        | "shape-rendering" ->
            match value.Trim() with
            | "auto"              -> Some { style with ShapeRendering = Some ShapeRenderingAuto }
            | "optimizeSpeed"     -> Some { style with ShapeRendering = Some OptimizeSpeed }
            | "crispEdges"        -> Some { style with ShapeRendering = Some CrispEdges }
            | "geometricPrecision"-> Some { style with ShapeRendering = Some GeometricPrecision }
            | _ -> None
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

    /// Parse a CSS text block into (selector, Style) pairs.
    /// Supports class selectors (.foo), element-type selectors (circle), and
    /// multi-selector rules (circle, .foo). Comments are stripped first.
    let parseCssBlock (css: string) : (string * Style) list =
        let noComments = Regex.Replace(css, @"/\*.*?\*/", "", RegexOptions.Singleline)
        Regex.Matches(noComments, @"([^{]+)\{([^}]*)\}", RegexOptions.Singleline)
        |> Seq.cast<Match>
        |> Seq.toList
        |> List.collect (fun m ->
            let selectors =
                m.Groups.[1].Value.Split(',')
                |> Array.map (fun s -> s.Trim())
                |> Array.filter (fun s -> s <> "")
            let declarations = m.Groups.[2].Value
            let style =
                declarations.Split(';')
                |> Array.fold (fun s decl ->
                    match decl.Split([|':'|], 2) with
                    | [|name; value|] ->
                        match tryParseCssProperty name value s with
                        | Some updated -> updated
                        | None -> s
                    | _ -> s) Style.empty
            if style = Style.empty then []
            else selectors |> Array.toList |> List.map (fun sel -> (sel, style)))

    /// Merge sheet style into element style where the element has no value set (inline wins).
    let fillStyleFromSheet (sheet: Style) (existing: Style) : Style =
        {
            Fill             = existing.Fill             |> Option.orElse sheet.Fill
            Stroke           = existing.Stroke           |> Option.orElse sheet.Stroke
            StrokeWidth      = existing.StrokeWidth      |> Option.orElse sheet.StrokeWidth
            Opacity          = existing.Opacity          |> Option.orElse sheet.Opacity
            FillOpacity      = existing.FillOpacity      |> Option.orElse sheet.FillOpacity
            Name             = existing.Name             |> Option.orElse sheet.Name
            StrokeLinecap    = existing.StrokeLinecap    |> Option.orElse sheet.StrokeLinecap
            StrokeLinejoin   = existing.StrokeLinejoin   |> Option.orElse sheet.StrokeLinejoin
            StrokeDashArray  = existing.StrokeDashArray  |> Option.orElse sheet.StrokeDashArray
            StrokeDashOffset = existing.StrokeDashOffset |> Option.orElse sheet.StrokeDashOffset
            FillRule         = existing.FillRule         |> Option.orElse sheet.FillRule
            ClipPath         = existing.ClipPath         |> Option.orElse sheet.ClipPath
            Filter           = existing.Filter           |> Option.orElse sheet.Filter
            MarkerStart      = existing.MarkerStart      |> Option.orElse sheet.MarkerStart
            MarkerMid        = existing.MarkerMid        |> Option.orElse sheet.MarkerMid
            MarkerEnd        = existing.MarkerEnd        |> Option.orElse sheet.MarkerEnd
            StrokeMiterLimit = existing.StrokeMiterLimit |> Option.orElse sheet.StrokeMiterLimit
            Mask             = existing.Mask             |> Option.orElse sheet.Mask
            Visibility       = existing.Visibility       |> Option.orElse sheet.Visibility
            Display          = existing.Display          |> Option.orElse sheet.Display
            PaintOrder       = existing.PaintOrder       |> Option.orElse sheet.PaintOrder
            VectorEffect     = existing.VectorEffect     |> Option.orElse sheet.VectorEffect
            ShapeRendering   = existing.ShapeRendering   |> Option.orElse sheet.ShapeRendering
        }

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

    let tryParseViewBox (s: string) : ViewBox option =
        let nums =
            s.Split([|','; ' '|], StringSplitOptions.RemoveEmptyEntries)
            |> Array.choose tryParseFloat
        match nums with
        | [|minX; minY; width; height|] ->
            Some (ViewBox.create (Point.ofFloats (minX, minY)) (Area.ofFloats (width, height)))
        | _ -> None

    let tryAttr name (xel: XElement) =
        xel.Attribute(XName.Get name) |> Option.ofObj |> Option.map _.Value

    // SVG 1.1 uses xlink:href; SVG 2 uses plain href. Check both, normalizing to href.
    let private xlinkNs = XNamespace.Get "http://www.w3.org/1999/xlink"
    let tryHref (xel: XElement) =
        xel.Attribute(XName.Get "href") |> Option.ofObj
        |> Option.orElse (xel.Attribute(xlinkNs + "href") |> Option.ofObj)
        |> Option.map _.Value

    let tryParseSpreadMethod (s: string) =
        match s.ToLowerInvariant() with
        | "reflect" -> Some Reflect
        | "repeat"  -> Some Repeat
        | _         -> Some Pad

    let tryParseFilterUnits (s: string) =
        match s.ToLowerInvariant() with
        | "userspaceonuse" -> Some UserSpaceOnUse
        | _               -> Some ObjectBoundingBox

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
        let state =
            match state.Mode with
            | Strict -> warnState (sprintf "Unknown element '%s'" xel.Name.LocalName) (Some xel.Name.LocalName) state
            | Lenient -> state
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
        GroupElement.Element (Element.ofRaw raw), state

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
        | El "a"        -> parseAnchor xel state
        | _             -> parseRaw xel state

    and parseAnchor (xel: XElement) (state: ParseState) : GroupElement * ParseState =
        let url = tryHref xel |> Option.defaultValue ""
        let (children, finalState) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements =
            children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let anchor = Anchor.create url |> Anchor.withBody (elements |> Seq.ofList)
        GroupElement.Element (anchor |> Element.create |> applyCommon xel), finalState

    let parseGradientStop (xel: XElement) : GradientStop option =
        let offsetStr = tryAttr "offset" xel |> Option.defaultValue "0"
        let offset =
            if offsetStr.EndsWith("%") then
                tryParseFloat (offsetStr.[..offsetStr.Length - 2]) |> Option.map (fun v -> v / 100.0) |> Option.defaultValue 0.0
            else
                tryParseFloat offsetStr |> Option.defaultValue 0.0
        let color =
            tryAttr "stop-color" xel
            |> Option.bind tryParseColor
            |> Option.defaultValue (Color.ofName Colors.Black)
        let stop = GradientStop.create offset color
        let withOpacity =
            tryAttr "stop-opacity" xel
            |> Option.bind tryParseFloat
            |> Option.map (fun o -> stop |> GradientStop.withOpacity o)
            |> Option.defaultValue stop
        Some withOpacity

    // Apply an optional transform: applyOpt f (Some v) x = f v x; applyOpt f None x = x
    let inline applyOpt f optVal x =
        match optVal with
        | Some v -> f v x
        | None   -> x

    let parseLinearGradient (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let x1 = match xel with FloatAttr "x1" v -> v | _ -> 0.0
        let y1 = match xel with FloatAttr "y1" v -> v | _ -> 0.0
        let x2 = match xel with FloatAttr "x2" v -> v | _ -> 1.0
        let y2 = match xel with FloatAttr "y2" v -> v | _ -> 0.0
        let point1 = Point.ofFloats (x1, y1)
        let point2 = Point.ofFloats (x2, y2)
        let stops =
            xel.Elements()
            |> Seq.filter (fun e -> e.Name.LocalName = "stop")
            |> Seq.choose parseGradientStop
            |> Seq.toList
        let gradient =
            LinearGradient.create elemId point1 point2 stops
            |> applyOpt LinearGradient.withSpreadMethod (tryAttr "spreadMethod" xel |> Option.bind tryParseSpreadMethod)
            |> applyOpt LinearGradient.withGradientUnits (tryAttr "gradientUnits" xel |> Option.bind tryParseFilterUnits)
            |> applyOpt LinearGradient.withHref (tryHref xel)
        fun (defs, s) -> SvgDefinitions.addGradient (Gradient.ofLinear gradient) defs, s

    let parseRadialGradient (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let cx = match xel with FloatAttr "cx" v -> v | _ -> 0.5
        let cy = match xel with FloatAttr "cy" v -> v | _ -> 0.5
        let r  = match xel with FloatAttr "r"  v -> v | _ -> 0.5
        let center = Point.ofFloats (cx, cy)
        let radius = Length.ofFloat r
        let stops =
            xel.Elements()
            |> Seq.filter (fun e -> e.Name.LocalName = "stop")
            |> Seq.choose parseGradientStop
            |> Seq.toList
        let focal =
            match xel with
            | FloatAttr "fx" fx ->
                let fy = match xel with FloatAttr "fy" v -> v | _ -> cy
                Some (Point.ofFloats (fx, fy))
            | _ -> None
        let gradient =
            RadialGradient.create elemId center radius stops
            |> applyOpt RadialGradient.withFocal focal
            |> applyOpt RadialGradient.withSpreadMethod (tryAttr "spreadMethod" xel |> Option.bind tryParseSpreadMethod)
            |> applyOpt RadialGradient.withGradientUnits (tryAttr "gradientUnits" xel |> Option.bind tryParseFilterUnits)
            |> applyOpt RadialGradient.withHref (tryHref xel)
        fun (defs, s) -> SvgDefinitions.addGradient (Gradient.ofRadial gradient) defs, s

    let parseClipPath (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let (children, _) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements = children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let clipPath =
            ClipPath.create elemId
            |> ClipPath.addElements (elements |> Seq.ofList)
            |> applyOpt ClipPath.withClipPathUnits (tryAttr "clipPathUnits" xel |> Option.bind tryParseFilterUnits)
        fun (defs, s) -> SvgDefinitions.addClipPath clipPath defs, s

    let parseMask (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let (children, _) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements = children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let location =
            match xel with
            | FloatAttr "x" x ->
                let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
                Some (Point.ofFloats (x, y))
            | _ -> None
        let size =
            match xel with
            | FloatAttr "width" w ->
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                Some (Area.ofFloats (w, h))
            | _ -> None
        let mask =
            Mask.create elemId
            |> Mask.addElements (elements |> Seq.ofList)
            |> applyOpt Mask.withLocation location
            |> applyOpt Mask.withSize size
            |> applyOpt Mask.withMaskUnits (tryAttr "maskUnits" xel |> Option.bind tryParseFilterUnits)
            |> applyOpt Mask.withMaskContentUnits (tryAttr "maskContentUnits" xel |> Option.bind tryParseFilterUnits)
        fun (defs, s) -> SvgDefinitions.addMask mask defs, s

    let parsePattern (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let (children, _) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements = children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let position =
            match xel with
            | FloatAttr "x" x ->
                let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
                Some (Point.ofFloats (x, y))
            | _ -> None
        let size =
            match xel with
            | FloatAttr "width" w ->
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                Some (Area.ofFloats (w, h))
            | _ -> None
        let viewBox = tryAttr "viewBox" xel |> Option.bind tryParseViewBox
        let pattern =
            Pattern.create elemId
            |> Pattern.addElements (elements |> Seq.ofList)
            |> applyOpt Pattern.withPosition position
            |> applyOpt Pattern.withSize size
            |> applyOpt Pattern.withViewBox viewBox
            |> applyOpt Pattern.withPatternUnits (tryAttr "patternUnits" xel |> Option.bind tryParseFilterUnits)
            |> applyOpt Pattern.withPatternContentUnits (tryAttr "patternContentUnits" xel |> Option.bind tryParseFilterUnits)
        fun (defs, s) -> SvgDefinitions.addPattern pattern defs, s

    let parseMarker (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel |> Option.defaultValue ""
        let (children, _) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements = children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let viewBox = tryAttr "viewBox" xel |> Option.bind tryParseViewBox
        let refPoint =
            match xel with
            | FloatAttr "refX" rx ->
                let ry = match xel with FloatAttr "refY" v -> v | _ -> 0.0
                Some (Point.ofFloats (rx, ry))
            | _ -> None
        let size =
            match xel with
            | FloatAttr "width" w ->
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                Some (Area.ofFloats (w, h))
            | _ -> None
        let orient =
            tryAttr "orient" xel |> Option.bind (fun s ->
                if s = "auto" then Some AutoOrient
                else tryParseFloat s |> Option.map AngleOrient)
        let units =
            tryAttr "markerUnits" xel |> Option.bind (fun s ->
                match s with
                | "userSpaceOnUse" -> Some UserSpaceOnUseUnits
                | _ -> Some StrokeWidthUnits)
        let marker =
            Marker.create elemId
            |> Marker.addElements (elements |> Seq.ofList)
            |> applyOpt Marker.withViewBox viewBox
            |> applyOpt Marker.withRefPoint refPoint
            |> applyOpt Marker.withSize size
            |> applyOpt Marker.withOrient orient
            |> applyOpt Marker.withUnits units
        fun (defs, s) -> SvgDefinitions.addMarker marker defs, s

    let parseFilter (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let elemId = tryAttr "id" xel
        let location =
            match xel with
            | FloatAttr "x" x ->
                let y = match xel with FloatAttr "y" v -> v | _ -> 0.0
                Some (Point.ofFloats (x, y))
            | _ -> None
        let area =
            match xel with
            | FloatAttr "width" w ->
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                Some (Area.ofFloats (w, h))
            | _ -> None
        let filterUnits = tryAttr "filterUnits" xel |> Option.bind tryParseFilterUnits
        let filter =
            {
                Filter.empty with
                    Id = elemId
                    Location = location
                    Area = area
                    FilterUnits = filterUnits
            }
        fun (defs, s) -> SvgDefinitions.addFilter filter defs, s

    let parseSymbol (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState -> SvgDefinitions * ParseState =
        let (children, _) =
            xel.Elements()
            |> Seq.fold (fun (acc, s) child ->
                let (ge, s2) = parseElement child s
                (acc @ [ge], s2)) ([], state)
        let elements = children |> List.choose (function GroupElement.Element e -> Some e | _ -> None)
        let viewBox =
            tryAttr "viewBox" xel |> Option.bind tryParseViewBox
            |> Option.defaultWith (fun () ->
                let w = match xel with FloatAttr "width" v -> v | _ -> 0.0
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                ViewBox.create Point.origin (Area.ofFloats (w, h)))
        let size =
            match xel with
            | FloatAttr "width" w ->
                let h = match xel with FloatAttr "height" v -> v | _ -> 0.0
                Some (Area.ofFloats (w, h))
            | _ -> None
        let symbol =
            Symbol.create viewBox
            |> Symbol.addElements (elements |> Seq.ofList)
            |> (fun sym -> Symbol.withSize size sym)
        fun (defs, s) -> SvgDefinitions.addSymbol symbol defs, s

    let parseDefs (xel: XElement) (state: ParseState) : SvgDefinitions * ParseState =
        xel.Elements()
        |> Seq.fold (fun (defs, s) child ->
            match child with
            | El "g" ->
                let (group, s2) = parseGroup child s
                SvgDefinitions.addGroup group defs, s2
            | El "linearGradient"  -> parseLinearGradient child s (defs, s)
            | El "radialGradient"  -> parseRadialGradient child s (defs, s)
            | El "clipPath"        -> parseClipPath child s (defs, s)
            | El "mask"            -> parseMask child s (defs, s)
            | El "pattern"         -> parsePattern child s (defs, s)
            | El "marker"          -> parseMarker child s (defs, s)
            | El "filter"          -> parseFilter child s (defs, s)
            | El "symbol"          -> parseSymbol child s (defs, s)
            | El "style"           -> defs, addCssRules (parseCssBlock child.Value) s
            | El "circle" | El "ellipse" | El "rect" | El "line"
            | El "path"   | El "polygon" | El "polyline" | El "text"
            | El "image"  | El "use" ->
                let (ge, s2) = parseElement child s
                match ge with
                | GroupElement.Element e -> SvgDefinitions.addElement e defs, s2
                | _ -> defs, s2
            | _ ->
                let s =
                    match s.Mode with
                    | Strict -> warnState (sprintf "Unknown definition element '%s'" child.Name.LocalName) (Some child.Name.LocalName) s
                    | Lenient -> s
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

    // Apply parsed CSS rules to an element based on class names and element-type selector.
    // Sheet style fills in fields not already set by inline style (inline wins).
    let private applySheetToElement (rules: (string * Style) list) (element: Element) : Element =
        if rules.IsEmpty then element
        else
            // Collect matching styles: class rules (.foo) and element-type rules (circle, rect…)
            // Element-type matching: we look at the Tag name in the element's content.
            let tagName =
                match element.Content with
                | TagContent tag -> Some tag.Name
                | RawContent _ -> None
            let matchedStyle =
                rules
                |> List.fold (fun acc (sel, style) ->
                    let classMatch = sel.StartsWith(".") && element.Classes |> Seq.exists (fun c -> "." + c = sel)
                    let typeMatch  = tagName |> Option.exists (fun n -> n = sel)
                    if classMatch || typeMatch then fillStyleFromSheet style acc
                    else acc) Style.empty
            if matchedStyle = Style.empty then element
            else
                let existing = element.Style |> Option.defaultValue Style.empty
                let merged = fillStyleFromSheet matchedStyle existing
                { element with Style = if merged = Style.empty then None else Some merged }

    let parseSvgRoot (mode: ParseMode) (root: XElement) : Result<ParseResult<Svg>, ParseError> =
        let state = stateFor mode

        // Collect <style> elements from the SVG root (before defs/body parsing so rules are available)
        let cssText =
            root.Elements()
            |> Seq.filter (fun e -> e.Name.LocalName = "style")
            |> Seq.map _.Value
            |> String.concat "\n"
        let stateWithCss =
            if cssText.Trim() = "" then state
            else addCssRules (parseCssBlock cssText) state

        // Separate <defs> from body elements — defs go into SvgDefinitions, rest into Body
        // Also exclude <style>, <title>, <desc> from body (handled separately)
        let excludedLocally = System.Collections.Generic.HashSet(["defs"; "style"; "title"; "desc"])
        let defsElements = root.Elements() |> Seq.filter (fun e -> e.Name.LocalName = "defs") |> Seq.toList
        let bodyElements = root.Elements() |> Seq.filter (fun e -> not (excludedLocally.Contains(e.Name.LocalName))) |> Seq.toList

        let (definitions, stateAfterDefs) =
            defsElements
            |> List.fold (fun (defs, s) defsEl ->
                let (parsed, s2) = SvgElementParsers.parseDefs defsEl s
                let merged =
                    { defs with Contents = Seq.append defs.Contents parsed.Contents }
                merged, s2) (SvgDefinitions.create, stateWithCss)

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
            |> Option.bind (fun a -> SvgParserHelpers.tryParseViewBox a.Value)

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

        let baseSvg =
            {
                Body = children |> Seq.ofList
                Definitions = defsOption
                Size = size
                ViewBox = viewBox
                PreserveAspectRatio = None
                Title = title
                Description = description
            }

        // Apply parsed CSS rules to elements (class and element-type selectors)
        let svg =
            if finalState.CssRules.IsEmpty then baseSvg
            else Svg.mapElements (applySheetToElement finalState.CssRules) baseSvg

        Ok { Value = svg; Warnings = finalState.Warnings }


module private SvgHtmlParser =

    open System.Text.RegularExpressions

    // Try to extract SVG root elements from HTML.
    // Strategy 1: parse as XML (works for XHTML / well-formed HTML).
    // Strategy 2: regex extraction (works for HTML5 where the document isn't XML-valid).
    let extractSvgStrings (html: string) : string list =
        try
            let doc = XDocument.Parse(html)
            doc.Descendants()
            |> Seq.filter (fun el -> el.Name.LocalName = "svg")
            |> Seq.map _.ToString()
            |> Seq.toList
        with _ ->
            // HTML5 fallback: find <svg ...> ... </svg> blocks.
            // Handles nesting by tracking depth; good enough for typical embedding.
            let svgOpen = Regex(@"<svg[\s>]", RegexOptions.IgnoreCase)
            let svgClose = Regex(@"</svg\s*>", RegexOptions.IgnoreCase)
            let mutable result = []
            let mutable searchFrom = 0
            let mutable loop = true
            while loop do
                let openMatch = svgOpen.Match(html, searchFrom)
                if not openMatch.Success then
                    loop <- false
                else
                    let start = openMatch.Index
                    let mutable depth = 1
                    let mutable pos = start + openMatch.Length
                    while depth > 0 && pos < html.Length do
                        let nextOpen  = svgOpen.Match(html, pos)
                        let nextClose = svgClose.Match(html, pos)
                        match nextOpen.Success, nextClose.Success with
                        | true, true when nextOpen.Index < nextClose.Index ->
                            depth <- depth + 1
                            pos <- nextOpen.Index + nextOpen.Length
                        | _, true ->
                            depth <- depth - 1
                            pos <- nextClose.Index + nextClose.Length
                        | _ ->
                            depth <- 0   // malformed; stop
                    if depth = 0 then
                        result <- html.[start .. pos - 1] :: result
                    searchFrom <- pos
            List.rev result


module SvgParser =

    let ofStringWith (mode: ParseMode) (svgString: string) : Result<ParseResult<Svg>, ParseError> =
        try
            let doc = XDocument.Parse(svgString)
            match doc.Root with
            | null ->
                Error { Message = "SVG document has no root element"; ElementName = None }
            | root when root.Name.LocalName <> "svg" ->
                Error { Message = sprintf "Root element is '%s', expected 'svg'" root.Name.LocalName; ElementName = Some root.Name.LocalName }
            | root ->
                SvgRootParser.parseSvgRoot mode root
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    /// Parse SVG from a string in Lenient mode (unknown elements preserved as raw passthrough).
    let ofString (svgString: string) = ofStringWith Lenient svgString

    let ofFileWith (mode: ParseMode) (path: string) : Result<ParseResult<Svg>, ParseError> =
        try
            let content = System.IO.File.ReadAllText(path)
            ofStringWith mode content
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    /// Parse SVG from a file path in Lenient mode.
    let ofFile (path: string) = ofFileWith Lenient path

    let ofStreamWith (mode: ParseMode) (stream: System.IO.Stream) : Result<ParseResult<Svg>, ParseError> =
        try
            use reader = new System.IO.StreamReader(stream)
            let content = reader.ReadToEnd()
            ofStringWith mode content
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    /// Parse SVG from a stream in Lenient mode.
    let ofStream (stream: System.IO.Stream) = ofStreamWith Lenient stream

    let ofGzipStreamWith (mode: ParseMode) (stream: System.IO.Stream) : Result<ParseResult<Svg>, ParseError> =
        try
            use gzip = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress)
            use reader = new System.IO.StreamReader(gzip, System.Text.Encoding.UTF8)
            ofStringWith mode (reader.ReadToEnd())
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    /// Parse SVGZ (gzip-compressed SVG) from a stream in Lenient mode.
    let ofGzipStream (stream: System.IO.Stream) = ofGzipStreamWith Lenient stream

    let ofGzipFileWith (mode: ParseMode) (path: string) : Result<ParseResult<Svg>, ParseError> =
        try
            use file = System.IO.File.OpenRead(path)
            ofGzipStreamWith mode file
        with ex ->
            Error { Message = ex.Message; ElementName = None }

    /// Parse SVGZ from a file path in Lenient mode.
    let ofGzipFile (path: string) = ofGzipFileWith Lenient path

    /// Extract and parse all SVG elements embedded in an HTML string.
    /// Works with XHTML (XML parse) and HTML5 (regex extraction).
    /// Returns one result per SVG found; an empty list means no SVG was present.
    let ofHtmlStringWith (mode: ParseMode) (html: string) : Result<ParseResult<Svg>, ParseError> list =
        SvgHtmlParser.extractSvgStrings html
        |> List.map (ofStringWith mode)

    /// Extract and parse all SVG elements embedded in an HTML string in Lenient mode.
    let ofHtmlString (html: string) = ofHtmlStringWith Lenient html

    let ofHtmlFileWith (mode: ParseMode) (path: string) : Result<ParseResult<Svg>, ParseError> list =
        try
            let content = System.IO.File.ReadAllText(path)
            ofHtmlStringWith mode content
        with ex ->
            [ Error { Message = ex.Message; ElementName = None } ]

    /// Extract and parse all SVG elements embedded in an HTML file in Lenient mode.
    let ofHtmlFile (path: string) = ofHtmlFileWith Lenient path

    let stripUnknown (svg: Svg) : Svg =
        let rec stripBody (body: Body) : Body =
            body
            |> Seq.choose (function
                | GroupElement.Element e when Element.isRaw e -> None
                | GroupElement.Element e -> Some (GroupElement.Element e)
                | GroupElement.Group g ->
                    let cleaned = { g with Body = stripBody g.Body }
                    Some (GroupElement.Group cleaned))
        { svg with Body = stripBody svg.Body }
