namespace SharpVG
open PointHelpers

type SvgText(text : Text) =
    inherit SvgElement(Element.PlainElement(BaseElement.Text(text)))

    override __.Name = "text"

    override __.Body = Some(text.Body)

    override __.Attributes =
        pointToDescriptiveString text.UpperLeft
