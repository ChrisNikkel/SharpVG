namespace SharpVG
open Helpers
open PointHelpers
open SizeHelpers

type SvgCircle(circle : Circle, style : Style option) =
//TODO: FINISH UP MULTI CONSTRUCTORS
    inherit SvgElement(Element.PlainElement(BaseElement.Circle(circle)), style)
    new(circle : Circle) = SvgCircle(circle, None)

    member __.Circle = circle

    override __.Name = "circle"

    override __.Attributes =
        pointModifierToDescriptiveString circle.Center "c" "" +
        " r=" + quote (sizeToString circle.Radius)