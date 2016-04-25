namespace SharpVG

type style =
    {
        fill : color;
        stroke : color;
        strokeWidth : size;
    }

module Style =
    let toString style =
        "stroke=" + Tag.quote (Color.toString style.stroke) + " " +
        "stroke-width=" + Tag.quote (Size.toString style.strokeWidth) + " " +
        "fill=" + Tag.quote (Color.toString style.fill)
