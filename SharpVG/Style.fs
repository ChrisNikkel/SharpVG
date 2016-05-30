namespace SharpVG

type style =
    {
        Fill : color option;
        Stroke : color option;
        StrokeWidth : length option;
        Opacity: double option;
    }

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

    let init fill stroke strokeWidth opacity =
        { Fill = Some(fill); Stroke = Some(stroke); StrokeWidth = Some(strokeWidth); Opacity = Some(opacity) }

    let initWithFill fill =
        empty |> withFill fill

    let initWithStroke stroke =
        empty |> withStroke stroke

    let initWithStrokeWidth strokeWidth =
        empty |> withStrokeWidth strokeWidth

    let initWithOpacity opacity =
        empty |> withOpacity opacity

    let private mapToString f separator style =
        [
            style.Stroke |> Option.map (Color.toString >> (f "stroke"));
            style.StrokeWidth |> Option.map (Length.toString >> (f "stroke-width"));
            style.Fill |> Option.map (Color.toString >> (f "fill");
            style.Opacity |> Option.map (string >> (f "opacity"))
        ] |> List.choose id |> String.concat separator

    let toString style =
        let stylePartToString name value =
            name + "=" + Tag.quote value
        mapToString stylePartToString " " style

    let toCssString style =
        let stylePartToString name value =
           name + ":" + value
        mapToString stylePartToString ";" style

    let toStyleString style =
        "style=" + (Tag.quote <| toCssString style)