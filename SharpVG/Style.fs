namespace SharpVG

type Visibility =
    | Visible
    | Hidden
    | Collapse
with
    override this.ToString() =
        match this with
        | Visible -> "visible"
        | Hidden -> "hidden"
        | Collapse -> "collapse"

type Display =
    | Inline
    | DisplayNone
with
    override this.ToString() =
        match this with
        | Inline -> "inline"
        | DisplayNone -> "none"

type StrokeLinecap =
    | ButtLinecap
    | RoundLinecap
    | SquareLinecap
with
    override this.ToString() =
        match this with
        | ButtLinecap -> "butt"
        | RoundLinecap -> "round"
        | SquareLinecap -> "square"

type StrokeLinejoin =
    | MiterLinejoin
    | RoundLinejoin
    | BevelLinejoin
with
    override this.ToString() =
        match this with
        | MiterLinejoin -> "miter"
        | RoundLinejoin -> "round"
        | BevelLinejoin -> "bevel"

type FillRule =
    | NonZero
    | EvenOdd
with
    override this.ToString() =
        match this with
        | NonZero -> "nonzero"
        | EvenOdd -> "evenodd"

type PaintLayer =
    | FillLayer
    | StrokeLayer
    | MarkersLayer
with
    override this.ToString() =
        match this with
        | FillLayer -> "fill"
        | StrokeLayer -> "stroke"
        | MarkersLayer -> "markers"

type VectorEffect =
    | VectorEffectNone
    | NonScalingStroke
    | NonScalingSize
    | NonRotation
    | FixedPosition
with
    override this.ToString() =
        match this with
        | VectorEffectNone -> "none"
        | NonScalingStroke -> "non-scaling-stroke"
        | NonScalingSize -> "non-scaling-size"
        | NonRotation -> "non-rotation"
        | FixedPosition -> "fixed-position"

type ShapeRendering =
    | ShapeRenderingAuto
    | OptimizeSpeed
    | CrispEdges
    | GeometricPrecision
with
    override this.ToString() =
        match this with
        | ShapeRenderingAuto -> "auto"
        | OptimizeSpeed -> "optimizeSpeed"
        | CrispEdges -> "crispEdges"
        | GeometricPrecision -> "geometricPrecision"

type Style =
    {
        Fill : Color option;
        Stroke : Color option;
        StrokeWidth : Length option;
        Opacity: float option;
        FillOpacity: float option;
        Name: string option;
        StrokeLinecap: StrokeLinecap option;
        StrokeLinejoin: StrokeLinejoin option;
        StrokeDashArray: float list option;
        StrokeDashOffset: float option;
        FillRule: FillRule option;
        ClipPath: string option;
        Filter: string option;
        MarkerStart: string option;
        MarkerMid: string option;
        MarkerEnd: string option;
        StrokeMiterLimit: float option;
        Mask: string option;
        Visibility: Visibility option;
        Display: Display option;
        PaintOrder: PaintLayer list option;
        VectorEffect: VectorEffect option;
        ShapeRendering: ShapeRendering option;
    }
with
    static member private MapToString f style =
        [
            style.Stroke |> Option.map (Color.toString >> (f "stroke"));
            style.StrokeWidth |> Option.map (Length.toString >> (f "stroke-width"));
            style.Fill |> Option.map (Color.toString >> (f "fill"));
            style.Opacity |> Option.map (string >> (f "opacity"));
            style.FillOpacity |> Option.map (string >> (f "fill-opacity"));
            style.StrokeLinecap |> Option.map (fun lc -> f "stroke-linecap" (lc.ToString()));
            style.StrokeLinejoin |> Option.map (fun lj -> f "stroke-linejoin" (lj.ToString()));
            style.StrokeDashArray |> Option.map (fun da -> f "stroke-dasharray" (da |> List.map string |> String.concat ","));
            style.StrokeDashOffset |> Option.map (string >> (f "stroke-dashoffset"));
            style.FillRule |> Option.map (fun fr -> f "fill-rule" (fr.ToString()));
            style.ClipPath |> Option.map (fun id -> f "clip-path" ("url(#" + id + ")"));
            style.Filter |> Option.map (fun id -> f "filter" ("url(#" + id + ")"));
            style.MarkerStart |> Option.map (fun id -> f "marker-start" ("url(#" + id + ")"));
            style.MarkerMid |> Option.map (fun id -> f "marker-mid" ("url(#" + id + ")"));
            style.MarkerEnd |> Option.map (fun id -> f "marker-end" ("url(#" + id + ")"));
            style.StrokeMiterLimit |> Option.map (string >> (f "stroke-miterlimit"));
            style.Mask |> Option.map (fun id -> f "mask" ("url(#" + id + ")"));
            style.Visibility |> Option.map (fun v -> f "visibility" (v.ToString()));
            style.Display |> Option.map (fun d -> f "display" (d.ToString()));
            style.PaintOrder |> Option.map (fun layers -> f "paint-order" (if List.isEmpty layers then "normal" else layers |> List.map (fun l -> l.ToString()) |> String.concat " "));
            style.VectorEffect |> Option.map (fun ve -> f "vector-effect" (ve.ToString()));
            style.ShapeRendering |> Option.map (fun sr -> f "shape-rendering" (sr.ToString()));
        ] |> List.choose id

    static member ToAttributes style =
        Style.MapToString Attribute.createCSS style

    static member ToString style =
        let stylePartToString name value = name + ":" + value
        Style.MapToString stylePartToString style  |> String.concat ";"

    static member ToCssString style =
        let cssName =
            match style.Name with
            | Some name  when name.Contains(".") -> name
            | Some name -> ("." + name) + " "
            | None -> ""
        cssName + "{" + (style |> Style.ToString) + "}"
    static member ToTag style =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.createCSS "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (Style.ToCssString style) + "]]>")

    override this.ToString() =
        this |> Style.ToString

type Styles =
    {
        Styles: seq<Style>
    }
with
    static member ToTag styles =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.createCSS "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (styles |> Seq.map Style.ToCssString |> String.concat " ") + "]]>")

module Style =
    let empty =
        { Fill = None; Stroke = None; StrokeWidth = None; Opacity = None; FillOpacity = None; Name = None;
          StrokeLinecap = None; StrokeLinejoin = None; StrokeDashArray = None; StrokeDashOffset = None;
          FillRule = None; ClipPath = None; Filter = None; MarkerStart = None; MarkerMid = None; MarkerEnd = None;
          StrokeMiterLimit = None; Mask = None; Visibility = None; Display = None;
          PaintOrder = None; VectorEffect = None; ShapeRendering = None }

    let withName name (style : Style) =
        { style with Name = Option.ofObj name }

    let createWithPen pen =
        { empty with Stroke = Some(pen.Color); Opacity = Some(pen.Opacity); StrokeWidth = Some(pen.Width) }

    let withStrokePen pen style =
        { style with Stroke = Some(pen.Color); Opacity = Some(pen.Opacity); StrokeWidth = Some(pen.Width) }

    let withFillPen pen style =
        { style with Fill = Some(pen.Color); FillOpacity = Some(pen.Opacity) }

    let withFill fill style =
        { style with Fill = Some(fill) }

    let withStroke stroke (style : Style) =
        { style with Stroke = Some(stroke) }

    let withStrokeWidth strokeWidth style =
        { style with StrokeWidth = Some(strokeWidth) }

    let withOpacity opacity (style : Style) =
        { style with Opacity = Some(opacity) }

    let withFillOpacity opacity style =
        { style with FillOpacity = Some(opacity) }

    let create fill stroke strokeWidth opacity fillOpacity =
        { empty with Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); FillOpacity = Some(fillOpacity) }

    let createNamed fill stroke strokeWidth opacity fillOpacity name =
        { empty with Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); FillOpacity = Some(fillOpacity); Name = Option.ofObj name }

    let createWithFill fill =
        empty |> withFill fill

    let createWithStroke stroke =
        empty |> withStroke stroke

    let createWithStrokeWidth strokeWidth =
        empty |> withStrokeWidth strokeWidth

    let createWithOpacity opacity =
        empty |> withOpacity opacity

    let createWithFillOpacity opacity =
        empty |> withFillOpacity opacity

    let withStrokeLinecap linecap style =
        { style with StrokeLinecap = Some linecap }

    let withStrokeLinejoin linejoin style =
        { style with StrokeLinejoin = Some linejoin }

    let withStrokeDashArray dashArray style =
        { style with StrokeDashArray = Some dashArray }

    let withStrokeDashOffset dashOffset style =
        { style with StrokeDashOffset = Some dashOffset }

    let withFillRule fillRule style =
        { style with FillRule = Some fillRule }

    let withClipPath id style =
        { style with ClipPath = Some id }

    let withFilter id style =
        { style with Filter = Some id }

    let withMarkerStart id style =
        { style with MarkerStart = Some id }

    let withMarkerMid id style =
        { style with MarkerMid = Some id }

    let withMarkerEnd id style =
        { style with MarkerEnd = Some id }

    let withStrokeMiterLimit limit style =
        { style with StrokeMiterLimit = Some limit }

    let withMask id style =
        { style with Mask = Some id }

    let withVisibility visibility style =
        { style with Visibility = Some visibility }

    let withDisplay display style =
        { style with Display = Some display }

    let withPaintOrder paintOrder style =
        { style with PaintOrder = Some paintOrder }

    let withVectorEffect vectorEffect style =
        { style with VectorEffect = Some vectorEffect }

    let withShapeRendering shapeRendering style =
        { style with ShapeRendering = Some shapeRendering }

    let createWithName name =
        empty |> withName name

    let isNamed style =
        style.Name.IsSome

    let toAttributes =
        Style.ToAttributes

    let toString =
        Style.ToString

    let toAttribute style =
        Attribute.createCSS "style" (toString style)

    let toCssString =
        Style.ToCssString

    let toTag =
        Style.ToTag

module Styles =
    let toTag =
        Styles.ToTag

    let toString styles =
        styles |> toTag |> Tag.toString

    let named styles =
        styles |> Seq.filter Style.isNamed
