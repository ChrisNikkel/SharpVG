namespace SharpVG

type Style = { Fill : Color; Stroke : Color; StrokeWidth : Size }

module StyleHelpers =
    open Helpers
    open SizeHelpers
    open ColorHelpers

    let styleToString style =
        "stroke=" + quote (colorToString style.Stroke) + " " +
        "stroke-width=" + quote (sizeToString style.StrokeWidth) + " " +
        "fill=" + quote (colorToString style.Fill)