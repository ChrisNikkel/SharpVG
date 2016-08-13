// TODO: Add anchor, decorator, spacing, and writing mode to text
// TODO: Add def and use to make reusable elements
namespace SharpVG

type Style =
    {
        Fill : Color option;
        Stroke : Color option;
        StrokeWidth : Length option;
        Opacity: float option;
        Name: string option;
    }

[<CompilationRepresentation (CompilationRepresentationFlags.ModuleSuffix)>]
module Style =
    let empty =
        { Fill = None; Stroke = None; StrokeWidth = None; Opacity = None; Name = None; }

    let withName name (style : Style) =
        { style with Name = Some(name) }

    let withFill fill style =
        { style with Fill = Some(fill) }

    let withStroke stroke style =
        { style with Stroke = Some(stroke) }

    let withStrokeWidth strokeWidth style =
        { style with StrokeWidth = Some(strokeWidth) }

    let withOpacity opacity style =
        { style with Opacity = Some(opacity) }

    // TODO: Style.create isn't consistant with the other creates which only take required values as parameters
    let create fill stroke strokeWidth opacity =
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); Name = None }

    let createNamed fill stroke strokeWidth opacity name =
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity); Name = Some(name) }

    let createWithFill fill =
        empty |> withFill fill

    let createWithStroke stroke =
        empty |> withStroke stroke

    let createWithStrokeWidth strokeWidth =
        empty |> withStrokeWidth strokeWidth

    let createWithOpacity opacity =
        empty |> withOpacity opacity

    let isNamed style =
        style.Name.IsSome

    let private mapToString f style =
        [
            style.Stroke |> Option.map (Color.toString >> (f "stroke"));
            style.StrokeWidth |> Option.map (Length.toString >> (f "stroke-width"));
            style.Fill |> Option.map (Color.toString >> (f "fill"));
            style.Opacity |> Option.map (string >> (f "opacity"))
        ] |> List.choose id

    let toAttributes style =
        mapToString Attribute.createCSS style

    let toString style =
        let stylePartToString name value =
           name + ":" + value
        mapToString stylePartToString style  |> String.concat ";"

    let toAttribute style =
        Attribute.createCSS "style" (toString style)

    let toCssString style =
        match style.Name with | Some name  -> "." + name + " " | None -> "" + "{" + (style |> toString) + "}"

    let toTag style =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.createCSS "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (toCssString style) + "]]>")

module Styles =
    let toTag styles =
        Tag.create "style"
        |> Tag.withAttribute (Attribute.createCSS "type" "text/css")
        |> Tag.withBody ("<![CDATA[" + (styles |> Seq.map Style.toCssString |> String.concat " ") + "]]>")

    let toString styles =
        styles |> toTag |> Tag.toString

    let named styles =
        styles |> Seq.filter Style.isNamed
