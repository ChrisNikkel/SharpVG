namespace SharpVG

type Style =
    {
        Fill : Color option;
        Stroke : Color option;
        StrokeWidth : Length option;
        Opacity: float option;
    }

type NamedStyle = {
    Name : string;
    Style : Style;
}

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Style =
    let empty =
        { Fill = None; Stroke = None; StrokeWidth = None; Opacity = None }

    let withFill fill style =
        { style with Fill = Some(fill) }

    let withStroke stroke style =
        { style with Stroke = Some(stroke) }

    let withStrokeWidth strokeWidth style =
        { style with StrokeWidth = Some(strokeWidth) }

    let withOpacity opacity style =
        { style with Opacity = Some(opacity) }

    let create fill stroke strokeWidth opacity =
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity) }

    let createWithFill fill =
        empty |> withFill fill

    let createWithStroke stroke =
        empty |> withStroke stroke

    let createWithStrokeWidth strokeWidth =
        empty |> withStrokeWidth strokeWidth

    let createWithOpacity opacity =
        empty |> withOpacity opacity

    let private mapToString f style =
        [
            style.Stroke |> Option.map (Color.toString >> (f "stroke"));
            style.StrokeWidth |> Option.map (Length.toString >> (f "stroke-width"));
            style.Fill |> Option.map (Color.toString >> (f "fill"));
            style.Opacity |> Option.map (string >> (f "opacity"))
        ] |> List.choose id

    let toAttributes style =
        let stylePartToAttribute name value =
            Attribute.create name value
        mapToString stylePartToAttribute style |> Set.ofList

    let toString style =
        let stylePartToString name value =
           name + ":" + value
        mapToString stylePartToString style  |> String.concat ";"

    let toAttribute style =
        Attribute.create "style" (toString style)

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module NamedStyle =
    let ofStyle name style =
        { Name = name; Style = style }

    let toCssString namedStyle =
        "." + namedStyle.Name + "{" + (namedStyle.Style |> Style.toString) + "}"

    let toTag namedStyle =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.create "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (toCssString namedStyle) + "]]>")

    let toString = toTag >> Tag.toString


module Styles =
    let toTag styles =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.create "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (styles |> Seq.map NamedStyle.toCssString |> String.concat " ") + "]]>")

    let toString (styles:seq<NamedStyle>) =
        styles |> toTag |> Tag.toString