namespace SharpVG
open Helpers
open PointHelpers

type SvgPolyline(points : Points, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Polyline(points)), style)

    override __.Name = "polyline"

    override __.Attributes = "points=" + quote (pointsToString points)