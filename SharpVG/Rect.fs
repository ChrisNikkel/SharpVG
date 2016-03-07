namespace SharpVG
open PointHelpers
open AreaHelpers

type SvgRect(rect : Rect) =
    inherit SvgElement(Element.PlainElement(BaseElement.Rect(rect)))

    member __.Rect = rect

    override __.Name = "rect"

    override __.Attributes =
        pointToDescriptiveString rect.UpperLeft + " " +
        areaToString rect.Size