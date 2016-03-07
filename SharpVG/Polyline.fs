namespace SharpVG
open Helpers
open PointHelpers

type SvgPolyline(points : Points) =
    inherit SvgElement(Element.PlainElement(BaseElement.Polyline(points)))

    override __.Name = "polyline"

    override __.Attributes = "points=" + quote (pointsToString points)