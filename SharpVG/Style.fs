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
        "stroke=" + Tag.quote (Color.toString style.stroke) + " " +
        "stroke-width=" + Tag.quote (Size.toString style.strokeWidth) + " " +
        "fill=" + Tag.quote (Color.toString style.fill) + " " +
        "opacity=" + Tag.quote (string style.opacity) + " "

    let toCssString style =
        let stylePartToString name value =
           name + ":" + value + ";"
        (stylePartToString "stroke" <| Color.toString style.stroke)
        + (stylePartToString "stroke-width" <| Size.toString style.strokeWidth)
        + (stylePartToString "fill"  <| Color.toString style.fill)
        + (stylePartToString "opacity" <| string style.opacity)


    let toStyleString style =
        "style=" + (Tag.quote <| toCssString style)