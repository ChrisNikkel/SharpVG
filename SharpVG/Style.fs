namespace SharpVG

type Style =
    {
        Fill : Color option;
        Stroke : Color option;
        StrokeWidth : Length option;
        Opacity: float option;
        FillOpacity: float option;
        Name: string option;
    }
with
    static member private MapToString f style =
        [
            style.Stroke |> Option.map (Color.toString >> (f "stroke"));
            style.StrokeWidth |> Option.map (Length.toString >> (f "stroke-width"));
            style.Fill |> Option.map (Color.toString >> (f "fill"));
            style.Opacity |> Option.map (string >> (f "opacity"));
            style.FillOpacity |> Option.map (string >> (f "fill-opacity"))
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
        { Fill = None; Stroke = None; StrokeWidth = None; Opacity = None; FillOpacity = None; Name = None; }

    let withName name (style : Style) =
        { style with Name = Option.ofObj name }

    let createWithPen pen =
        { Stroke = Some(pen.Color); Opacity = Some(pen.Opacity); StrokeWidth = Some(pen.Width); Fill = None; FillOpacity = None; Name = None; }

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
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); FillOpacity = Some(fillOpacity); Name = None }

    let createNamed fill stroke strokeWidth opacity fillOpacity name =
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); FillOpacity = Some(fillOpacity); Name = Option.ofObj name }

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
