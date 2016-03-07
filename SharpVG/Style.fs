namespace SharpVG
open Helpers
open SizeHelpers
open ColorHelpers

type SvgStyle(style : Style) =
    member __.Style = style
    member __.toString =
        "stroke=" + quote (colorToString style.Stroke) + " " +
        "stroke-width=" + quote (sizeToString style.StrokeWidth) + " " +
        "fill=" + quote (colorToString style.Fill)
    override __.ToString() = __.toString