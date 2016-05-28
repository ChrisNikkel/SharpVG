namespace SharpVG

type style =
    {
        fill : color;
        stroke : color;
        strokeWidth : size;
        opacity: double;
    }

module Style =
    let toString style =
        let stylePartToString name value =
           name + ":" + value + ";"
        "style=" + ((stylePartToString "stroke" <| Color.toString style.stroke)
            + (stylePartToString "stroke-width" <| Size.toString style.strokeWidth)
            + (stylePartToString "fill"  <| Color.toString style.fill)
            + (stylePartToString "opacity" <| string style.opacity)
        |> Tag.quote)