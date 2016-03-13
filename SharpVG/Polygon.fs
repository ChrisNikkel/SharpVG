namespace SharpVG
open Helpers
open PointHelpers

type SvgPolygon(points : Points, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Polygon(points)), style)

    override __.Name = "polygon"

    override __.Attributes = "points=" + quote (pointsToString points)