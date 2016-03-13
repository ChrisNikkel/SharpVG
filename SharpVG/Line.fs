namespace SharpVG
open PointHelpers

type SvgLine(line : Line, style : Style option) =
    inherit SvgElement(Element.PlainElement(BaseElement.Line(line)), style)

    member __.Line = line

    override __.Name = "line"

    override __.Attributes =
        pointModifierToDescriptiveString line.Point1 "" "1" + " " +
        pointModifierToDescriptiveString line.Point2 "" "2"

// TODO: Move to base and change from interface to inheritance
//    override __.ToString() = (__ :> ElementBase).toString