namespace SharpVG
open Helpers
open PointHelpers

type SvgPolygon(points : Points) =
    inherit SvgElement(Element.PlainElement(BaseElement.Polygon(points)))

    override __.Name = "polygon"

    override __.Attributes = "points=" + quote (pointsToString points)