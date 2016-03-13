namespace SharpVG
open Helpers
open PointHelpers

type SvgEllipse(ellipse : Ellipse, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Ellipse(ellipse)), style)

    new(ellipse : Ellipse) = SvgEllipse(ellipse, None)

    member __.Ellipse = ellipse

    override __.Name = "ellipse"

    override __.Attributes =
        pointModifierToDescriptiveString ellipse.Center "c" "" +
        " r=" + quote (pointToString ellipse.Radius)