namespace SharpVG
open PointHelpers
open AreaHelpers

type SvgRect(rect : Rect, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Rect(rect)), style)

    member __.Rect = rect

    override __.Name = "rect"

    override __.Attributes =
        pointToDescriptiveString rect.UpperLeft + " " +
        areaToString rect.Size