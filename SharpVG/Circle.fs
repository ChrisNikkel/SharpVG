namespace SharpVG
open Helpers
open PointHelpers
open SizeHelpers

type SvgCircle(circle : Circle) =
    inherit SvgElement(Element.PlainElement(BaseElement.Circle(circle)))

    member __.Circle = circle

    override __.Name = "circle"

    override __.Attributes =
        pointModifierToDescriptiveString circle.Center "c" "" +
        " r=" + quote (sizeToString circle.Radius)