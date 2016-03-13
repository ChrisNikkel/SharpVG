namespace SharpVG
open PointHelpers

type SvgText(text : Text, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Text(text)), style)

    override __.Name = "text"

    override __.Body = Some(text.Body)

    override __.Attributes =
        pointToDescriptiveString text.UpperLeft
