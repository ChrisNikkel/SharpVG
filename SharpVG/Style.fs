namespace SharpVG
open Helpers
open SizeHelpers
open ColorHelpers

type Style =
    {
        Fill : Color;
        Stroke : Color;
        StrokeWidth : Size;
    }
    member __.toString = 
        "stroke=" + quote (colorToString __.Stroke) + " " +
        "stroke-width=" + quote (sizeToString __.StrokeWidth) + " " +
        "fill=" + quote (colorToString __.Fill)
    override __.ToString() = __.toString